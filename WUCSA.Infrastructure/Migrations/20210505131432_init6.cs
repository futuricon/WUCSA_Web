using Microsoft.EntityFrameworkCore.Migrations;

namespace WUCSA.Infrastructure.Migrations
{
    public partial class init6 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CoverPhotoPath",
                table: "SportType");

            migrationBuilder.DropColumn(
                name: "Rules",
                table: "SportType");

            migrationBuilder.AddColumn<string>(
                name: "Slug",
                table: "Staff",
                maxLength: 80,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "RulesFilePath",
                table: "SportType",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Slug",
                table: "SportType",
                maxLength: 80,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Slug",
                table: "Event",
                maxLength: 80,
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Slug",
                table: "Staff");

            migrationBuilder.DropColumn(
                name: "RulesFilePath",
                table: "SportType");

            migrationBuilder.DropColumn(
                name: "Slug",
                table: "SportType");

            migrationBuilder.DropColumn(
                name: "Slug",
                table: "Event");

            migrationBuilder.AddColumn<string>(
                name: "CoverPhotoPath",
                table: "SportType",
                type: "nvarchar(64)",
                maxLength: 64,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Rules",
                table: "SportType",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
