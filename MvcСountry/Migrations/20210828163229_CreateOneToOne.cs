using Microsoft.EntityFrameworkCore.Migrations;

namespace MvcСountry.Migrations
{
    public partial class CreateOneToOne : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Country_City_CapitalId",
                table: "Country");

            migrationBuilder.DropForeignKey(
                name: "FK_Country_Regions_RegionId",
                table: "Country");

            migrationBuilder.DropTable(
                name: "Regions");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Country",
                table: "Country");

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

            migrationBuilder.RenameColumn(
                name: "alpha2Code",
                table: "Country",
                newName: "Alpha2Code");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "Country",
                newName: "CityId");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "City",
                newName: "CityId");

            migrationBuilder.AlterColumn<int>(
                name: "CityId",
                table: "Country",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int")
                .OldAnnotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddColumn<int>(
                name: "CountryId",
                table: "Country",
                type: "int",
                nullable: false,
                defaultValue: 0)
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Country",
                table: "Country",
                column: "CountryId");

            migrationBuilder.CreateIndex(
                name: "IX_Country_CityId",
                table: "Country",
                column: "CityId");

            migrationBuilder.AddForeignKey(
                name: "FK_Country_City_CityId",
                table: "Country",
                column: "CityId",
                principalTable: "City",
                principalColumn: "CityId",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Country_City_CityId",
                table: "Country");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Country",
                table: "Country");

            migrationBuilder.DropIndex(
                name: "IX_Country_CityId",
                table: "Country");

            migrationBuilder.DropColumn(
                name: "CountryId",
                table: "Country");

            migrationBuilder.RenameColumn(
                name: "Alpha2Code",
                table: "Country",
                newName: "alpha2Code");

            migrationBuilder.RenameColumn(
                name: "CityId",
                table: "Country",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "CityId",
                table: "City",
                newName: "Id");

            migrationBuilder.AlterColumn<int>(
                name: "Id",
                table: "Country",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int")
                .Annotation("SqlServer:Identity", "1, 1");

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

            migrationBuilder.AddPrimaryKey(
                name: "PK_Country",
                table: "Country",
                column: "Id");

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
    }
}
