using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LoveShop.Migrations
{
    /// <inheritdoc />
    public partial class ChangeRowVersionName : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "RowVersion",
                table: "suppliers",
                newName: "row_version");

            migrationBuilder.RenameColumn(
                name: "RowVersion",
                table: "products_in_cart",
                newName: "row_version");

            migrationBuilder.RenameColumn(
                name: "RowVersion",
                table: "products",
                newName: "row_version");

            migrationBuilder.RenameColumn(
                name: "RowVersion",
                table: "product_categories",
                newName: "row_version");

            migrationBuilder.RenameColumn(
                name: "RowVersion",
                table: "orders",
                newName: "row_version");

            migrationBuilder.RenameColumn(
                name: "RowVersion",
                table: "order_items",
                newName: "row_version");

            migrationBuilder.RenameColumn(
                name: "RowVersion",
                table: "customers",
                newName: "row_version");

            migrationBuilder.RenameColumn(
                name: "RowVersion",
                table: "categories",
                newName: "row_version");

            migrationBuilder.RenameColumn(
                name: "RowVersion",
                table: "carts",
                newName: "row_version");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "row_version",
                table: "suppliers",
                newName: "RowVersion");

            migrationBuilder.RenameColumn(
                name: "row_version",
                table: "products_in_cart",
                newName: "RowVersion");

            migrationBuilder.RenameColumn(
                name: "row_version",
                table: "products",
                newName: "RowVersion");

            migrationBuilder.RenameColumn(
                name: "row_version",
                table: "product_categories",
                newName: "RowVersion");

            migrationBuilder.RenameColumn(
                name: "row_version",
                table: "orders",
                newName: "RowVersion");

            migrationBuilder.RenameColumn(
                name: "row_version",
                table: "order_items",
                newName: "RowVersion");

            migrationBuilder.RenameColumn(
                name: "row_version",
                table: "customers",
                newName: "RowVersion");

            migrationBuilder.RenameColumn(
                name: "row_version",
                table: "categories",
                newName: "RowVersion");

            migrationBuilder.RenameColumn(
                name: "row_version",
                table: "carts",
                newName: "RowVersion");
        }
    }
}
