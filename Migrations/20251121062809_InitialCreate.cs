using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace WarehouseManagerServer.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterDatabase()
                .Annotation("Npgsql:Enum:movement_type_enum", "in,out,adjustment,transfer,remove")
                .Annotation("Npgsql:Enum:permission_enum", "read,write,delete,owner");

            migrationBuilder.CreateTable(
                name: "users",
                columns: table => new
                {
                    user_id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    username = table.Column<string>(type: "character varying(40)", maxLength: 40, nullable: false),
                    email = table.Column<string>(type: "character varying(40)", maxLength: 40, nullable: false),
                    password_hash = table.Column<string>(type: "text", nullable: false),
                    join_date = table.Column<DateTime>(type: "timestamp with time zone", nullable: true, defaultValueSql: "CURRENT_TIMESTAMP")
                },
                constraints: table =>
                {
                    table.PrimaryKey("users_pkey", x => x.user_id);
                });

            migrationBuilder.CreateTable(
                name: "warehouses",
                columns: table => new
                {
                    warehouse_id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    name = table.Column<string>(type: "character varying(40)", maxLength: 40, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("warehouses_pkey", x => x.warehouse_id);
                });

            migrationBuilder.CreateTable(
                name: "RefreshTokens",
                columns: table => new
                {
                    TokenId = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    UserId = table.Column<int>(type: "integer", nullable: false),
                    Token = table.Column<string>(type: "text", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    ExpiresAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    RevokedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RefreshTokens", x => x.TokenId);
                    table.ForeignKey(
                        name: "FK_RefreshTokens_users_UserId",
                        column: x => x.UserId,
                        principalTable: "users",
                        principalColumn: "user_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "categories",
                columns: table => new
                {
                    category_id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    warehouse_id = table.Column<int>(type: "integer", nullable: false),
                    name = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("categories_pkey", x => x.category_id);
                    table.ForeignKey(
                        name: "category_warehouse_id_fkey",
                        column: x => x.warehouse_id,
                        principalTable: "warehouses",
                        principalColumn: "warehouse_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "permissions",
                columns: table => new
                {
                    user_id = table.Column<int>(type: "integer", nullable: false),
                    warehouse_id = table.Column<int>(type: "integer", nullable: false),
                    user_permissions = table.Column<int[]>(type: "permission_enum[]", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("permissions_pkey", x => new { x.user_id, x.warehouse_id });
                    table.ForeignKey(
                        name: "permission_user_id_fkey",
                        column: x => x.user_id,
                        principalTable: "users",
                        principalColumn: "user_id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "permission_warehouse_id_fkey",
                        column: x => x.warehouse_id,
                        principalTable: "warehouses",
                        principalColumn: "warehouse_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "suppliers",
                columns: table => new
                {
                    supplier_id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    warehouse_id = table.Column<int>(type: "integer", nullable: false),
                    name = table.Column<string>(type: "text", nullable: false),
                    contact_info = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("suppliers_pkey", x => x.supplier_id);
                    table.ForeignKey(
                        name: "supplier_warehouse_id_fkey",
                        column: x => x.warehouse_id,
                        principalTable: "warehouses",
                        principalColumn: "warehouse_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserWarehouses",
                columns: table => new
                {
                    UsersUserId = table.Column<int>(type: "integer", nullable: false),
                    WarehousesWarehouseId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserWarehouses", x => new { x.UsersUserId, x.WarehousesWarehouseId });
                    table.ForeignKey(
                        name: "FK_UserWarehouses_users_UsersUserId",
                        column: x => x.UsersUserId,
                        principalTable: "users",
                        principalColumn: "user_id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserWarehouses_warehouses_WarehousesWarehouseId",
                        column: x => x.WarehousesWarehouseId,
                        principalTable: "warehouses",
                        principalColumn: "warehouse_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "products",
                columns: table => new
                {
                    product_id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    name = table.Column<string>(type: "text", nullable: false),
                    warehouse_id = table.Column<int>(type: "integer", nullable: false),
                    supplier_id = table.Column<int>(type: "integer", nullable: true),
                    category_id = table.Column<int>(type: "integer", nullable: true),
                    unit_price = table.Column<decimal>(type: "numeric(12,2)", precision: 12, scale: 2, nullable: false, defaultValueSql: "1"),
                    quantity = table.Column<int>(type: "integer", nullable: false, defaultValue: 0),
                    expiry_date = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("products_pkey", x => x.product_id);
                    table.ForeignKey(
                        name: "product_category_id_fkey",
                        column: x => x.category_id,
                        principalTable: "categories",
                        principalColumn: "category_id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "product_supplier_id_fkey",
                        column: x => x.supplier_id,
                        principalTable: "suppliers",
                        principalColumn: "supplier_id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "product_warehouse_id_fkey",
                        column: x => x.warehouse_id,
                        principalTable: "warehouses",
                        principalColumn: "warehouse_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "movements",
                columns: table => new
                {
                    movement_id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    product_id = table.Column<int>(type: "integer", nullable: false),
                    quantity = table.Column<int>(type: "integer", nullable: false),
                    movement_type = table.Column<int>(type: "movement_type_enum", nullable: false),
                    date = table.Column<DateTime>(type: "timestamp with time zone", nullable: true, defaultValueSql: "CURRENT_TIMESTAMP")
                },
                constraints: table =>
                {
                    table.PrimaryKey("movements_pkey", x => x.movement_id);
                    table.ForeignKey(
                        name: "movement_product_id_fkey",
                        column: x => x.product_id,
                        principalTable: "products",
                        principalColumn: "product_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_categories_warehouse_id",
                table: "categories",
                column: "warehouse_id");

            migrationBuilder.CreateIndex(
                name: "idx_movements_product",
                table: "movements",
                column: "product_id");

            migrationBuilder.CreateIndex(
                name: "IX_permissions_warehouse_id",
                table: "permissions",
                column: "warehouse_id");

            migrationBuilder.CreateIndex(
                name: "idx_products_category",
                table: "products",
                column: "category_id");

            migrationBuilder.CreateIndex(
                name: "IX_products_supplier_id",
                table: "products",
                column: "supplier_id");

            migrationBuilder.CreateIndex(
                name: "IX_products_warehouse_id",
                table: "products",
                column: "warehouse_id");

            migrationBuilder.CreateIndex(
                name: "IX_RefreshTokens_UserId",
                table: "RefreshTokens",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_suppliers_warehouse_id",
                table: "suppliers",
                column: "warehouse_id");

            migrationBuilder.CreateIndex(
                name: "IX_users_email",
                table: "users",
                column: "email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_users_username",
                table: "users",
                column: "username",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_UserWarehouses_WarehousesWarehouseId",
                table: "UserWarehouses",
                column: "WarehousesWarehouseId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "movements");

            migrationBuilder.DropTable(
                name: "permissions");

            migrationBuilder.DropTable(
                name: "RefreshTokens");

            migrationBuilder.DropTable(
                name: "UserWarehouses");

            migrationBuilder.DropTable(
                name: "products");

            migrationBuilder.DropTable(
                name: "users");

            migrationBuilder.DropTable(
                name: "categories");

            migrationBuilder.DropTable(
                name: "suppliers");

            migrationBuilder.DropTable(
                name: "warehouses");
        }
    }
}
