using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WarehouseManagerServer.Migrations
{
    /// <inheritdoc />
    public partial class FixHistoriesTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "history_product_id_fkey",
                table: "histories");

            migrationBuilder.DropForeignKey(
                name: "history_user_id_fkey",
                table: "histories");

            migrationBuilder.RenameColumn(
                name: "user_id",
                table: "histories",
                newName: "UserId");

            migrationBuilder.RenameColumn(
                name: "product_id",
                table: "histories",
                newName: "ProductId");

            migrationBuilder.RenameIndex(
                name: "IX_histories_user_id",
                table: "histories",
                newName: "IX_histories_UserId");

            migrationBuilder.RenameIndex(
                name: "idx_histories_product",
                table: "histories",
                newName: "IX_histories_ProductId");

            migrationBuilder.AlterColumn<int>(
                name: "UserId",
                table: "histories",
                type: "integer",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AlterColumn<int>(
                name: "ProductId",
                table: "histories",
                type: "integer",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AddColumn<string>(
                name: "product_name",
                table: "histories",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "supplier_name",
                table: "histories",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "user_full_name",
                table: "histories",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "warehouse_id",
                table: "histories",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_histories_warehouse_id",
                table: "histories",
                column: "warehouse_id");

            migrationBuilder.AddForeignKey(
                name: "FK_histories_products_ProductId",
                table: "histories",
                column: "ProductId",
                principalTable: "products",
                principalColumn: "product_id");

            migrationBuilder.AddForeignKey(
                name: "FK_histories_users_UserId",
                table: "histories",
                column: "UserId",
                principalTable: "users",
                principalColumn: "user_id");

            migrationBuilder.AddForeignKey(
                name: "history_warehouse_id_fkey",
                table: "histories",
                column: "warehouse_id",
                principalTable: "warehouses",
                principalColumn: "warehouse_id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_histories_products_ProductId",
                table: "histories");

            migrationBuilder.DropForeignKey(
                name: "FK_histories_users_UserId",
                table: "histories");

            migrationBuilder.DropForeignKey(
                name: "history_warehouse_id_fkey",
                table: "histories");

            migrationBuilder.DropIndex(
                name: "IX_histories_warehouse_id",
                table: "histories");

            migrationBuilder.DropColumn(
                name: "product_name",
                table: "histories");

            migrationBuilder.DropColumn(
                name: "supplier_name",
                table: "histories");

            migrationBuilder.DropColumn(
                name: "user_full_name",
                table: "histories");

            migrationBuilder.DropColumn(
                name: "warehouse_id",
                table: "histories");

            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "histories",
                newName: "user_id");

            migrationBuilder.RenameColumn(
                name: "ProductId",
                table: "histories",
                newName: "product_id");

            migrationBuilder.RenameIndex(
                name: "IX_histories_UserId",
                table: "histories",
                newName: "IX_histories_user_id");

            migrationBuilder.RenameIndex(
                name: "IX_histories_ProductId",
                table: "histories",
                newName: "idx_histories_product");

            migrationBuilder.AlterColumn<int>(
                name: "user_id",
                table: "histories",
                type: "integer",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "product_id",
                table: "histories",
                type: "integer",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "history_product_id_fkey",
                table: "histories",
                column: "product_id",
                principalTable: "products",
                principalColumn: "product_id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "history_user_id_fkey",
                table: "histories",
                column: "user_id",
                principalTable: "users",
                principalColumn: "user_id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
