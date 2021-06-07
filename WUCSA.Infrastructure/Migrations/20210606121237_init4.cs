using Microsoft.EntityFrameworkCore.Migrations;

namespace WUCSA.Infrastructure.Migrations
{
    public partial class init4 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "StaticPage",
                table: "Blog",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "StaticPage",
                table: "Blog");
        }
    }
}
