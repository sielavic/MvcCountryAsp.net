using Microsoft.EntityFrameworkCore.Migrations;

namespace MvcСountry.Migrations
{
    public partial class CreateRegionsCity : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Capital",
                table: "Country");

            migrationBuilder.DropColumn(
                name: "Region",
                table: "Country");

            migrationBuilder.AddColumn<int>(
                name: "CapitalId",
                table: "Country",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "RegionId",
                table: "Country",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "City",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_City", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Regions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Regions", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Country_CapitalId",
                table: "Country",
                column: "CapitalId");

            migrationBuilder.CreateIndex(
                name: "IX_Country_RegionId",
                table: "Country",
                column: "RegionId");

            migrationBuilder.AddForeignKey(
                name: "FK_Country_City_CapitalId",
                table: "Country",
                column: "CapitalId",
                principalTable: "City",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Country_Regions_RegionId",
                table: "Country",
                column: "RegionId",
                principalTable: "Regions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Country_City_CapitalId",
                table: "Country");

            migrationBuilder.DropForeignKey(
                name: "FK_Country_Regions_RegionId",
                table: "Country");

            migrationBuilder.DropTable(
                name: "City");

            migrationBuilder.DropTable(
                name: "Regions");

            migrationBuilder.DropIndex(
                name: "IX_Country_CapitalId",
                table: "Country");

            migrationBuilder.DropIndex(
                name: "IX_Country_RegionId",
                table: "Country");

            migrationBuilder.DropColumn(
                name: "CapitalId",
                table: "Country");

            migrationBuilder.DropColumn(
                name: "RegionId",
                table: "Country");

            migrationBuilder.AddColumn<string>(
                name: "Capital",
                table: "Country",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Region",
                table: "Country",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
