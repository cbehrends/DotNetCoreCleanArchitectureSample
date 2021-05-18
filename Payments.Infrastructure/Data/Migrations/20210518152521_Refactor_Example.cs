using Microsoft.EntityFrameworkCore.Migrations;

namespace Payments.Infrastructure.Data.Migrations
{
    public partial class Refactor_Example : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ClaimId",
                table: "Payments",
                newName: "OrderId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "OrderId",
                table: "Payments",
                newName: "ClaimId");
        }
    }
}
