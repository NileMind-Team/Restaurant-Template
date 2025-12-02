using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NileFood.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddOffersTables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ItemOffers",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MenuItemId = table.Column<int>(type: "int", nullable: false),
                    IsPercentage = table.Column<bool>(type: "bit", nullable: false),
                    DiscountValue = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    StartDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EndDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsEnabled = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ItemOffers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ItemOffers_MenuItems_MenuItemId",
                        column: x => x.MenuItemId,
                        principalTable: "MenuItems",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "BranchItemOffers",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BranchId = table.Column<int>(type: "int", nullable: false),
                    ItemOfferId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BranchItemOffers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BranchItemOffers_Branches_BranchId",
                        column: x => x.BranchId,
                        principalTable: "Branches",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BranchItemOffers_ItemOffers_ItemOfferId",
                        column: x => x.ItemOfferId,
                        principalTable: "ItemOffers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_BranchItemOffers_BranchId",
                table: "BranchItemOffers",
                column: "BranchId");

            migrationBuilder.CreateIndex(
                name: "IX_BranchItemOffers_ItemOfferId",
                table: "BranchItemOffers",
                column: "ItemOfferId");

            migrationBuilder.CreateIndex(
                name: "IX_ItemOffers_MenuItemId",
                table: "ItemOffers",
                column: "MenuItemId");

            migrationBuilder.CreateIndex(
                name: "IX_ItemOffers_StartDate_EndDate",
                table: "ItemOffers",
                columns: new[] { "StartDate", "EndDate" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BranchItemOffers");

            migrationBuilder.DropTable(
                name: "ItemOffers");
        }
    }
}
