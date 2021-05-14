using Microsoft.EntityFrameworkCore.Migrations;

namespace WUCSA.Infrastructure.Migrations
{
    public partial class init8 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Event_SportType_SportTypeId",
                table: "Event");

            migrationBuilder.DropColumn(
                name: "Height",
                table: "Participant");

            migrationBuilder.AlterColumn<string>(
                name: "SportTypeId",
                table: "Event",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(32)");

            migrationBuilder.AddColumn<string>(
                name: "RankId",
                table: "Event",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Event_RankId",
                table: "Event",
                column: "RankId");

            migrationBuilder.AddForeignKey(
                name: "FK_Event_Rank_RankId",
                table: "Event",
                column: "RankId",
                principalTable: "Rank",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Event_SportType_SportTypeId",
                table: "Event",
                column: "SportTypeId",
                principalTable: "SportType",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Event_Rank_RankId",
                table: "Event");

            migrationBuilder.DropForeignKey(
                name: "FK_Event_SportType_SportTypeId",
                table: "Event");

            migrationBuilder.DropIndex(
                name: "IX_Event_RankId",
                table: "Event");

            migrationBuilder.DropColumn(
                name: "RankId",
                table: "Event");

            migrationBuilder.AddColumn<int>(
                name: "Height",
                table: "Participant",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<string>(
                name: "SportTypeId",
                table: "Event",
                type: "nvarchar(32)",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Event_SportType_SportTypeId",
                table: "Event",
                column: "SportTypeId",
                principalTable: "SportType",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
