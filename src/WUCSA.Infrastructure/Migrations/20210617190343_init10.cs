using Microsoft.EntityFrameworkCore.Migrations;

namespace WUCSA.Infrastructure.Migrations
{
    public partial class init10 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RankParticipant_SportType_SportTypeId",
                table: "RankParticipant");

            migrationBuilder.AlterColumn<string>(
                name: "SportTypeId",
                table: "RankParticipant",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(32)");

            migrationBuilder.AddForeignKey(
                name: "FK_RankParticipant_SportType_SportTypeId",
                table: "RankParticipant",
                column: "SportTypeId",
                principalTable: "SportType",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RankParticipant_SportType_SportTypeId",
                table: "RankParticipant");

            migrationBuilder.AlterColumn<string>(
                name: "SportTypeId",
                table: "RankParticipant",
                type: "nvarchar(32)",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_RankParticipant_SportType_SportTypeId",
                table: "RankParticipant",
                column: "SportTypeId",
                principalTable: "SportType",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
