using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace WUCSA.Infrastructure.Migrations
{
    public partial class EventFilesAdded : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EventPartsFilePath",
                table: "Event");

            migrationBuilder.DropColumn(
                name: "RulesFilePath",
                table: "Event");

            migrationBuilder.AddColumn<int>(
                name: "OrderNumber",
                table: "Staff",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<DateTime>(
                name: "EventEndDate",
                table: "Event",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "EventPromoVideo",
                table: "Event",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "EventRelatedFile",
                columns: table => new
                {
                    Id = table.Column<string>(maxLength: 32, nullable: false),
                    OrderNumber = table.Column<int>(nullable: false),
                    Title = table.Column<string>(maxLength: 100, nullable: false),
                    TitleRu = table.Column<string>(maxLength: 100, nullable: false),
                    TitleUz = table.Column<string>(maxLength: 100, nullable: false),
                    Path = table.Column<string>(nullable: true),
                    EventId = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EventRelatedFile", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EventRelatedFile_Event_EventId",
                        column: x => x.EventId,
                        principalTable: "Event",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_EventRelatedFile_EventId",
                table: "EventRelatedFile",
                column: "EventId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "EventRelatedFile");

            migrationBuilder.DropColumn(
                name: "OrderNumber",
                table: "Staff");

            migrationBuilder.DropColumn(
                name: "EventEndDate",
                table: "Event");

            migrationBuilder.DropColumn(
                name: "EventPromoVideo",
                table: "Event");

            migrationBuilder.AddColumn<string>(
                name: "EventPartsFilePath",
                table: "Event",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "RulesFilePath",
                table: "Event",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
