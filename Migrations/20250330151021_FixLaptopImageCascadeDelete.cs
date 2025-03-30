using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LaptopManagement.Migrations
{
    /// <inheritdoc />
    public partial class FixLaptopImageCascadeDelete : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CartItems_Laptops_LaptopId",
                table: "CartItems");

            migrationBuilder.DropForeignKey(
                name: "FK_LaptopImages_Laptops_LaptopId",
                table: "LaptopImages");

            migrationBuilder.AddForeignKey(
                name: "FK_CartItems_Laptops_LaptopId",
                table: "CartItems",
                column: "LaptopId",
                principalTable: "Laptops",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_LaptopImages_Laptops_LaptopId",
                table: "LaptopImages",
                column: "LaptopId",
                principalTable: "Laptops",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CartItems_Laptops_LaptopId",
                table: "CartItems");

            migrationBuilder.DropForeignKey(
                name: "FK_LaptopImages_Laptops_LaptopId",
                table: "LaptopImages");

            migrationBuilder.AddForeignKey(
                name: "FK_CartItems_Laptops_LaptopId",
                table: "CartItems",
                column: "LaptopId",
                principalTable: "Laptops",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_LaptopImages_Laptops_LaptopId",
                table: "LaptopImages",
                column: "LaptopId",
                principalTable: "Laptops",
                principalColumn: "Id");
        }
    }
}
