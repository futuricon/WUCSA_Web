using Microsoft.EntityFrameworkCore.Migrations;

namespace WUCSA.Infrastructure.Migrations
{
    public partial class init4 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Certificate_Participant_ParticipantId",
                table: "Certificate");

            migrationBuilder.DropForeignKey(
                name: "FK_EventParticipant_Event_EventId",
                table: "EventParticipant");

            migrationBuilder.DropForeignKey(
                name: "FK_EventParticipant_Participant_ParticipantId",
                table: "EventParticipant");

            migrationBuilder.DropForeignKey(
                name: "FK_RankParticipant_Participant_ParticipantId",
                table: "RankParticipant");

            migrationBuilder.DropForeignKey(
                name: "FK_RankParticipant_Rank_RankId",
                table: "RankParticipant");

            migrationBuilder.DropPrimaryKey(
                name: "PK_RankParticipant",
                table: "RankParticipant");

            migrationBuilder.DropIndex(
                name: "IX_RankParticipant_ParticipantId",
                table: "RankParticipant");

            migrationBuilder.DropPrimaryKey(
                name: "PK_EventParticipant",
                table: "EventParticipant");

            migrationBuilder.DropIndex(
                name: "IX_EventParticipant_ParticipantId",
                table: "EventParticipant");

            migrationBuilder.DropIndex(
                name: "IX_Certificate_ParticipantId",
                table: "Certificate");

            migrationBuilder.DropColumn(
                name: "ParticipantId",
                table: "RankParticipant");

            migrationBuilder.DropColumn(
                name: "PositionNumber",
                table: "RankParticipant");

            migrationBuilder.DropColumn(
                name: "RankPartsFileUrl",
                table: "Rank");

            migrationBuilder.DropColumn(
                name: "Gender",
                table: "Participant");

            migrationBuilder.DropColumn(
                name: "ProfilePhotoPath",
                table: "Participant");

            migrationBuilder.DropColumn(
                name: "ParticipantId",
                table: "EventParticipant");

            migrationBuilder.DropColumn(
                name: "EventPartsFileUrl",
                table: "Event");

            migrationBuilder.DropColumn(
                name: "RulesFileUrl",
                table: "Event");

            migrationBuilder.DropColumn(
                name: "ParticipantId",
                table: "Certificate");

            migrationBuilder.AddColumn<string>(
                name: "Email",
                table: "Staff",
                maxLength: 30,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "FacebookUrl",
                table: "Staff",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "InstagramUrl",
                table: "Staff",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "Staff",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "Location",
                table: "Staff",
                maxLength: 100,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "PhoneNumber",
                table: "Staff",
                maxLength: 20,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "TelegramUrl",
                table: "Staff",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "TwitterUrl",
                table: "Staff",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CoverPhotoPath",
                table: "SportType",
                maxLength: 64,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "SportType",
                maxLength: 300,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DescriptionRu",
                table: "SportType",
                maxLength: 300,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DescriptionUz",
                table: "SportType",
                maxLength: 300,
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "SportType",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "Rules",
                table: "SportType",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "RankId",
                table: "RankParticipant",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(32)",
                oldMaxLength: 32);

            migrationBuilder.AddColumn<string>(
                name: "Id",
                table: "RankParticipant",
                maxLength: 32,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "Gender",
                table: "RankParticipant",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "RankParticipant",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "Weight",
                table: "RankParticipant",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "Rank",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "Rank",
                maxLength: 100,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "NameRu",
                table: "Rank",
                maxLength: 100,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "NameUz",
                table: "Rank",
                maxLength: 100,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "RankPartsFilePath",
                table: "Rank",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "EventParticipantId",
                table: "Participant",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Location",
                table: "Participant",
                maxLength: 100,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "PositionNumber",
                table: "Participant",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "RankParticipantId",
                table: "Participant",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "EventId",
                table: "EventParticipant",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(32)",
                oldMaxLength: 32);

            migrationBuilder.AddColumn<string>(
                name: "Id",
                table: "EventParticipant",
                maxLength: 32,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "Weight",
                table: "EventParticipant",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "EventPartsFilePath",
                table: "Event",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "Event",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "RulesFilePath",
                table: "Event",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "Blog",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddPrimaryKey(
                name: "PK_RankParticipant",
                table: "RankParticipant",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_EventParticipant",
                table: "EventParticipant",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_RankParticipant_RankId",
                table: "RankParticipant",
                column: "RankId");

            migrationBuilder.CreateIndex(
                name: "IX_Participant_EventParticipantId",
                table: "Participant",
                column: "EventParticipantId");

            migrationBuilder.CreateIndex(
                name: "IX_Participant_RankParticipantId",
                table: "Participant",
                column: "RankParticipantId");

            migrationBuilder.CreateIndex(
                name: "IX_EventParticipant_EventId",
                table: "EventParticipant",
                column: "EventId");

            migrationBuilder.AddForeignKey(
                name: "FK_EventParticipant_Event_EventId",
                table: "EventParticipant",
                column: "EventId",
                principalTable: "Event",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Participant_EventParticipant_EventParticipantId",
                table: "Participant",
                column: "EventParticipantId",
                principalTable: "EventParticipant",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Participant_RankParticipant_RankParticipantId",
                table: "Participant",
                column: "RankParticipantId",
                principalTable: "RankParticipant",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_RankParticipant_Rank_RankId",
                table: "RankParticipant",
                column: "RankId",
                principalTable: "Rank",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_EventParticipant_Event_EventId",
                table: "EventParticipant");

            migrationBuilder.DropForeignKey(
                name: "FK_Participant_EventParticipant_EventParticipantId",
                table: "Participant");

            migrationBuilder.DropForeignKey(
                name: "FK_Participant_RankParticipant_RankParticipantId",
                table: "Participant");

            migrationBuilder.DropForeignKey(
                name: "FK_RankParticipant_Rank_RankId",
                table: "RankParticipant");

            migrationBuilder.DropPrimaryKey(
                name: "PK_RankParticipant",
                table: "RankParticipant");

            migrationBuilder.DropIndex(
                name: "IX_RankParticipant_RankId",
                table: "RankParticipant");

            migrationBuilder.DropIndex(
                name: "IX_Participant_EventParticipantId",
                table: "Participant");

            migrationBuilder.DropIndex(
                name: "IX_Participant_RankParticipantId",
                table: "Participant");

            migrationBuilder.DropPrimaryKey(
                name: "PK_EventParticipant",
                table: "EventParticipant");

            migrationBuilder.DropIndex(
                name: "IX_EventParticipant_EventId",
                table: "EventParticipant");

            migrationBuilder.DropColumn(
                name: "Email",
                table: "Staff");

            migrationBuilder.DropColumn(
                name: "FacebookUrl",
                table: "Staff");

            migrationBuilder.DropColumn(
                name: "InstagramUrl",
                table: "Staff");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "Staff");

            migrationBuilder.DropColumn(
                name: "Location",
                table: "Staff");

            migrationBuilder.DropColumn(
                name: "PhoneNumber",
                table: "Staff");

            migrationBuilder.DropColumn(
                name: "TelegramUrl",
                table: "Staff");

            migrationBuilder.DropColumn(
                name: "TwitterUrl",
                table: "Staff");

            migrationBuilder.DropColumn(
                name: "CoverPhotoPath",
                table: "SportType");

            migrationBuilder.DropColumn(
                name: "Description",
                table: "SportType");

            migrationBuilder.DropColumn(
                name: "DescriptionRu",
                table: "SportType");

            migrationBuilder.DropColumn(
                name: "DescriptionUz",
                table: "SportType");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "SportType");

            migrationBuilder.DropColumn(
                name: "Rules",
                table: "SportType");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "RankParticipant");

            migrationBuilder.DropColumn(
                name: "Gender",
                table: "RankParticipant");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "RankParticipant");

            migrationBuilder.DropColumn(
                name: "Weight",
                table: "RankParticipant");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "Rank");

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
                name: "RankPartsFilePath",
                table: "Rank");

            migrationBuilder.DropColumn(
                name: "EventParticipantId",
                table: "Participant");

            migrationBuilder.DropColumn(
                name: "Location",
                table: "Participant");

            migrationBuilder.DropColumn(
                name: "PositionNumber",
                table: "Participant");

            migrationBuilder.DropColumn(
                name: "RankParticipantId",
                table: "Participant");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "EventParticipant");

            migrationBuilder.DropColumn(
                name: "Weight",
                table: "EventParticipant");

            migrationBuilder.DropColumn(
                name: "EventPartsFilePath",
                table: "Event");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "Event");

            migrationBuilder.DropColumn(
                name: "RulesFilePath",
                table: "Event");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "Blog");

            migrationBuilder.AlterColumn<string>(
                name: "RankId",
                table: "RankParticipant",
                type: "nvarchar(32)",
                maxLength: 32,
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ParticipantId",
                table: "RankParticipant",
                type: "nvarchar(32)",
                maxLength: 32,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "PositionNumber",
                table: "RankParticipant",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "RankPartsFileUrl",
                table: "Rank",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Gender",
                table: "Participant",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "ProfilePhotoPath",
                table: "Participant",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "EventId",
                table: "EventParticipant",
                type: "nvarchar(32)",
                maxLength: 32,
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ParticipantId",
                table: "EventParticipant",
                type: "nvarchar(32)",
                maxLength: 32,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "EventPartsFileUrl",
                table: "Event",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "RulesFileUrl",
                table: "Event",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ParticipantId",
                table: "Certificate",
                type: "nvarchar(32)",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_RankParticipant",
                table: "RankParticipant",
                columns: new[] { "RankId", "ParticipantId" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_EventParticipant",
                table: "EventParticipant",
                columns: new[] { "EventId", "ParticipantId" });

            migrationBuilder.CreateIndex(
                name: "IX_RankParticipant_ParticipantId",
                table: "RankParticipant",
                column: "ParticipantId");

            migrationBuilder.CreateIndex(
                name: "IX_EventParticipant_ParticipantId",
                table: "EventParticipant",
                column: "ParticipantId");

            migrationBuilder.CreateIndex(
                name: "IX_Certificate_ParticipantId",
                table: "Certificate",
                column: "ParticipantId");

            migrationBuilder.AddForeignKey(
                name: "FK_Certificate_Participant_ParticipantId",
                table: "Certificate",
                column: "ParticipantId",
                principalTable: "Participant",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_EventParticipant_Event_EventId",
                table: "EventParticipant",
                column: "EventId",
                principalTable: "Event",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_EventParticipant_Participant_ParticipantId",
                table: "EventParticipant",
                column: "ParticipantId",
                principalTable: "Participant",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_RankParticipant_Participant_ParticipantId",
                table: "RankParticipant",
                column: "ParticipantId",
                principalTable: "Participant",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_RankParticipant_Rank_RankId",
                table: "RankParticipant",
                column: "RankId",
                principalTable: "Rank",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
