using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace WUCSA.Infrastructure.Migrations
{
    public partial class init3 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RankName",
                table: "Rank");

            migrationBuilder.AddColumn<int>(
                name: "RankLocation",
                table: "Rank",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "RankPartsFileUrl",
                table: "Rank",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "RankYear",
                table: "Rank",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "SportTypeId",
                table: "Rank",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "EventPartsFileUrl",
                table: "Event",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "RulesFileUrl",
                table: "Event",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SportTypeId",
                table: "Event",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "StaffId",
                table: "Certificate",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "SportType",
                columns: table => new
                {
                    Id = table.Column<string>(maxLength: 32, nullable: false),
                    Name = table.Column<string>(maxLength: 100, nullable: false),
                    NameRu = table.Column<string>(maxLength: 100, nullable: false),
                    NameUz = table.Column<string>(maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SportType", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Staff",
                columns: table => new
                {
                    Id = table.Column<string>(maxLength: 32, nullable: false),
                    ProfilePhotoPath = table.Column<string>(maxLength: 100, nullable: true),
                    FirstName = table.Column<string>(maxLength: 40, nullable: false),
                    LastName = table.Column<string>(maxLength: 40, nullable: false),
                    Position = table.Column<string>(maxLength: 80, nullable: false),
                    PositionRu = table.Column<string>(maxLength: 80, nullable: false),
                    PositionUz = table.Column<string>(maxLength: 80, nullable: false),
                    BirthDate = table.Column<DateTime>(nullable: false),
                    Description = table.Column<string>(maxLength: 400, nullable: true),
                    DescriptionRu = table.Column<string>(maxLength: 400, nullable: true),
                    DescriptionUz = table.Column<string>(maxLength: 400, nullable: true),
                    IsMember = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Staff", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Rank_SportTypeId",
                table: "Rank",
                column: "SportTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_Event_SportTypeId",
                table: "Event",
                column: "SportTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_Certificate_StaffId",
                table: "Certificate",
                column: "StaffId");

            migrationBuilder.AddForeignKey(
                name: "FK_Certificate_Staff_StaffId",
                table: "Certificate",
                column: "StaffId",
                principalTable: "Staff",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Event_SportType_SportTypeId",
                table: "Event",
                column: "SportTypeId",
                principalTable: "SportType",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Rank_SportType_SportTypeId",
                table: "Rank",
                column: "SportTypeId",
                principalTable: "SportType",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Certificate_Staff_StaffId",
                table: "Certificate");

            migrationBuilder.DropForeignKey(
                name: "FK_Event_SportType_SportTypeId",
                table: "Event");

            migrationBuilder.DropForeignKey(
                name: "FK_Rank_SportType_SportTypeId",
                table: "Rank");

            migrationBuilder.DropTable(
                name: "SportType");

            migrationBuilder.DropTable(
                name: "Staff");

            migrationBuilder.DropIndex(
                name: "IX_Rank_SportTypeId",
                table: "Rank");

            migrationBuilder.DropIndex(
                name: "IX_Event_SportTypeId",
                table: "Event");

            migrationBuilder.DropIndex(
                name: "IX_Certificate_StaffId",
                table: "Certificate");

            migrationBuilder.DropColumn(
                name: "RankLocation",
                table: "Rank");

            migrationBuilder.DropColumn(
                name: "RankPartsFileUrl",
                table: "Rank");

            migrationBuilder.DropColumn(
                name: "RankYear",
                table: "Rank");

            migrationBuilder.DropColumn(
                name: "SportTypeId",
                table: "Rank");

            migrationBuilder.DropColumn(
                name: "EventPartsFileUrl",
                table: "Event");

            migrationBuilder.DropColumn(
                name: "RulesFileUrl",
                table: "Event");

            migrationBuilder.DropColumn(
                name: "SportTypeId",
                table: "Event");

            migrationBuilder.DropColumn(
                name: "StaffId",
                table: "Certificate");

            migrationBuilder.AddColumn<string>(
                name: "RankName",
                table: "Rank",
                type: "nvarchar(40)",
                maxLength: 40,
                nullable: false,
                defaultValue: "");
        }
    }
}
