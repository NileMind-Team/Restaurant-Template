using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NileFood.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddBranchMenuItemOptionsTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MenuItemOptions_MenuItems_MenuItemId",
                table: "MenuItemOptions");

            migrationBuilder.DropIndex(
                name: "IX_MenuItemOptions_MenuItemId",
                table: "MenuItemOptions");

            migrationBuilder.DropColumn(
                name: "MenuItemId",
                table: "MenuItemOptions");

            migrationBuilder.CreateTable(
                name: "BranchMenuItemOptions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BranchMenuItemId = table.Column<int>(type: "int", nullable: false),
                    MenuItemOptionId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BranchMenuItemOptions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BranchMenuItemOptions_BranchMenuItems_BranchMenuItemId",
                        column: x => x.BranchMenuItemId,
                        principalTable: "BranchMenuItems",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BranchMenuItemOptions_MenuItemOptions_MenuItemOptionId",
                        column: x => x.MenuItemOptionId,
                        principalTable: "MenuItemOptions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_BranchMenuItemOptions_BranchMenuItemId",
                table: "BranchMenuItemOptions",
                column: "BranchMenuItemId");

            migrationBuilder.CreateIndex(
                name: "IX_BranchMenuItemOptions_MenuItemOptionId",
                table: "BranchMenuItemOptions",
                column: "MenuItemOptionId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BranchMenuItemOptions");

            migrationBuilder.AddColumn<int>(
                name: "MenuItemId",
                table: "MenuItemOptions",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_MenuItemOptions_MenuItemId",
                table: "MenuItemOptions",
                column: "MenuItemId");

            migrationBuilder.AddForeignKey(
                name: "FK_MenuItemOptions_MenuItems_MenuItemId",
                table: "MenuItemOptions",
                column: "MenuItemId",
                principalTable: "MenuItems",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
