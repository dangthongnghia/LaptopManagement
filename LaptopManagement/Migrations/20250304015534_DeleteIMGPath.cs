using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LaptopManagement.Migrations
{
    /// <inheritdoc />
    public partial class DeleteIMGPath : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ImagePath",
                table: "Laptops");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ImagePath",
                table: "Laptops",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
