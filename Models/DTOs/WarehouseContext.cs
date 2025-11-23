using Microsoft.EntityFrameworkCore;
using WarehouseManagerServer.Models.Entities;

namespace WarehouseManagerServer.Models.DTOs;

public partial class WarehouseContext : DbContext
{
    public WarehouseContext()
    {
    }

    public WarehouseContext(DbContextOptions<WarehouseContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Category> Categories { get; set; }

    public virtual DbSet<Movement> Movements { get; set; }

    public virtual DbSet<Product> Products { get; set; }

    public virtual DbSet<Supplier> Suppliers { get; set; }

    public virtual DbSet<User> Users { get; set; }

    public virtual DbSet<Permission> Permissions { get; set; }

    public virtual DbSet<Warehouse> Warehouses { get; set; }
    
    public virtual DbSet<RecoveryCode> RecoveryCodes { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder
            .HasPostgresEnum("movement_type_enum", ["in", "out", "adjustment", "transfer", "remove"])
            .HasPostgresEnum("permission_enum", ["read", "write", "delete", "owner"]);

        modelBuilder.Entity<Category>(entity =>
        {
            entity.HasKey(e => e.CategoryId).HasName("categories_pkey");

            entity.ToTable("categories");

            entity.Property(e => e.CategoryId).HasColumnName("category_id");
            entity.Property(e => e.WarehouseId).HasColumnName("warehouse_id");
            entity.Property(e => e.Name).HasColumnName("name");

            entity.HasOne(d => d.Warehouse).WithMany(p => p.Categories)
                .HasForeignKey(d => d.WarehouseId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("category_warehouse_id_fkey");
        });

        modelBuilder.Entity<Movement>(entity =>
        {
            entity.HasKey(e => e.MovementId).HasName("movements_pkey");

            entity.ToTable("movements");

            entity.HasIndex(e => e.ProductId, "idx_movements_product");

            entity.Property(e => e.MovementId).HasColumnName("movement_id");
            entity.Property(e => e.Date)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnName("date");
            entity.Property(e => e.ProductId).HasColumnName("product_id");
            entity.Property(e => e.Quantity).HasColumnName("quantity");
            entity.Property(e => e.MovementType)
                .HasColumnName("movement_type")
                .HasColumnType("movement_type_enum");

            entity.HasOne(d => d.Product).WithMany(p => p.Movements)
                .HasForeignKey(d => d.ProductId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("movement_product_id_fkey");
        });

        modelBuilder.Entity<Product>(entity =>
        {
            entity.HasKey(e => e.ProductId).HasName("products_pkey");

            entity.ToTable("products");

            entity.HasIndex(e => e.CategoryId, "idx_products_category");

            entity.Property(e => e.ProductId).HasColumnName("product_id");
            entity.Property(e => e.WarehouseId).HasColumnName("warehouse_id");
            entity.Property(e => e.SupplierId).HasColumnName("supplier_id");
            entity.Property(e => e.CategoryId).HasColumnName("category_id");
            entity.Property(e => e.Name).HasColumnName("name");
            entity.Property(e => e.Quantity)
                .HasDefaultValue(0)
                .HasColumnName("quantity");
            entity.Property(e => e.UnitPrice)
                .HasPrecision(12, 2)
                .HasDefaultValueSql("1")
                .HasColumnName("unit_price");
            entity.Property(e => e.ExpiryDate)
                .HasColumnName("expiry_date");

            entity.HasOne(d => d.Warehouse).WithMany(p => p.Products)
                .HasForeignKey(d => d.WarehouseId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("product_warehouse_id_fkey");
            entity.HasOne(d => d.Category).WithMany(p => p.Products)
                .HasForeignKey(d => d.CategoryId)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("product_category_id_fkey");
            entity.HasOne(d => d.Supplier).WithMany(p => p.Products)
                .HasForeignKey(d => d.SupplierId)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("product_supplier_id_fkey");
        });

        modelBuilder.Entity<Supplier>(entity =>
        {
            entity.HasKey(e => e.SupplierId).HasName("suppliers_pkey");

            entity.ToTable("suppliers");

            entity.Property(e => e.SupplierId).HasColumnName("supplier_id");
            entity.Property(e => e.WarehouseId).HasColumnName("warehouse_id");
            entity.Property(e => e.ContactInfo).HasColumnName("contact_info");
            entity.Property(e => e.Name).HasColumnName("name");

            entity.HasOne(e => e.Warehouse).WithMany(d => d.Suppliers)
                .HasForeignKey(e => e.WarehouseId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("supplier_warehouse_id_fkey");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.UserId).HasName("users_pkey");

            entity.ToTable("users");

            entity.Property(e => e.UserId).HasColumnName("user_id");
            entity.Property(e => e.Email)
                .HasMaxLength(40)
                .HasColumnName("email");
            entity.Property(e => e.JoinDate)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnName("join_date");
            entity.Property(e => e.PasswordHash).HasColumnName("password_hash");
            entity.Property(e => e.FullName)
                .HasMaxLength(40)
                .HasColumnName("fullname");
            
            entity.HasIndex(e => e.Email).IsUnique();
            
            entity.HasMany(e => e.Warehouses)
                .WithMany(p => p.Users)
                .UsingEntity(j => j.ToTable("UserWarehouses"));
        });

        modelBuilder.Entity<Permission>(entity =>
        {
            entity.HasKey(e => new { e.UserId, e.WarehouseId }).HasName("permissions_pkey");

            entity.ToTable("permissions");

            entity.Property(e => e.UserId).HasColumnName("user_id");
            entity.Property(e => e.WarehouseId).HasColumnName("warehouse_id");
            entity.Property(e => e.UserPermissions)
                .HasColumnName("user_permissions")
                .HasColumnType("permission_enum[]");
            
            entity.HasOne(d => d.User).WithMany(p => p.Permissions)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("permission_user_id_fkey");
            entity.HasOne(d => d.Warehouse).WithMany(p => p.Permissions)
                .HasForeignKey(d => d.WarehouseId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("permission_warehouse_id_fkey");
        });

        modelBuilder.Entity<Warehouse>(entity =>
        {
            entity.HasKey(e => e.WarehouseId).HasName("warehouses_pkey");

            entity.ToTable("warehouses");

            entity.Property(e => e.WarehouseId).HasColumnName("warehouse_id");
            entity.Property(e => e.Name)
                .HasMaxLength(40)
                .HasColumnName("name");
            entity.HasMany(e => e.Users)
                .WithMany(p => p.Warehouses);
        });

        modelBuilder.Entity<RecoveryCode>(entity =>
        {
            entity.HasKey(e => e.CodeId).HasName("recovery_code_pkey");

            entity.ToTable("recovery_code");

            entity.Property(e => e.CodeId).HasColumnName("code_id");
            entity.Property(e => e.UserId)
                .HasColumnName("user_id");
            entity.Property(e => e.Code)
                .HasMaxLength(7)
                .HasColumnName("code");
            entity.Property(e => e.CreatedAt)
                .HasColumnName("created_at");
            
            entity.HasIndex(e => e.Code).IsUnique();
            
            entity.HasOne(e => e.User)
                .WithOne(p => p.RecoveryCode)
                .HasForeignKey<RecoveryCode>(e => e.UserId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    public DbSet<RefreshToken> RefreshTokens => Set<RefreshToken>();

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}