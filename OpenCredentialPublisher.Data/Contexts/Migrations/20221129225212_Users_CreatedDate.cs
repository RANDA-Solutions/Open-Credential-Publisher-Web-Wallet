using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace OpenCredentialPublisher.Data.Contexts.Migrations
{
    public partial class Users_CreatedDate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedDate",
                table: "AspNetUsers",
                type: "datetime2",
                nullable: false,
                defaultValueSql: "getdate()");

            migrationBuilder.AddColumn<string>(
                name: "UserId",
                schema: "cred",
                table: "Artifacts",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.Sql(@"
                UPDATE A
                SET A.UserId = CP.UserId
                FROM cred.Artifacts A
                JOIN cred.EvidenceArtifacts EA ON EA.ArtifactId = A.ArtifactId
                JOIN cred.Evidence EV ON EV.EvidenceId = EA.EvidenceId
                JOIN cred.AssertionEvidence AE ON AE.EvidenceId = EV.EvidenceId
                JOIN cred.Assertions ASS ON ASS.AssertionId = AE.AssertionId
                JOIN cred.ClrAssertions CASS ON CASS.AssertionId = ASS.AssertionId
                JOIN cred.Clrs C ON C.ClrId = CASS.ClrId
                JOIN cred.CredentialPackages CP ON CP.Id = C.CredentialPackageId
            ");

            migrationBuilder.CreateIndex(
                name: "IX_Artifacts_UserId",
                schema: "cred",
                table: "Artifacts",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Artifacts_AspNetUsers_UserId",
                schema: "cred",
                table: "Artifacts",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Artifacts_AspNetUsers_UserId",
                schema: "cred",
                table: "Artifacts");

            migrationBuilder.DropIndex(
                name: "IX_Artifacts_UserId",
                schema: "cred",
                table: "Artifacts");

            migrationBuilder.DropColumn(
                name: "CreatedDate",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "UserId",
                schema: "cred",
                table: "Artifacts");
        }
    }
}
