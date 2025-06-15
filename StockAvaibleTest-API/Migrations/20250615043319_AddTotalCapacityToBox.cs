using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace StockAvaibleTest_API.Migrations
{
    /// <inheritdoc />
    public partial class AddTotalCapacityToBox : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "TotalCapacity",
                table: "Boxes",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TotalCapacity",
                table: "Boxes");
        }
    }
}
