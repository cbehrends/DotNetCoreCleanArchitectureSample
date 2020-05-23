using Microsoft.EntityFrameworkCore.Migrations;

namespace Claims.Infrastructure.Data.Migrations
{
    public partial class Unique_Constraint_To_Service_Description : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Services_Description",
                table: "Services",
                column: "Description",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Services_Description",
                table: "Services");
        }
    }
}
