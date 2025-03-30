using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LaptopManagement.Migrations
{
    public partial class UpdateCartItem : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Example: Add a new column to the CartItem table
            migrationBuilder.Sql("ALTER TABLE CartItems ADD COLUMN NewColumn NVARCHAR(100)");

            // Example: Update existing data in the CartItem table
            migrationBuilder.Sql("UPDATE CartItems SET NewColumn = 'DefaultValue' WHERE NewColumn IS NULL");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // Example: Remove the new column from the CartItem table
            migrationBuilder.Sql("ALTER TABLE CartItems DROP COLUMN NewColumn");
        }
    }
}