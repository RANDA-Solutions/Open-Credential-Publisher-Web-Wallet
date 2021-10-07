using Microsoft.EntityFrameworkCore.Migrations;

namespace OpenCredentialPublisher.Data.Contexts.Migrations
{
    public partial class credSchema : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DiscoveryDocumentModel");

            migrationBuilder.EnsureSchema(
                name: "cred");

            migrationBuilder.CreateTable(
                name: "DiscoveryDocumentModel",
                schema: "cred",
                columns: table => new
                {
                    DiscoveryDocumentKey = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Context = table.Column<string>(nullable: true),
                    AuthorizationUrl = table.Column<string>(nullable: false),
                    Image = table.Column<string>(nullable: true),
                    Name = table.Column<string>(nullable: false),
                    PrivacyPolicyUrl = table.Column<string>(nullable: false),
                    RegistrationUrl = table.Column<string>(nullable: false),
                    ScopesOffered = table.Column<string>(nullable: false),
                    TermsOfServiceUrl = table.Column<string>(nullable: false),
                    TokenUrl = table.Column<string>(nullable: false),
                    Id = table.Column<int>(nullable: false),
                    SourceForeignKey = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DiscoveryDocumentModel", x => x.DiscoveryDocumentKey);
                    table.ForeignKey(
                        name: "FK_DiscoveryDocumentModel_Sources_SourceForeignKey",
                        column: x => x.SourceForeignKey,
                        principalTable: "Sources",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });
            migrationBuilder.RenameTable(
                name: "Verifications",
                schema: "clr",
                newName: "Verifications",
                newSchema: "cred");

            migrationBuilder.RenameTable(
                name: "VerifiableCredentials",
                newName: "VerifiableCredentials",
                newSchema: "cred");

            migrationBuilder.RenameTable(
                name: "RubricCriterionLevels",
                schema: "clr",
                newName: "RubricCriterionLevels",
                newSchema: "cred");

            migrationBuilder.RenameTable(
                name: "RubricCriterionLevelAlignments",
                schema: "clr",
                newName: "RubricCriterionLevelAlignments",
                newSchema: "cred");

            migrationBuilder.RenameTable(
                name: "Results",
                schema: "clr",
                newName: "Results",
                newSchema: "cred");

            migrationBuilder.RenameTable(
                name: "ResultDescriptions",
                schema: "clr",
                newName: "ResultDescriptions",
                newSchema: "cred");

            migrationBuilder.RenameTable(
                name: "ResultDescriptionAlignments",
                schema: "clr",
                newName: "ResultDescriptionAlignments",
                newSchema: "cred");

            migrationBuilder.RenameTable(
                name: "ResultAlignments",
                schema: "clr",
                newName: "ResultAlignments",
                newSchema: "cred");

            migrationBuilder.RenameTable(
                name: "Profiles",
                schema: "clr",
                newName: "Profiles",
                newSchema: "cred");

            migrationBuilder.RenameTable(
                name: "ProfileEndorsements",
                schema: "clr",
                newName: "ProfileEndorsements",
                newSchema: "cred");

            migrationBuilder.RenameTable(
                name: "Identities",
                schema: "clr",
                newName: "Identities",
                newSchema: "cred");

            migrationBuilder.RenameTable(
                name: "EvidenceArtifacts",
                schema: "clr",
                newName: "EvidenceArtifacts",
                newSchema: "cred");

            migrationBuilder.RenameTable(
                name: "Evidence",
                schema: "clr",
                newName: "Evidence",
                newSchema: "cred");

            migrationBuilder.RenameTable(
                name: "Endorsements",
                schema: "clr",
                newName: "Endorsements",
                newSchema: "cred");

            migrationBuilder.RenameTable(
                name: "EndorsementClaims",
                schema: "clr",
                newName: "EndorsementClaims",
                newSchema: "cred");

            migrationBuilder.RenameTable(
                name: "Criteria",
                schema: "clr",
                newName: "Criteria",
                newSchema: "cred");

            migrationBuilder.RenameTable(
                name: "CredentialPackages",
                newName: "CredentialPackages",
                newSchema: "cred");

            migrationBuilder.RenameTable(
                name: "ClrSets",
                schema: "clr",
                newName: "ClrSets",
                newSchema: "cred");

            migrationBuilder.RenameTable(
                name: "Clrs",
                schema: "clr",
                newName: "Clrs",
                newSchema: "cred");

            migrationBuilder.RenameTable(
                name: "ClrEndorsements",
                schema: "clr",
                newName: "ClrEndorsements",
                newSchema: "cred");

            migrationBuilder.RenameTable(
                name: "ClrAssertions",
                schema: "clr",
                newName: "ClrAssertions",
                newSchema: "cred");

            migrationBuilder.RenameTable(
                name: "ClrAchievements",
                schema: "clr",
                newName: "ClrAchievements",
                newSchema: "cred");

            migrationBuilder.RenameTable(
                name: "BadgrBackpacks",
                newName: "BadgrBackpacks",
                newSchema: "cred");

            migrationBuilder.RenameTable(
                name: "BadgrAssertions",
                newName: "BadgrAssertions",
                newSchema: "cred");

            migrationBuilder.RenameTable(
                name: "Associations",
                schema: "clr",
                newName: "Associations",
                newSchema: "cred");

            migrationBuilder.RenameTable(
                name: "Assertions",
                schema: "clr",
                newName: "Assertions",
                newSchema: "cred");

            migrationBuilder.RenameTable(
                name: "AssertionEvidence",
                schema: "clr",
                newName: "AssertionEvidence",
                newSchema: "cred");

            migrationBuilder.RenameTable(
                name: "AssertionEndorsements",
                schema: "clr",
                newName: "AssertionEndorsements",
                newSchema: "cred");

            migrationBuilder.RenameTable(
                name: "Artifacts",
                schema: "clr",
                newName: "Artifacts",
                newSchema: "cred");

            migrationBuilder.RenameTable(
                name: "Alignments",
                schema: "clr",
                newName: "Alignments",
                newSchema: "cred");

            migrationBuilder.RenameTable(
                name: "Achievements",
                schema: "clr",
                newName: "Achievements",
                newSchema: "cred");

            migrationBuilder.RenameTable(
                name: "AchievementEndorsements",
                schema: "clr",
                newName: "AchievementEndorsements",
                newSchema: "cred");

            migrationBuilder.RenameTable(
                name: "AchievementAssociations",
                schema: "clr",
                newName: "AchievementAssociations",
                newSchema: "cred");

            migrationBuilder.RenameTable(
                name: "AchievementAlignments",
                schema: "clr",
                newName: "AchievementAlignments",
                newSchema: "cred");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

            migrationBuilder.EnsureSchema(
                name: "clr");

            migrationBuilder.RenameTable(
                name: "Verifications",
                schema: "cred",
                newName: "Verifications",
                newSchema: "clr");

            migrationBuilder.RenameTable(
                name: "VerifiableCredentials",
                schema: "cred",
                newName: "VerifiableCredentials");

            migrationBuilder.RenameTable(
                name: "RubricCriterionLevels",
                schema: "cred",
                newName: "RubricCriterionLevels",
                newSchema: "clr");

            migrationBuilder.RenameTable(
                name: "RubricCriterionLevelAlignments",
                schema: "cred",
                newName: "RubricCriterionLevelAlignments",
                newSchema: "clr");

            migrationBuilder.RenameTable(
                name: "Results",
                schema: "cred",
                newName: "Results",
                newSchema: "clr");

            migrationBuilder.RenameTable(
                name: "ResultDescriptions",
                schema: "cred",
                newName: "ResultDescriptions",
                newSchema: "clr");

            migrationBuilder.RenameTable(
                name: "ResultDescriptionAlignments",
                schema: "cred",
                newName: "ResultDescriptionAlignments",
                newSchema: "clr");

            migrationBuilder.RenameTable(
                name: "ResultAlignments",
                schema: "cred",
                newName: "ResultAlignments",
                newSchema: "clr");

            migrationBuilder.RenameTable(
                name: "Profiles",
                schema: "cred",
                newName: "Profiles",
                newSchema: "clr");

            migrationBuilder.RenameTable(
                name: "ProfileEndorsements",
                schema: "cred",
                newName: "ProfileEndorsements",
                newSchema: "clr");

            migrationBuilder.RenameTable(
                name: "Identities",
                schema: "cred",
                newName: "Identities",
                newSchema: "clr");

            migrationBuilder.RenameTable(
                name: "EvidenceArtifacts",
                schema: "cred",
                newName: "EvidenceArtifacts",
                newSchema: "clr");

            migrationBuilder.RenameTable(
                name: "Evidence",
                schema: "cred",
                newName: "Evidence",
                newSchema: "clr");

            migrationBuilder.RenameTable(
                name: "Endorsements",
                schema: "cred",
                newName: "Endorsements",
                newSchema: "clr");

            migrationBuilder.RenameTable(
                name: "EndorsementClaims",
                schema: "cred",
                newName: "EndorsementClaims",
                newSchema: "clr");

            migrationBuilder.RenameTable(
                name: "DiscoveryDocumentModel",
                schema: "cred",
                newName: "DiscoveryDocumentModel",
                newSchema: "clr");

            migrationBuilder.RenameTable(
                name: "Criteria",
                schema: "cred",
                newName: "Criteria",
                newSchema: "clr");

            migrationBuilder.RenameTable(
                name: "CredentialPackages",
                schema: "cred",
                newName: "CredentialPackages");

            migrationBuilder.RenameTable(
                name: "ClrSets",
                schema: "cred",
                newName: "ClrSets",
                newSchema: "clr");

            migrationBuilder.RenameTable(
                name: "Clrs",
                schema: "cred",
                newName: "Clrs",
                newSchema: "clr");

            migrationBuilder.RenameTable(
                name: "ClrEndorsements",
                schema: "cred",
                newName: "ClrEndorsements",
                newSchema: "clr");

            migrationBuilder.RenameTable(
                name: "ClrAssertions",
                schema: "cred",
                newName: "ClrAssertions",
                newSchema: "clr");

            migrationBuilder.RenameTable(
                name: "ClrAchievements",
                schema: "cred",
                newName: "ClrAchievements",
                newSchema: "clr");

            migrationBuilder.RenameTable(
                name: "BadgrBackpacks",
                schema: "cred",
                newName: "BadgrBackpacks");

            migrationBuilder.RenameTable(
                name: "BadgrAssertions",
                schema: "cred",
                newName: "BadgrAssertions");

            migrationBuilder.RenameTable(
                name: "Associations",
                schema: "cred",
                newName: "Associations",
                newSchema: "clr");

            migrationBuilder.RenameTable(
                name: "Assertions",
                schema: "cred",
                newName: "Assertions",
                newSchema: "clr");

            migrationBuilder.RenameTable(
                name: "AssertionEvidence",
                schema: "cred",
                newName: "AssertionEvidence",
                newSchema: "clr");

            migrationBuilder.RenameTable(
                name: "AssertionEndorsements",
                schema: "cred",
                newName: "AssertionEndorsements",
                newSchema: "clr");

            migrationBuilder.RenameTable(
                name: "Artifacts",
                schema: "cred",
                newName: "Artifacts",
                newSchema: "clr");

            migrationBuilder.RenameTable(
                name: "Alignments",
                schema: "cred",
                newName: "Alignments",
                newSchema: "clr");

            migrationBuilder.RenameTable(
                name: "Achievements",
                schema: "cred",
                newName: "Achievements",
                newSchema: "clr");

            migrationBuilder.RenameTable(
                name: "AchievementEndorsements",
                schema: "cred",
                newName: "AchievementEndorsements",
                newSchema: "clr");

            migrationBuilder.RenameTable(
                name: "AchievementAssociations",
                schema: "cred",
                newName: "AchievementAssociations",
                newSchema: "clr");

            migrationBuilder.RenameTable(
                name: "AchievementAlignments",
                schema: "cred",
                newName: "AchievementAlignments",
                newSchema: "clr");
        }
    }
}
