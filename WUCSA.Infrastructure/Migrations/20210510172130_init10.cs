using Microsoft.EntityFrameworkCore.Migrations;

namespace WUCSA.Infrastructure.Migrations
{
    public partial class init10 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Event_SportType_SportTypeId",
                table: "Event");

            migrationBuilder.DropIndex(
                name: "IX_Event_SportTypeId",
                table: "Event");

            migrationBuilder.DropColumn(
                name: "CompetitionType",
                table: "Event");

            migrationBuilder.DropColumn(
                name: "SportTypeId",
                table: "Event");

            migrationBuilder.AlterColumn<string>(
                name: "Location",
                table: "Event",
                maxLength: 100,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(80)",
                oldMaxLength: 80);

            migrationBuilder.AlterColumn<string>(
                name: "DescriptionUz",
                table: "Event",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(80)",
                oldMaxLength: 80);

            migrationBuilder.AlterColumn<string>(
                name: "DescriptionRu",
                table: "Event",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(160)",
                oldMaxLength: 160);

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "Event",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(160)",
                oldMaxLength: 160);

            migrationBuilder.AddColumn<int>(
                name: "EventLocation",
                table: "Event",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "Title",
                table: "Event",
                maxLength: 100,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "TitleRu",
                table: "Event",
                maxLength: 100,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "TitleUz",
                table: "Event",
                maxLength: 100,
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateTable(
                name: "EventSportType",
                columns: table => new
                {
                    EventId = table.Column<string>(maxLength: 32, nullable: false),
                    SportTypeId = table.Column<string>(maxLength: 32, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EventSportType", x => new { x.EventId, x.SportTypeId });
                    table.ForeignKey(
                        name: "FK_EventSportType_Event_EventId",
                        column: x => x.EventId,
                        principalTable: "Event",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_EventSportType_SportType_SportTypeId",
                        column: x => x.SportTypeId,
                        principalTable: "SportType",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_EventSportType_SportTypeId",
                table: "EventSportType",
                column: "SportTypeId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "EventSportType");

            migrationBuilder.DropColumn(
                name: "EventLocation",
                table: "Event");

            migrationBuilder.DropColumn(
                name: "Title",
                table: "Event");

            migrationBuilder.DropColumn(
                name: "TitleRu",
                table: "Event");

            migrationBuilder.DropColumn(
                name: "TitleUz",
                table: "Event");

            migrationBuilder.AlterColumn<string>(
                name: "Location",
                table: "Event",
                type: "nvarchar(80)",
                maxLength: 80,
                nullable: false,
                oldClrType: typeof(string),
                oldMaxLength: 100);

            migrationBuilder.AlterColumn<string>(
                name: "DescriptionUz",
                table: "Event",
                type: "nvarchar(80)",
                maxLength: 80,
                nullable: false,
                oldClrType: typeof(string));

            migrationBuilder.AlterColumn<string>(
                name: "DescriptionRu",
                table: "Event",
                type: "nvarchar(160)",
                maxLength: 160,
                nullable: false,
                oldClrType: typeof(string));

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "Event",
                type: "nvarchar(160)",
                maxLength: 160,
                nullable: false,
                oldClrType: typeof(string));

            migrationBuilder.AddColumn<string>(
                name: "CompetitionType",
                table: "Event",
                type: "nvarchar(80)",
                maxLength: 80,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "SportTypeId",
                table: "Event",
                type: "nvarchar(32)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Event_SportTypeId",
                table: "Event",
                column: "SportTypeId");

            migrationBuilder.AddForeignKey(
                name: "FK_Event_SportType_SportTypeId",
                table: "Event",
                column: "SportTypeId",
                principalTable: "SportType",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
