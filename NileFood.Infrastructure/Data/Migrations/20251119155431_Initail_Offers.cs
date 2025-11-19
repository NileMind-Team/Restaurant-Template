using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NileFood.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class Initail_Offers : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Offers",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    ImageUrl = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    DiscountType = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    DiscountValue = table.Column<decimal>(type: "decimal(10,2)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    StartDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EndDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UsageLimit = table.Column<int>(type: "int", nullable: true),
                    UsageCount = table.Column<int>(type: "int", nullable: false),
                    MinOrderAmount = table.Column<decimal>(type: "decimal(10,2)", nullable: true),
                    Level = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Offers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "OfferTargets",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    OfferId = table.Column<int>(type: "int", nullable: false),
                    BranchId = table.Column<int>(type: "int", nullable: true),
                    CategoryId = table.Column<int>(type: "int", nullable: true),
                    MenuItemId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OfferTargets", x => x.Id);
                    table.ForeignKey(
                        name: "FK_OfferTargets_Branches_BranchId",
                        column: x => x.BranchId,
                        principalTable: "Branches",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_OfferTargets_Categories_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "Categories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_OfferTargets_MenuItems_MenuItemId",
                        column: x => x.MenuItemId,
                        principalTable: "MenuItems",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_OfferTargets_Offers_OfferId",
                        column: x => x.OfferId,
                        principalTable: "Offers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Offers_Level",
                table: "Offers",
                column: "Level");

            migrationBuilder.CreateIndex(
                name: "IX_Offers_StartDate_EndDate",
                table: "Offers",
                columns: new[] { "StartDate", "EndDate" });

            migrationBuilder.CreateIndex(
                name: "IX_OfferTarget_Composite",
                table: "OfferTargets",
                columns: new[] { "OfferId", "BranchId", "CategoryId", "MenuItemId" });

            migrationBuilder.CreateIndex(
                name: "IX_OfferTargets_BranchId",
                table: "OfferTargets",
                column: "BranchId");

            migrationBuilder.CreateIndex(
                name: "IX_OfferTargets_CategoryId",
                table: "OfferTargets",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_OfferTargets_MenuItemId",
                table: "OfferTargets",
                column: "MenuItemId");

            migrationBuilder.CreateIndex(
                name: "IX_OfferTargets_OfferId",
                table: "OfferTargets",
                column: "OfferId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "OfferTargets");

            migrationBuilder.DropTable(
                name: "Offers");
        }
    }
}
