using Microsoft.EntityFrameworkCore.Migrations;

namespace WUCSA.Infrastructure.Migrations
{
    public partial class init7 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Rank_SportType_SportTypeId",
                table: "Rank");

            migrationBuilder.AlterColumn<string>(
                name: "SportTypeId",
                table: "Rank",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(32)");

            migrationBuilder.AddForeignKey(
                name: "FK_Rank_SportType_SportTypeId",
                table: "Rank",
                column: "SportTypeId",
                principalTable: "SportType",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Rank_SportType_SportTypeId",
                table: "Rank");

            migrationBuilder.AlterColumn<string>(
                name: "SportTypeId",
                table: "Rank",
                type: "nvarchar(32)",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Rank_SportType_SportTypeId",
                table: "Rank",
                column: "SportTypeId",
                principalTable: "SportType",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
