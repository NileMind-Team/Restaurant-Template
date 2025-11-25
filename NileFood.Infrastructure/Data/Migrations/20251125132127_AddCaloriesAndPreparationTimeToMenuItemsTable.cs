using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NileFood.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddCaloriesAndPreparationTimeToMenuItemsTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Calories",
                table: "MenuItems",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "PreparationTimeEnd",
                table: "MenuItems",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "PreparationTimeStart",
                table: "MenuItems",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Calories",
                table: "MenuItems");

            migrationBuilder.DropColumn(
                name: "PreparationTimeEnd",
                table: "MenuItems");

            migrationBuilder.DropColumn(
                name: "PreparationTimeStart",
                table: "MenuItems");
        }
    }
}
