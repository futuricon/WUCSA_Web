using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace WUCSA.Infrastructure.Migrations
{
    public partial class init5 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Name",
                table: "Rank");

            migrationBuilder.DropColumn(
                name: "NameRu",
                table: "Rank");

            migrationBuilder.DropColumn(
                name: "NameUz",
                table: "Rank");

            migrationBuilder.DropColumn(
                name: "RankYear",
                table: "Rank");

            migrationBuilder.AddColumn<string>(
                name: "AuthorId",
                table: "RankParticipant",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "AuthorId",
                table: "Rank",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "RankDate",
                table: "Rank",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "Slug",
                table: "Rank",
                maxLength: 80,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Title",
                table: "Rank",
                maxLength: 100,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "TitleRu",
                table: "Rank",
                maxLength: 100,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "TitleUz",
                table: "Rank",
                maxLength: 100,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "AuthorId",
                table: "EventParticipant",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Gender",
                table: "EventParticipant",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "EventParticipant",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "AuthorId",
                table: "Event",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_RankParticipant_AuthorId",
                table: "RankParticipant",
                column: "AuthorId");

            migrationBuilder.CreateIndex(
                name: "IX_Rank_AuthorId",
                table: "Rank",
                column: "AuthorId");

            migrationBuilder.CreateIndex(
                name: "IX_EventParticipant_AuthorId",
                table: "EventParticipant",
                column: "AuthorId");

            migrationBuilder.CreateIndex(
                name: "IX_Event_AuthorId",
                table: "Event",
                column: "AuthorId");

            migrationBuilder.AddForeignKey(
                name: "FK_Event_Users_AuthorId",
                table: "Event",
                column: "AuthorId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_EventParticipant_Users_AuthorId",
                table: "EventParticipant",
                column: "AuthorId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Rank_Users_AuthorId",
                table: "Rank",
                column: "AuthorId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_RankParticipant_Users_AuthorId",
                table: "RankParticipant",
                column: "AuthorId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Event_Users_AuthorId",
                table: "Event");

            migrationBuilder.DropForeignKey(
                name: "FK_EventParticipant_Users_AuthorId",
                table: "EventParticipant");

            migrationBuilder.DropForeignKey(
                name: "FK_Rank_Users_AuthorId",
                table: "Rank");

            migrationBuilder.DropForeignKey(
                name: "FK_RankParticipant_Users_AuthorId",
                table: "RankParticipant");

            migrationBuilder.DropIndex(
                name: "IX_RankParticipant_AuthorId",
                table: "RankParticipant");

            migrationBuilder.DropIndex(
                name: "IX_Rank_AuthorId",
                table: "Rank");

            migrationBuilder.DropIndex(
                name: "IX_EventParticipant_AuthorId",
                table: "EventParticipant");

            migrationBuilder.DropIndex(
                name: "IX_Event_AuthorId",
                table: "Event");

            migrationBuilder.DropColumn(
                name: "AuthorId",
                table: "RankParticipant");

            migrationBuilder.DropColumn(
                name: "AuthorId",
                table: "Rank");

            migrationBuilder.DropColumn(
                name: "RankDate",
                table: "Rank");

            migrationBuilder.DropColumn(
                name: "Slug",
                table: "Rank");

            migrationBuilder.DropColumn(
                name: "Title",
                table: "Rank");

            migrationBuilder.DropColumn(
                name: "TitleRu",
                table: "Rank");

            migrationBuilder.DropColumn(
                name: "TitleUz",
                table: "Rank");

            migrationBuilder.DropColumn(
                name: "AuthorId",
                table: "EventParticipant");

            migrationBuilder.DropColumn(
                name: "Gender",
                table: "EventParticipant");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "EventParticipant");

            migrationBuilder.DropColumn(
                name: "AuthorId",
                table: "Event");

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "Rank",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "NameRu",
                table: "Rank",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "NameUz",
                table: "Rank",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<DateTime>(
                name: "RankYear",
                table: "Rank",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }
    }
}
