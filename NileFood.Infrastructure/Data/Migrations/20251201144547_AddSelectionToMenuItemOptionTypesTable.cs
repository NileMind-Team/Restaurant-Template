using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NileFood.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddSelectionToMenuItemOptionTypesTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "CanSelectMultipleOptions",
                table: "MenuItemOptionTypes",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsSelectionRequired",
                table: "MenuItemOptionTypes",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CanSelectMultipleOptions",
                table: "MenuItemOptionTypes");

            migrationBuilder.DropColumn(
                name: "IsSelectionRequired",
                table: "MenuItemOptionTypes");
        }
    }
}
