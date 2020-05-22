using Microsoft.EntityFrameworkCore.Migrations;

namespace Claims.Infrastructure.Data.Migrations
{
    public partial class FK_RenderedService_To_Service : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                "FK_RenderedServices_Services_ServiceId",
                "RenderedServices");

            migrationBuilder.AddForeignKey(
                "FK_RenderedServices_Services_ServiceId",
                "RenderedServices",
                "ServiceId",
                "Services",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                "FK_RenderedServices_Services_ServiceId",
                "RenderedServices");

            migrationBuilder.AddForeignKey(
                "FK_RenderedServices_Services_ServiceId",
                "RenderedServices",
                "ServiceId",
                "Services",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}