using Microsoft.EntityFrameworkCore;
using WarehouseManagerServer.Models;

namespace WarehouseManagerServer.Data;

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

    public virtual DbSet<UserPermission> UserPermissions { get; set; }

    public virtual DbSet<Warehouse> Warehouses { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder
            .HasPostgresEnum("movement_type_enum", new[] { "in", "out", "adjustment", "transfer", "remove" })
            .HasPostgresEnum("permission_enum", new[] { "in", "out", "adjustment", "transfer", "remove" });

        modelBuilder.Entity<Category>(entity =>
        {
            entity.HasKey(e => e.CategoryId).HasName("categories_pkey");

            entity.ToTable("categories");

            entity.Property(e => e.CategoryId).HasColumnName("category_id");
            entity.Property(e => e.Name).HasColumnName("name");
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
            entity.Property(e => e.MovementTypeEnum)
                .HasColumnName("movement_type")
                .HasColumnType("movement_type_enum");

            entity.HasOne(d => d.Product).WithMany(p => p.Movements)
                .HasForeignKey(d => d.ProductId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("movements_product_id_fkey");
        });

        modelBuilder.Entity<Product>(entity =>
        {
            entity.HasKey(e => e.ProductId).HasName("products_pkey");

            entity.ToTable("products");

            entity.HasIndex(e => e.CategoryId, "idx_products_category");

            entity.HasIndex(e => e.Sku, "products_sku_key").IsUnique();

            entity.Property(e => e.ProductId).HasColumnName("product_id");
            entity.Property(e => e.CategoryId).HasColumnName("category_id");
            entity.Property(e => e.Name).HasColumnName("name");
            entity.Property(e => e.Quantity)
                .HasDefaultValue(0)
                .HasColumnName("quantity");
            entity.Property(e => e.Sku)
                .HasMaxLength(50)
                .HasColumnName("sku");
            entity.Property(e => e.UnitPrice)
                .HasPrecision(12, 2)
                .HasDefaultValueSql("1")
                .HasColumnName("unit_price");

            entity.HasOne(d => d.Category).WithMany(p => p.Products)
                .HasForeignKey(d => d.CategoryId)
                .HasConstraintName("products_category_id_fkey");
            entity.HasOne(d => d.Warehouse).WithMany(p => p.Products)
                .HasForeignKey(d => d.WarehouseId)
                .HasConstraintName("products_warehouse_id_fkey");
        });

        modelBuilder.Entity<Supplier>(entity =>
        {
            entity.HasKey(e => e.SupplierId).HasName("suppliers_pkey");

            entity.ToTable("suppliers");

            entity.Property(e => e.SupplierId).HasColumnName("supplier_id");
            entity.Property(e => e.ContactInfo).HasColumnName("contact_info");
            entity.Property(e => e.Name).HasColumnName("name");
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
            entity.Property(e => e.Salt).HasColumnName("salt");
            entity.Property(e => e.Username)
                .HasMaxLength(40)
                .HasColumnName("username");
            entity.HasMany(e => e.Warehouses)
                .WithMany(p => p.Users)
                .UsingEntity(j => j.ToTable("UserWarehouses"));
        });

        modelBuilder.Entity<UserPermission>(entity =>
        {
            entity.HasKey(e => new { e.UserId, e.WarehouseId }).HasName("user_permissions_pkey");

            entity.ToTable("user_permissions");

            entity.Property(e => e.UserId).HasColumnName("user_id");
            entity.Property(e => e.WarehouseId).HasColumnName("warehouse_id");
            entity.Property(e => e.PermissionEnum)
                .HasColumnName("permissions")
                .HasColumnType("permission_enum[]");
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

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}