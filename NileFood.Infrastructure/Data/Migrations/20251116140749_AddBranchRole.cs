using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NileFood.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddBranchRole : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "feb07bce-2808-40bb-8561-9b95ed1d2816", "644f048f-1dcf-4b6c-b21c-55a24018046e", "Branch", "BRANCH" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "feb07bce-2808-40bb-8561-9b95ed1d2816");
        }
    }
}
