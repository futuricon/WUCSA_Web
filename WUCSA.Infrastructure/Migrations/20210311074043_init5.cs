using Microsoft.EntityFrameworkCore.Migrations;

namespace WUCSA.Infrastructure.Migrations
{
    public partial class init5 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Summary",
                table: "Blog");

            migrationBuilder.DropColumn(
                name: "SummaryRu",
                table: "Blog");

            migrationBuilder.DropColumn(
                name: "SummaryUz",
                table: "Blog");

            migrationBuilder.AddColumn<string>(
                name: "Content",
                table: "Blog",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "ContentRu",
                table: "Blog",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "ContentUz",
                table: "Blog",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Content",
                table: "Blog");

            migrationBuilder.DropColumn(
                name: "ContentRu",
                table: "Blog");

            migrationBuilder.DropColumn(
                name: "ContentUz",
                table: "Blog");

            migrationBuilder.AddColumn<string>(
                name: "Summary",
                table: "Blog",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "SummaryRu",
                table: "Blog",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "SummaryUz",
                table: "Blog",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
