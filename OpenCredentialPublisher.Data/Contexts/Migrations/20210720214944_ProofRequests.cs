using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace OpenCredentialPublisher.Data.Contexts.Migrations
{
    public partial class ProofRequests : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ProofRequestId",
                table: "Messages",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "ProofRequests",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ThreadId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PublicId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ForRelationship = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ProofAttributes = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ProofPredicates = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NotificationAddress = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CredentialSchemaId = table.Column<int>(type: "int", nullable: false),
                    InvitationId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    InvitationLink = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ShortInvitationLink = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    StepId = table.Column<int>(type: "int", nullable: false),
                    CreatedOn = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    ModifiedOn = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProofRequests", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProofRequests_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_ProofRequests_CredentialSchema_CredentialSchemaId",
                        column: x => x.CredentialSchemaId,
                        principalTable: "CredentialSchema",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "ProofRequestSteps",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProofRequestSteps", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ProofResponses",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProofRequestId = table.Column<int>(type: "int", nullable: false),
                    ProofResultId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    VerificationResult = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SelfAttestedAttributes = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RevealedAttributes = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Predicates = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UnrevealedAttributes = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Identifiers = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedOn = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProofResponses", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProofResponses_ProofRequests_ProofRequestId",
                        column: x => x.ProofRequestId,
                        principalTable: "ProofRequests",
                        principalColumn: "Id");
                });

            migrationBuilder.InsertData(
                table: "ProofRequestSteps",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 1, "Created" },
                    { 2, "WaitingForAgentContext" },
                    { 3, "InvitationLinkRequested" },
                    { 4, "InvitationLinkReceived" },
                    { 5, "ProofReceived" },
                    { 9, "Deleted" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Messages_ProofRequestId",
                table: "Messages",
                column: "ProofRequestId");

            migrationBuilder.CreateIndex(
                name: "IX_ProofRequests_CredentialSchemaId",
                table: "ProofRequests",
                column: "CredentialSchemaId");

            migrationBuilder.CreateIndex(
                name: "IX_ProofRequests_UserId",
                table: "ProofRequests",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_ProofResponses_ProofRequestId",
                table: "ProofResponses",
                column: "ProofRequestId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Messages_ProofRequests_ProofRequestId",
                table: "Messages",
                column: "ProofRequestId",
                principalTable: "ProofRequests",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);


            var sql = @"
                CREATE OR ALTER VIEW cred.CredentialPackageArtifacts
                as
                SELECT AR.ArtifactId, CP.Id AS CredentialPackageId, CP.TypeId, CP.UserId, CP.Revoked, CP.IsDeleted, AR.IsPdf, Ar.IsUrl, AR.[Description], Ev.[Name], AR.[Url], AR.AssertionId, AR.ClrId, AR.ClrIssuedOn, AR.ClrName, AR.EvidenceName FROM cred.CredentialPackages CP 
                JOIN cred.VerifiableCredentials VC ON CP.Id = VC.ParentCredentialPackageId
                JOIN cred.ClrSets CSE ON VC.Id = CSE.ParentVerifiableCredentialId
                JOIN cred.Clrs Clr ON CSE.Id = Clr.ParentClrSetId
                JOIN cred.ClrAssertions ASS ON Clr.ClrId = ASS.ClrId
                JOIN cred.AssertionEvidence AE ON ASS.AssertionId = AE.AssertionId
                JOIN cred.Evidence Ev ON Ev.EvidenceId = AE.EvidenceId
                JOIN cred.EvidenceArtifacts EA ON AE.EvidenceId = EA.EvidenceId
                JOIN cred.Artifacts AR ON EA.ArtifactId = AR.ArtifactId
                UNION
                SELECT AR.ArtifactId, CP.Id AS CredentialPackageId, CP.TypeId, CP.UserId, CP.Revoked, CP.IsDeleted, AR.IsPdf, Ar.IsUrl, AR.[Description], Ev.[Name], AR.[Url], AR.AssertionId, AR.ClrId, AR.ClrIssuedOn, AR.ClrName, AR.EvidenceName FROM cred.CredentialPackages CP 
                JOIN cred.VerifiableCredentials VC ON CP.Id = VC.ParentCredentialPackageId
                JOIN cred.Clrs Clr ON VC.Id = Clr.ParentVerifiableCredentialId
                JOIN cred.ClrAssertions ASS ON Clr.ClrId = ASS.ClrId
                JOIN cred.AssertionEvidence AE ON ASS.AssertionId = AE.AssertionId
                JOIN cred.Evidence Ev ON Ev.EvidenceId = AE.EvidenceId
                JOIN cred.EvidenceArtifacts EA ON AE.EvidenceId = EA.EvidenceId
                JOIN cred.Artifacts AR ON EA.ArtifactId = AR.ArtifactId
                UNION
                SELECT AR.ArtifactId, CP.Id AS CredentialPackageId, CP.TypeId, CP.UserId, CP.Revoked, CP.IsDeleted, AR.IsPdf, Ar.IsUrl, AR.[Description], Ev.[Name], AR.[Url], AR.AssertionId, AR.ClrId, AR.ClrIssuedOn, AR.ClrName, AR.EvidenceName FROM cred.CredentialPackages CP 
                JOIN cred.ClrSets CSE ON CP.Id = CSE.ParentCredentialPackageId
                JOIN cred.Clrs Clr ON CSE.Id = Clr.ParentClrSetId
                JOIN cred.ClrAssertions ASS ON Clr.ClrId = ASS.ClrId
                JOIN cred.AssertionEvidence AE ON ASS.AssertionId = AE.AssertionId
                JOIN cred.Evidence Ev ON Ev.EvidenceId = AE.EvidenceId
                JOIN cred.EvidenceArtifacts EA ON AE.EvidenceId = EA.EvidenceId
                JOIN cred.Artifacts AR ON EA.ArtifactId = AR.ArtifactId
                UNION
                SELECT AR.ArtifactId, CP.Id AS CredentialPackageId, CP.TypeId, CP.UserId, CP.Revoked, CP.IsDeleted, AR.IsPdf, Ar.IsUrl, AR.[Description], Ev.[Name], AR.[Url], AR.AssertionId, AR.ClrId, AR.ClrIssuedOn, AR.ClrName, AR.EvidenceName FROM cred.CredentialPackages CP 
                JOIN cred.Clrs Clr ON CP.Id = Clr.ParentCredentialPackageId
                JOIN cred.ClrAssertions ASS ON Clr.ClrId = ASS.ClrId
                JOIN cred.AssertionEvidence AE ON ASS.AssertionId = AE.AssertionId
                JOIN cred.Evidence Ev ON Ev.EvidenceId = AE.EvidenceId
                JOIN cred.EvidenceArtifacts EA ON AE.EvidenceId = EA.EvidenceId
                JOIN cred.Artifacts AR ON EA.ArtifactId = AR.ArtifactId
            ";

            migrationBuilder.Sql(sql);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Messages_ProofRequests_ProofRequestId",
                table: "Messages");

            migrationBuilder.DropTable(
                name: "ProofRequestSteps");

            migrationBuilder.DropTable(
                name: "ProofResponses");

            migrationBuilder.DropTable(
                name: "ProofRequests");

            migrationBuilder.DropIndex(
                name: "IX_Messages_ProofRequestId",
                table: "Messages");

            migrationBuilder.DropColumn(
                name: "ProofRequestId",
                table: "Messages");

            migrationBuilder.Sql("DROP VIEW cred.CredentialPackageArtifacts");
        }
    }
}
