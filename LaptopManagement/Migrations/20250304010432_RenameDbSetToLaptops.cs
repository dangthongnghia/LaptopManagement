using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LaptopManagement.Migrations
{
    /// <inheritdoc />
    public partial class RenameDbSetToLaptops : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_LaptopImages_Laptop_LaptopId",
                table: "LaptopImages");

            migrationBuilder.DropForeignKey(
                name: "FK_Order_Laptop_LaptopId",
                table: "Order");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Order",
                table: "Order");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Laptop",
                table: "Laptop");

            migrationBuilder.RenameTable(
                name: "Order",
                newName: "Orders");

            migrationBuilder.RenameTable(
                name: "Laptop",
                newName: "Laptops");

            migrationBuilder.RenameIndex(
                name: "IX_Order_LaptopId",
                table: "Orders",
                newName: "IX_Orders_LaptopId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Orders",
                table: "Orders",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Laptops",
                table: "Laptops",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_LaptopImages_Laptops_LaptopId",
                table: "LaptopImages",
                column: "LaptopId",
                principalTable: "Laptops",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Orders_Laptops_LaptopId",
                table: "Orders",
                column: "LaptopId",
                principalTable: "Laptops",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_LaptopImages_Laptops_LaptopId",
                table: "LaptopImages");

            migrationBuilder.DropForeignKey(
                name: "FK_Orders_Laptops_LaptopId",
                table: "Orders");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Orders",
                table: "Orders");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Laptops",
                table: "Laptops");

            migrationBuilder.RenameTable(
                name: "Orders",
                newName: "Order");

            migrationBuilder.RenameTable(
                name: "Laptops",
                newName: "Laptop");

            migrationBuilder.RenameIndex(
                name: "IX_Orders_LaptopId",
                table: "Order",
                newName: "IX_Order_LaptopId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Order",
                table: "Order",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Laptop",
                table: "Laptop",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_LaptopImages_Laptop_LaptopId",
                table: "LaptopImages",
                column: "LaptopId",
                principalTable: "Laptop",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Order_Laptop_LaptopId",
                table: "Order",
                column: "LaptopId",
                principalTable: "Laptop",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
