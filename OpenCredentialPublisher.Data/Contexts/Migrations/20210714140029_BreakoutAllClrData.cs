using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace OpenCredentialPublisher.Data.Contexts.Migrations
{
    public partial class BreakoutAllClrData : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BadgrAssertions_IdentityDType_RecipientIdentityKey",
                table: "BadgrAssertions");

            migrationBuilder.DropForeignKey(
                name: "FK_ClrAssertions_Clrs_ClrId",
                table: "ClrAssertions");

            migrationBuilder.DropForeignKey(
                name: "FK_EvidenceArtifacts_Evidences_EvidenceId",
                table: "EvidenceArtifacts");

            migrationBuilder.DropForeignKey(
                name: "FK_Links_Clrs_ClrForeignKey",
                table: "Links");

            migrationBuilder.DropTable(
                name: "AssertionEvidences");

            migrationBuilder.DropPrimaryKey(
                name: "PK_DiscoveryDocumentModel",
                table: "DiscoveryDocumentModel");

            migrationBuilder.DropIndex(
                name: "IX_BadgrAssertions_RecipientIdentityKey",
                table: "BadgrAssertions");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Clrs",
                table: "Clrs");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Evidences",
                table: "Evidences");

            migrationBuilder.DropColumn(
                name: "DiscoveryDocumentKey",
                table: "DiscoveryDocumentModel");

            migrationBuilder.DropColumn(
                name: "RecipientIdentityKey",
                table: "BadgrAssertions");

            migrationBuilder.DropColumn(
                name: "Achievement",
                table: "Assertions");

            migrationBuilder.DropColumn(
                name: "Endorsements",
                table: "Assertions");

            migrationBuilder.DropColumn(
                name: "Evidence",
                table: "Assertions");

            migrationBuilder.DropColumn(
                name: "Recipient",
                table: "Assertions");

            migrationBuilder.DropColumn(
                name: "Results",
                table: "Assertions");

            migrationBuilder.DropColumn(
                name: "Source",
                table: "Assertions");

            migrationBuilder.DropColumn(
                name: "Verification",
                table: "Assertions");

            migrationBuilder.EnsureSchema(
                name: "clr");

            migrationBuilder.RenameTable(
                name: "EvidenceArtifacts",
                newName: "EvidenceArtifacts",
                newSchema: "clr");

            migrationBuilder.RenameTable(
                name: "ClrSets",
                newName: "ClrSets",
                newSchema: "clr");

            migrationBuilder.RenameTable(
                name: "Clrs",
                newName: "Clrs",
                newSchema: "clr");

            migrationBuilder.RenameTable(
                name: "ClrAssertions",
                newName: "ClrAssertions",
                newSchema: "clr");

            migrationBuilder.RenameTable(
                name: "Assertions",
                newName: "Assertions",
                newSchema: "clr");

            migrationBuilder.RenameTable(
                name: "Artifacts",
                newName: "Artifacts",
                newSchema: "clr");

            migrationBuilder.RenameTable(
                name: "Evidences",
                newName: "Evidence",
                newSchema: "clr");

            migrationBuilder.RenameColumn(
                name: "Id",
                schema: "clr",
                table: "Clrs",
                newName: "ClrId");

            migrationBuilder.RenameColumn(
                name: "Identifier",
                schema: "clr",
                table: "Clrs",
                newName: "Id");

            migrationBuilder.AlterColumn<string>(
                name: "Id",
                table: "DiscoveryDocumentModel",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Type",
                schema: "clr",
                table: "Clrs",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "AdditionalProperties",
                schema: "clr",
                table: "Clrs",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Context",
                schema: "clr",
                table: "Clrs",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "LearnerId",
                schema: "clr",
                table: "Clrs",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<bool>(
                name: "Partial",
                schema: "clr",
                table: "Clrs",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "PublisherId",
                schema: "clr",
                table: "Clrs",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "RevocationReason",
                schema: "clr",
                table: "Clrs",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "Revoked",
                schema: "clr",
                table: "Clrs",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "VerificationId",
                schema: "clr",
                table: "Clrs",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "AchievementId",
                schema: "clr",
                table: "Assertions",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "RecipientId",
                schema: "clr",
                table: "Assertions",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "SourceId",
                schema: "clr",
                table: "Assertions",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "VerificationId",
                schema: "clr",
                table: "Assertions",
                type: "int",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_DiscoveryDocumentModel",
                table: "DiscoveryDocumentModel",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Clrs",
                schema: "clr",
                table: "Clrs",
                column: "ClrId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Evidence",
                schema: "clr",
                table: "Evidence",
                column: "EvidenceId");

            migrationBuilder.CreateTable(
                name: "Alignments",
                schema: "clr",
                columns: table => new
                {
                    AlignmentId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Id = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Type = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    EducationalFramework = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TargetCode = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TargetDescription = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TargetName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TargetType = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TargetUrl = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AdditionalProperties = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Alignments", x => x.AlignmentId);
                });

            migrationBuilder.CreateTable(
                name: "AssertionEvidence",
                schema: "clr",
                columns: table => new
                {
                    AssertionEvidenceId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AssertionId = table.Column<int>(type: "int", nullable: false),
                    EvidenceId = table.Column<int>(type: "int", nullable: false),
                    Order = table.Column<int>(type: "int", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AssertionEvidence", x => x.AssertionEvidenceId);
                    table.ForeignKey(
                        name: "FK_AssertionEvidence_Assertions_AssertionId",
                        column: x => x.AssertionId,
                        principalSchema: "clr",
                        principalTable: "Assertions",
                        principalColumn: "AssertionId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_AssertionEvidence_Evidence_EvidenceId",
                        column: x => x.EvidenceId,
                        principalSchema: "clr",
                        principalTable: "Evidence",
                        principalColumn: "EvidenceId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Associations",
                schema: "clr",
                columns: table => new
                {
                    AssociationId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    AssociationType = table.Column<int>(type: "int", nullable: false),
                    TargetId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AdditionalProperties = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Associations", x => x.AssociationId);
                });

            migrationBuilder.CreateTable(
                name: "Criteria",
                schema: "clr",
                columns: table => new
                {
                    CriteriaId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Id = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Type = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Narrative = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AdditionalProperties = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Criteria", x => x.CriteriaId);
                });

            migrationBuilder.CreateTable(
                name: "EndorsementClaims",
                schema: "clr",
                columns: table => new
                {
                    EndorsementClaimId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Id = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Type = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    EndorsementComment = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AdditionalProperties = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EndorsementClaims", x => x.EndorsementClaimId);
                });

            migrationBuilder.CreateTable(
                name: "Identities",
                schema: "clr",
                columns: table => new
                {
                    IdentityId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Id = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Type = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Identity = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Hashed = table.Column<bool>(type: "bit", nullable: false),
                    Salt = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AdditionalProperties = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Identities", x => x.IdentityId);
                });

            migrationBuilder.CreateTable(
                name: "Results",
                schema: "clr",
                columns: table => new
                {
                    ResultId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Order = table.Column<int>(type: "int", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Id = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Type = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AchievedLevel = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ResultDescription = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Value = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AdditionalProperties = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AssertionId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Results", x => x.ResultId);
                    table.ForeignKey(
                        name: "FK_Results_Assertions_AssertionId",
                        column: x => x.AssertionId,
                        principalSchema: "clr",
                        principalTable: "Assertions",
                        principalColumn: "AssertionId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Verifications",
                schema: "clr",
                columns: table => new
                {
                    VerificationId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Id = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Type = table.Column<int>(type: "int", nullable: false),
                    AllowedOrigins = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Creator = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    StartsWith = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    VerificationProperty = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AdditionalProperties = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Verifications", x => x.VerificationId);
                });

            migrationBuilder.CreateTable(
                name: "ResultAlignments",
                schema: "clr",
                columns: table => new
                {
                    ResultAlignmentId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ResultId = table.Column<int>(type: "int", nullable: false),
                    AlignmentId = table.Column<int>(type: "int", nullable: false),
                    Order = table.Column<int>(type: "int", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ResultAlignments", x => x.ResultAlignmentId);
                    table.ForeignKey(
                        name: "FK_ResultAlignments_Alignments_AlignmentId",
                        column: x => x.AlignmentId,
                        principalSchema: "clr",
                        principalTable: "Alignments",
                        principalColumn: "AlignmentId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ResultAlignments_Results_ResultId",
                        column: x => x.ResultId,
                        principalSchema: "clr",
                        principalTable: "Results",
                        principalColumn: "ResultId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Profiles",
                schema: "clr",
                columns: table => new
                {
                    ProfileId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Id = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Type = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AdditionalName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    BirthDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FamilyName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    GivenName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Image = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Official = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RevocationList = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SourcedId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    StudentId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Telephone = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Url = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AdditionalProperties = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ParentProfileId = table.Column<int>(type: "int", nullable: true),
                    VerificationId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Profiles", x => x.ProfileId);
                    table.ForeignKey(
                        name: "FK_Profiles_Profiles_ParentProfileId",
                        column: x => x.ParentProfileId,
                        principalSchema: "clr",
                        principalTable: "Profiles",
                        principalColumn: "ProfileId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Profiles_Verifications_VerificationId",
                        column: x => x.VerificationId,
                        principalSchema: "clr",
                        principalTable: "Verifications",
                        principalColumn: "VerificationId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Achievements",
                schema: "clr",
                columns: table => new
                {
                    AchievementId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Id = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Type = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AchievementType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreditsAvailable = table.Column<float>(type: "real", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    HumanCode = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FieldOfStudy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Image = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Level = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Specialization = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Tags = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AdditionalProperties = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CriteriaId = table.Column<int>(type: "int", nullable: true),
                    IssuerProfileId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Achievements", x => x.AchievementId);
                    table.ForeignKey(
                        name: "FK_Achievements_Criteria_CriteriaId",
                        column: x => x.CriteriaId,
                        principalSchema: "clr",
                        principalTable: "Criteria",
                        principalColumn: "CriteriaId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Achievements_Profiles_IssuerProfileId",
                        column: x => x.IssuerProfileId,
                        principalSchema: "clr",
                        principalTable: "Profiles",
                        principalColumn: "ProfileId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Endorsements",
                schema: "clr",
                columns: table => new
                {
                    EndorsementId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsSigned = table.Column<bool>(type: "bit", nullable: false),
                    SignedEndorsement = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Id = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Type = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IssuedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    RevocationReason = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Revoked = table.Column<bool>(type: "bit", nullable: true),
                    AdditionalProperties = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IssuerId = table.Column<int>(type: "int", nullable: false),
                    VerificationId = table.Column<int>(type: "int", nullable: false),
                    EndorsementClaimId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Endorsements", x => x.EndorsementId);
                    table.ForeignKey(
                        name: "FK_Endorsements_EndorsementClaims_EndorsementClaimId",
                        column: x => x.EndorsementClaimId,
                        principalSchema: "clr",
                        principalTable: "EndorsementClaims",
                        principalColumn: "EndorsementClaimId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Endorsements_Profiles_IssuerId",
                        column: x => x.IssuerId,
                        principalSchema: "clr",
                        principalTable: "Profiles",
                        principalColumn: "ProfileId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Endorsements_Verifications_VerificationId",
                        column: x => x.VerificationId,
                        principalSchema: "clr",
                        principalTable: "Verifications",
                        principalColumn: "VerificationId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "AchievementAlignments",
                schema: "clr",
                columns: table => new
                {
                    AchievementAlignmentId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AchievementId = table.Column<int>(type: "int", nullable: false),
                    AlignmentId = table.Column<int>(type: "int", nullable: false),
                    Order = table.Column<int>(type: "int", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AchievementAlignments", x => x.AchievementAlignmentId);
                    table.ForeignKey(
                        name: "FK_AchievementAlignments_Achievements_AchievementId",
                        column: x => x.AchievementId,
                        principalSchema: "clr",
                        principalTable: "Achievements",
                        principalColumn: "AchievementId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_AchievementAlignments_Alignments_AlignmentId",
                        column: x => x.AlignmentId,
                        principalSchema: "clr",
                        principalTable: "Alignments",
                        principalColumn: "AlignmentId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "AchievementAssociations",
                schema: "clr",
                columns: table => new
                {
                    AchievementAssociationId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AchievementId = table.Column<int>(type: "int", nullable: false),
                    AssociationId = table.Column<int>(type: "int", nullable: false),
                    Order = table.Column<int>(type: "int", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AchievementAssociations", x => x.AchievementAssociationId);
                    table.ForeignKey(
                        name: "FK_AchievementAssociations_Achievements_AchievementId",
                        column: x => x.AchievementId,
                        principalSchema: "clr",
                        principalTable: "Achievements",
                        principalColumn: "AchievementId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_AchievementAssociations_Associations_AssociationId",
                        column: x => x.AssociationId,
                        principalSchema: "clr",
                        principalTable: "Associations",
                        principalColumn: "AssociationId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ClrAchievements",
                schema: "clr",
                columns: table => new
                {
                    ClrAchievementId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ClrId = table.Column<int>(type: "int", nullable: false),
                    AchievementId = table.Column<int>(type: "int", nullable: false),
                    Order = table.Column<int>(type: "int", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ClrAchievements", x => x.ClrAchievementId);
                    table.ForeignKey(
                        name: "FK_ClrAchievements_Achievements_AchievementId",
                        column: x => x.AchievementId,
                        principalSchema: "clr",
                        principalTable: "Achievements",
                        principalColumn: "AchievementId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ClrAchievements_Clrs_ClrId",
                        column: x => x.ClrId,
                        principalSchema: "clr",
                        principalTable: "Clrs",
                        principalColumn: "ClrId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ResultDescriptions",
                schema: "clr",
                columns: table => new
                {
                    ResultDescriptionId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Order = table.Column<int>(type: "int", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Id = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Type = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AllowedValues = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    RequiredLevel = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RequiredValue = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ResultType = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ValueMax = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ValueMin = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AdditionalProperties = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AchievementId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ResultDescriptions", x => x.ResultDescriptionId);
                    table.ForeignKey(
                        name: "FK_ResultDescriptions_Achievements_AchievementId",
                        column: x => x.AchievementId,
                        principalSchema: "clr",
                        principalTable: "Achievements",
                        principalColumn: "AchievementId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "AchievementEndorsements",
                schema: "clr",
                columns: table => new
                {
                    AchievementEndorsementId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AchievementId = table.Column<int>(type: "int", nullable: false),
                    EndorsementId = table.Column<int>(type: "int", nullable: false),
                    Order = table.Column<int>(type: "int", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AchievementEndorsements", x => x.AchievementEndorsementId);
                    table.ForeignKey(
                        name: "FK_AchievementEndorsements_Achievements_AchievementId",
                        column: x => x.AchievementId,
                        principalSchema: "clr",
                        principalTable: "Achievements",
                        principalColumn: "AchievementId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_AchievementEndorsements_Endorsements_EndorsementId",
                        column: x => x.EndorsementId,
                        principalSchema: "clr",
                        principalTable: "Endorsements",
                        principalColumn: "EndorsementId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "AssertionEndorsements",
                schema: "clr",
                columns: table => new
                {
                    AssertionEndorsementId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AssertionId = table.Column<int>(type: "int", nullable: false),
                    EndorsementId = table.Column<int>(type: "int", nullable: false),
                    Order = table.Column<int>(type: "int", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AssertionEndorsements", x => x.AssertionEndorsementId);
                    table.ForeignKey(
                        name: "FK_AssertionEndorsements_Assertions_AssertionId",
                        column: x => x.AssertionId,
                        principalSchema: "clr",
                        principalTable: "Assertions",
                        principalColumn: "AssertionId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_AssertionEndorsements_Endorsements_EndorsementId",
                        column: x => x.EndorsementId,
                        principalSchema: "clr",
                        principalTable: "Endorsements",
                        principalColumn: "EndorsementId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ClrEndorsements",
                schema: "clr",
                columns: table => new
                {
                    ClrEndorsementId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ClrId = table.Column<int>(type: "int", nullable: false),
                    EndorsementId = table.Column<int>(type: "int", nullable: false),
                    Order = table.Column<int>(type: "int", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ClrEndorsements", x => x.ClrEndorsementId);
                    table.ForeignKey(
                        name: "FK_ClrEndorsements_Clrs_ClrId",
                        column: x => x.ClrId,
                        principalSchema: "clr",
                        principalTable: "Clrs",
                        principalColumn: "ClrId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ClrEndorsements_Endorsements_EndorsementId",
                        column: x => x.EndorsementId,
                        principalSchema: "clr",
                        principalTable: "Endorsements",
                        principalColumn: "EndorsementId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ProfileEndorsements",
                schema: "clr",
                columns: table => new
                {
                    ProfileEndorsementId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProfileId = table.Column<int>(type: "int", nullable: false),
                    EndorsementId = table.Column<int>(type: "int", nullable: false),
                    Order = table.Column<int>(type: "int", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProfileEndorsements", x => x.ProfileEndorsementId);
                    table.ForeignKey(
                        name: "FK_ProfileEndorsements_Endorsements_EndorsementId",
                        column: x => x.EndorsementId,
                        principalSchema: "clr",
                        principalTable: "Endorsements",
                        principalColumn: "EndorsementId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ProfileEndorsements_Profiles_ProfileId",
                        column: x => x.ProfileId,
                        principalSchema: "clr",
                        principalTable: "Profiles",
                        principalColumn: "ProfileId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ResultDescriptionAlignments",
                schema: "clr",
                columns: table => new
                {
                    ResultDescriptionAlignmentId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ResultDescriptionId = table.Column<int>(type: "int", nullable: false),
                    AlignmentId = table.Column<int>(type: "int", nullable: false),
                    Order = table.Column<int>(type: "int", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ResultDescriptionAlignments", x => x.ResultDescriptionAlignmentId);
                    table.ForeignKey(
                        name: "FK_ResultDescriptionAlignments_Alignments_AlignmentId",
                        column: x => x.AlignmentId,
                        principalSchema: "clr",
                        principalTable: "Alignments",
                        principalColumn: "AlignmentId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ResultDescriptionAlignments_ResultDescriptions_ResultDescriptionId",
                        column: x => x.ResultDescriptionId,
                        principalSchema: "clr",
                        principalTable: "ResultDescriptions",
                        principalColumn: "ResultDescriptionId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "RubricCriterionLevels",
                schema: "clr",
                columns: table => new
                {
                    RubricCriterionLevelId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Order = table.Column<int>(type: "int", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Id = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Type = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Level = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Points = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AdditionalProperties = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ResultDescriptionId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RubricCriterionLevels", x => x.RubricCriterionLevelId);
                    table.ForeignKey(
                        name: "FK_RubricCriterionLevels_ResultDescriptions_ResultDescriptionId",
                        column: x => x.ResultDescriptionId,
                        principalSchema: "clr",
                        principalTable: "ResultDescriptions",
                        principalColumn: "ResultDescriptionId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "RubricCriterionLevelAlignments",
                schema: "clr",
                columns: table => new
                {
                    RubricCriterionLevelAlignmentId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RubricCriterionLevelId = table.Column<int>(type: "int", nullable: false),
                    AlignmentId = table.Column<int>(type: "int", nullable: false),
                    Order = table.Column<int>(type: "int", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RubricCriterionLevelAlignments", x => x.RubricCriterionLevelAlignmentId);
                    table.ForeignKey(
                        name: "FK_RubricCriterionLevelAlignments_Alignments_AlignmentId",
                        column: x => x.AlignmentId,
                        principalSchema: "clr",
                        principalTable: "Alignments",
                        principalColumn: "AlignmentId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_RubricCriterionLevelAlignments_RubricCriterionLevels_RubricCriterionLevelId",
                        column: x => x.RubricCriterionLevelId,
                        principalSchema: "clr",
                        principalTable: "RubricCriterionLevels",
                        principalColumn: "RubricCriterionLevelId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Clrs_LearnerId",
                schema: "clr",
                table: "Clrs",
                column: "LearnerId");

            migrationBuilder.CreateIndex(
                name: "IX_Clrs_PublisherId",
                schema: "clr",
                table: "Clrs",
                column: "PublisherId");

            migrationBuilder.CreateIndex(
                name: "IX_Clrs_VerificationId",
                schema: "clr",
                table: "Clrs",
                column: "VerificationId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Assertions_AchievementId",
                schema: "clr",
                table: "Assertions",
                column: "AchievementId",
                unique: true,
                filter: "[AchievementId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Assertions_RecipientId",
                schema: "clr",
                table: "Assertions",
                column: "RecipientId",
                unique: true,
                filter: "[RecipientId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Assertions_SourceId",
                schema: "clr",
                table: "Assertions",
                column: "SourceId");

            migrationBuilder.CreateIndex(
                name: "IX_Assertions_VerificationId",
                schema: "clr",
                table: "Assertions",
                column: "VerificationId",
                unique: true,
                filter: "[VerificationId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_AchievementAlignments_AchievementId",
                schema: "clr",
                table: "AchievementAlignments",
                column: "AchievementId");

            migrationBuilder.CreateIndex(
                name: "IX_AchievementAlignments_AlignmentId",
                schema: "clr",
                table: "AchievementAlignments",
                column: "AlignmentId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_AchievementAssociations_AchievementId",
                schema: "clr",
                table: "AchievementAssociations",
                column: "AchievementId");

            migrationBuilder.CreateIndex(
                name: "IX_AchievementAssociations_AssociationId",
                schema: "clr",
                table: "AchievementAssociations",
                column: "AssociationId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_AchievementEndorsements_AchievementId",
                schema: "clr",
                table: "AchievementEndorsements",
                column: "AchievementId");

            migrationBuilder.CreateIndex(
                name: "IX_AchievementEndorsements_EndorsementId",
                schema: "clr",
                table: "AchievementEndorsements",
                column: "EndorsementId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Achievements_CriteriaId",
                schema: "clr",
                table: "Achievements",
                column: "CriteriaId",
                unique: true,
                filter: "[CriteriaId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Achievements_IssuerProfileId",
                schema: "clr",
                table: "Achievements",
                column: "IssuerProfileId");

            migrationBuilder.CreateIndex(
                name: "IX_AssertionEndorsements_AssertionId",
                schema: "clr",
                table: "AssertionEndorsements",
                column: "AssertionId");

            migrationBuilder.CreateIndex(
                name: "IX_AssertionEndorsements_EndorsementId",
                schema: "clr",
                table: "AssertionEndorsements",
                column: "EndorsementId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_AssertionEvidence_AssertionId",
                schema: "clr",
                table: "AssertionEvidence",
                column: "AssertionId");

            migrationBuilder.CreateIndex(
                name: "IX_AssertionEvidence_EvidenceId",
                schema: "clr",
                table: "AssertionEvidence",
                column: "EvidenceId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ClrAchievements_AchievementId",
                schema: "clr",
                table: "ClrAchievements",
                column: "AchievementId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ClrAchievements_ClrId",
                schema: "clr",
                table: "ClrAchievements",
                column: "ClrId");

            migrationBuilder.CreateIndex(
                name: "IX_ClrEndorsements_ClrId",
                schema: "clr",
                table: "ClrEndorsements",
                column: "ClrId");

            migrationBuilder.CreateIndex(
                name: "IX_ClrEndorsements_EndorsementId",
                schema: "clr",
                table: "ClrEndorsements",
                column: "EndorsementId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Endorsements_EndorsementClaimId",
                schema: "clr",
                table: "Endorsements",
                column: "EndorsementClaimId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Endorsements_IssuerId",
                schema: "clr",
                table: "Endorsements",
                column: "IssuerId");

            migrationBuilder.CreateIndex(
                name: "IX_Endorsements_VerificationId",
                schema: "clr",
                table: "Endorsements",
                column: "VerificationId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ProfileEndorsements_EndorsementId",
                schema: "clr",
                table: "ProfileEndorsements",
                column: "EndorsementId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ProfileEndorsements_ProfileId",
                schema: "clr",
                table: "ProfileEndorsements",
                column: "ProfileId");

            migrationBuilder.CreateIndex(
                name: "IX_Profiles_ParentProfileId",
                schema: "clr",
                table: "Profiles",
                column: "ParentProfileId");

            migrationBuilder.CreateIndex(
                name: "IX_Profiles_VerificationId",
                schema: "clr",
                table: "Profiles",
                column: "VerificationId",
                unique: true,
                filter: "[VerificationId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_ResultAlignments_AlignmentId",
                schema: "clr",
                table: "ResultAlignments",
                column: "AlignmentId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ResultAlignments_ResultId",
                schema: "clr",
                table: "ResultAlignments",
                column: "ResultId");

            migrationBuilder.CreateIndex(
                name: "IX_ResultDescriptionAlignments_AlignmentId",
                schema: "clr",
                table: "ResultDescriptionAlignments",
                column: "AlignmentId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ResultDescriptionAlignments_ResultDescriptionId",
                schema: "clr",
                table: "ResultDescriptionAlignments",
                column: "ResultDescriptionId");

            migrationBuilder.CreateIndex(
                name: "IX_ResultDescriptions_AchievementId",
                schema: "clr",
                table: "ResultDescriptions",
                column: "AchievementId");

            migrationBuilder.CreateIndex(
                name: "IX_Results_AssertionId",
                schema: "clr",
                table: "Results",
                column: "AssertionId");

            migrationBuilder.CreateIndex(
                name: "IX_RubricCriterionLevelAlignments_AlignmentId",
                schema: "clr",
                table: "RubricCriterionLevelAlignments",
                column: "AlignmentId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_RubricCriterionLevelAlignments_RubricCriterionLevelId",
                schema: "clr",
                table: "RubricCriterionLevelAlignments",
                column: "RubricCriterionLevelId");

            migrationBuilder.CreateIndex(
                name: "IX_RubricCriterionLevels_ResultDescriptionId",
                schema: "clr",
                table: "RubricCriterionLevels",
                column: "ResultDescriptionId");

            migrationBuilder.AddForeignKey(
                name: "FK_Assertions_Achievements_AchievementId",
                schema: "clr",
                table: "Assertions",
                column: "AchievementId",
                principalSchema: "clr",
                principalTable: "Achievements",
                principalColumn: "AchievementId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Assertions_Identities_RecipientId",
                schema: "clr",
                table: "Assertions",
                column: "RecipientId",
                principalSchema: "clr",
                principalTable: "Identities",
                principalColumn: "IdentityId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Assertions_Profiles_SourceId",
                schema: "clr",
                table: "Assertions",
                column: "SourceId",
                principalSchema: "clr",
                principalTable: "Profiles",
                principalColumn: "ProfileId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Assertions_Verifications_VerificationId",
                schema: "clr",
                table: "Assertions",
                column: "VerificationId",
                principalSchema: "clr",
                principalTable: "Verifications",
                principalColumn: "VerificationId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ClrAssertions_Clrs_ClrId",
                schema: "clr",
                table: "ClrAssertions",
                column: "ClrId",
                principalSchema: "clr",
                principalTable: "Clrs",
                principalColumn: "ClrId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Clrs_Profiles_LearnerId",
                schema: "clr",
                table: "Clrs",
                column: "LearnerId",
                principalSchema: "clr",
                principalTable: "Profiles",
                principalColumn: "ProfileId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Clrs_Profiles_PublisherId",
                schema: "clr",
                table: "Clrs",
                column: "PublisherId",
                principalSchema: "clr",
                principalTable: "Profiles",
                principalColumn: "ProfileId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Clrs_Verifications_VerificationId",
                schema: "clr",
                table: "Clrs",
                column: "VerificationId",
                principalSchema: "clr",
                principalTable: "Verifications",
                principalColumn: "VerificationId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_EvidenceArtifacts_Evidence_EvidenceId",
                schema: "clr",
                table: "EvidenceArtifacts",
                column: "EvidenceId",
                principalSchema: "clr",
                principalTable: "Evidence",
                principalColumn: "EvidenceId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Links_Clrs_ClrForeignKey",
                table: "Links",
                column: "ClrForeignKey",
                principalSchema: "clr",
                principalTable: "Clrs",
                principalColumn: "ClrId",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Assertions_Achievements_AchievementId",
                schema: "clr",
                table: "Assertions");

            migrationBuilder.DropForeignKey(
                name: "FK_Assertions_Identities_RecipientId",
                schema: "clr",
                table: "Assertions");

            migrationBuilder.DropForeignKey(
                name: "FK_Assertions_Profiles_SourceId",
                schema: "clr",
                table: "Assertions");

            migrationBuilder.DropForeignKey(
                name: "FK_Assertions_Verifications_VerificationId",
                schema: "clr",
                table: "Assertions");

            migrationBuilder.DropForeignKey(
                name: "FK_ClrAssertions_Clrs_ClrId",
                schema: "clr",
                table: "ClrAssertions");

            migrationBuilder.DropForeignKey(
                name: "FK_Clrs_Profiles_LearnerId",
                schema: "clr",
                table: "Clrs");

            migrationBuilder.DropForeignKey(
                name: "FK_Clrs_Profiles_PublisherId",
                schema: "clr",
                table: "Clrs");

            migrationBuilder.DropForeignKey(
                name: "FK_Clrs_Verifications_VerificationId",
                schema: "clr",
                table: "Clrs");

            migrationBuilder.DropForeignKey(
                name: "FK_EvidenceArtifacts_Evidence_EvidenceId",
                schema: "clr",
                table: "EvidenceArtifacts");

            migrationBuilder.DropForeignKey(
                name: "FK_Links_Clrs_ClrForeignKey",
                table: "Links");

            migrationBuilder.DropTable(
                name: "AchievementAlignments",
                schema: "clr");

            migrationBuilder.DropTable(
                name: "AchievementAssociations",
                schema: "clr");

            migrationBuilder.DropTable(
                name: "AchievementEndorsements",
                schema: "clr");

            migrationBuilder.DropTable(
                name: "AssertionEndorsements",
                schema: "clr");

            migrationBuilder.DropTable(
                name: "AssertionEvidence",
                schema: "clr");

            migrationBuilder.DropTable(
                name: "ClrAchievements",
                schema: "clr");

            migrationBuilder.DropTable(
                name: "ClrEndorsements",
                schema: "clr");

            migrationBuilder.DropTable(
                name: "Identities",
                schema: "clr");

            migrationBuilder.DropTable(
                name: "ProfileEndorsements",
                schema: "clr");

            migrationBuilder.DropTable(
                name: "ResultAlignments",
                schema: "clr");

            migrationBuilder.DropTable(
                name: "ResultDescriptionAlignments",
                schema: "clr");

            migrationBuilder.DropTable(
                name: "RubricCriterionLevelAlignments",
                schema: "clr");

            migrationBuilder.DropTable(
                name: "Associations",
                schema: "clr");

            migrationBuilder.DropTable(
                name: "Endorsements",
                schema: "clr");

            migrationBuilder.DropTable(
                name: "Results",
                schema: "clr");

            migrationBuilder.DropTable(
                name: "Alignments",
                schema: "clr");

            migrationBuilder.DropTable(
                name: "RubricCriterionLevels",
                schema: "clr");

            migrationBuilder.DropTable(
                name: "EndorsementClaims",
                schema: "clr");

            migrationBuilder.DropTable(
                name: "ResultDescriptions",
                schema: "clr");

            migrationBuilder.DropTable(
                name: "Achievements",
                schema: "clr");

            migrationBuilder.DropTable(
                name: "Criteria",
                schema: "clr");

            migrationBuilder.DropTable(
                name: "Profiles",
                schema: "clr");

            migrationBuilder.DropTable(
                name: "Verifications",
                schema: "clr");

            migrationBuilder.DropPrimaryKey(
                name: "PK_DiscoveryDocumentModel",
                table: "DiscoveryDocumentModel");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Clrs",
                schema: "clr",
                table: "Clrs");

            migrationBuilder.DropIndex(
                name: "IX_Clrs_LearnerId",
                schema: "clr",
                table: "Clrs");

            migrationBuilder.DropIndex(
                name: "IX_Clrs_PublisherId",
                schema: "clr",
                table: "Clrs");

            migrationBuilder.DropIndex(
                name: "IX_Clrs_VerificationId",
                schema: "clr",
                table: "Clrs");

            migrationBuilder.DropIndex(
                name: "IX_Assertions_AchievementId",
                schema: "clr",
                table: "Assertions");

            migrationBuilder.DropIndex(
                name: "IX_Assertions_RecipientId",
                schema: "clr",
                table: "Assertions");

            migrationBuilder.DropIndex(
                name: "IX_Assertions_SourceId",
                schema: "clr",
                table: "Assertions");

            migrationBuilder.DropIndex(
                name: "IX_Assertions_VerificationId",
                schema: "clr",
                table: "Assertions");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Evidence",
                schema: "clr",
                table: "Evidence");

            migrationBuilder.DropColumn(
                name: "Type",
                schema: "clr",
                table: "Clrs");

            migrationBuilder.DropColumn(
                name: "AdditionalProperties",
                schema: "clr",
                table: "Clrs");

            migrationBuilder.DropColumn(
                name: "Context",
                schema: "clr",
                table: "Clrs");

            migrationBuilder.DropColumn(
                name: "LearnerId",
                schema: "clr",
                table: "Clrs");

            migrationBuilder.DropColumn(
                name: "Partial",
                schema: "clr",
                table: "Clrs");

            migrationBuilder.DropColumn(
                name: "PublisherId",
                schema: "clr",
                table: "Clrs");

            migrationBuilder.DropColumn(
                name: "RevocationReason",
                schema: "clr",
                table: "Clrs");

            migrationBuilder.DropColumn(
                name: "Revoked",
                schema: "clr",
                table: "Clrs");

            migrationBuilder.DropColumn(
                name: "VerificationId",
                schema: "clr",
                table: "Clrs");

            migrationBuilder.DropColumn(
                name: "AchievementId",
                schema: "clr",
                table: "Assertions");

            migrationBuilder.DropColumn(
                name: "RecipientId",
                schema: "clr",
                table: "Assertions");

            migrationBuilder.DropColumn(
                name: "SourceId",
                schema: "clr",
                table: "Assertions");

            migrationBuilder.DropColumn(
                name: "VerificationId",
                schema: "clr",
                table: "Assertions");

            migrationBuilder.RenameTable(
                name: "EvidenceArtifacts",
                schema: "clr",
                newName: "EvidenceArtifacts");

            migrationBuilder.RenameTable(
                name: "ClrSets",
                schema: "clr",
                newName: "ClrSets");

            migrationBuilder.RenameTable(
                name: "Clrs",
                schema: "clr",
                newName: "Clrs");

            migrationBuilder.RenameTable(
                name: "ClrAssertions",
                schema: "clr",
                newName: "ClrAssertions");

            migrationBuilder.RenameTable(
                name: "Assertions",
                schema: "clr",
                newName: "Assertions");

            migrationBuilder.RenameTable(
                name: "Artifacts",
                schema: "clr",
                newName: "Artifacts");

            migrationBuilder.RenameTable(
                name: "Evidence",
                schema: "clr",
                newName: "Evidences");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "Clrs",
                newName: "Identifier");

            migrationBuilder.RenameColumn(
                name: "ClrId",
                table: "Clrs",
                newName: "Id");

            migrationBuilder.AlterColumn<string>(
                name: "Id",
                table: "DiscoveryDocumentModel",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AddColumn<int>(
                name: "DiscoveryDocumentKey",
                table: "DiscoveryDocumentModel",
                type: "int",
                nullable: false,
                defaultValue: 0)
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddColumn<int>(
                name: "RecipientIdentityKey",
                table: "BadgrAssertions",
                type: "int",
                nullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "Id",
                table: "Clrs",
                type: "int",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)")
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddColumn<string>(
                name: "Achievement",
                table: "Assertions",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Endorsements",
                table: "Assertions",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Evidence",
                table: "Assertions",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Recipient",
                table: "Assertions",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Results",
                table: "Assertions",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Source",
                table: "Assertions",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Verification",
                table: "Assertions",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_DiscoveryDocumentModel",
                table: "DiscoveryDocumentModel",
                column: "DiscoveryDocumentKey");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Clrs",
                table: "Clrs",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Evidences",
                table: "Evidences",
                column: "EvidenceId");

            migrationBuilder.CreateTable(
                name: "AssertionEvidences",
                columns: table => new
                {
                    AssertionEvidenceId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AssertionId = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EvidenceId = table.Column<int>(type: "int", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    ModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Order = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AssertionEvidences", x => x.AssertionEvidenceId);
                    table.ForeignKey(
                        name: "FK_AssertionEvidences_Assertions_AssertionId",
                        column: x => x.AssertionId,
                        principalTable: "Assertions",
                        principalColumn: "AssertionId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_AssertionEvidences_Evidences_EvidenceId",
                        column: x => x.EvidenceId,
                        principalTable: "Evidences",
                        principalColumn: "EvidenceId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "AssertionClr",
                columns: table => new
                {
                    AssertionClrId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AssertionKey = table.Column<int>(type: "int", nullable: false),
                    ClrKey = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AssertionClr", x => x.AssertionClrId);
                    table.ForeignKey(
                        name: "FK_AssertionClr_ClrDType_ClrKey",
                        column: x => x.ClrKey,
                        principalTable: "ClrDType",
                        principalColumn: "ClrKey",
                        onDelete: ReferentialAction.Restrict);
                });

            
            migrationBuilder.CreateIndex(
                name: "IX_BadgrAssertions_RecipientIdentityKey",
                table: "BadgrAssertions",
                column: "RecipientIdentityKey");

            migrationBuilder.CreateIndex(
                name: "IX_AssertionEvidences_AssertionId",
                table: "AssertionEvidences",
                column: "AssertionId");

            migrationBuilder.CreateIndex(
                name: "IX_AssertionEvidences_EvidenceId",
                table: "AssertionEvidences",
                column: "EvidenceId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_ClrAssertions_Clrs_ClrId",
                table: "ClrAssertions",
                column: "ClrId",
                principalTable: "Clrs",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_EvidenceArtifacts_Evidences_EvidenceId",
                table: "EvidenceArtifacts",
                column: "EvidenceId",
                principalTable: "Evidences",
                principalColumn: "EvidenceId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Links_Clrs_ClrForeignKey",
                table: "Links",
                column: "ClrForeignKey",
                principalTable: "Clrs",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
