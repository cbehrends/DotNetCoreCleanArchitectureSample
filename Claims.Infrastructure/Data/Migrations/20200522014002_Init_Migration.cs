using Microsoft.EntityFrameworkCore.Migrations;

namespace Claims.Infrastructure.Data.Migrations
{
    public partial class Init_Migration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                "Claims",
                table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FirstName = table.Column<string>(maxLength: 50, nullable: false)
                },
                constraints: table => { table.PrimaryKey("PK_Claims", x => x.Id); });

            migrationBuilder.CreateTable(
                "Services",
                table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Description = table.Column<string>(maxLength: 50, nullable: false)
                },
                constraints: table => { table.PrimaryKey("PK_Services", x => x.Id); });

            migrationBuilder.CreateTable(
                "RenderedServices",
                table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ClaimId = table.Column<int>(nullable: false),
                    ServiceId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RenderedServices", x => x.Id);
                    table.ForeignKey(
                        "FK_RenderedServices_Claims_ClaimId",
                        x => x.ClaimId,
                        "Claims",
                        "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        "FK_RenderedServices_Services_ServiceId",
                        x => x.ServiceId,
                        "Services",
                        "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                "IX_RenderedServices_ClaimId",
                "RenderedServices",
                "ClaimId");

            migrationBuilder.CreateIndex(
                "IX_RenderedServices_ServiceId",
                "RenderedServices",
                "ServiceId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                "RenderedServices");

            migrationBuilder.DropTable(
                "Claims");

            migrationBuilder.DropTable(
                "Services");
        }
    }
}