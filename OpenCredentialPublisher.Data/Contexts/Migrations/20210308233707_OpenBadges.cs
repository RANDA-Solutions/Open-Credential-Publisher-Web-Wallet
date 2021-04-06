using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace OpenCredentialPublisher.Data.Contexts.Migrations
{
    public partial class OpenBadges : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "BadgrBackpacks",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CredentialPackageId = table.Column<int>(nullable: false),
                    AssertionsCount = table.Column<int>(nullable: false),
                    IssuedOn = table.Column<DateTime>(nullable: false),
                    Identifier = table.Column<string>(nullable: true),
                    Json = table.Column<string>(nullable: true),
                    Provider = table.Column<string>(nullable: true),
                    Name = table.Column<string>(nullable: true),
                    Revoked = table.Column<bool>(nullable: false),
                    RevocationReason = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BadgrBackpacks", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BadgrBackpacks_CredentialPackages_CredentialPackageId",
                        column: x => x.CredentialPackageId,
                        principalTable: "CredentialPackages",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "IdentityDType",
                columns: table => new
                {
                    IdentityKey = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Id = table.Column<string>(nullable: true),
                    Type = table.Column<string>(nullable: false),
                    Identity = table.Column<string>(nullable: false),
                    Hashed = table.Column<bool>(nullable: false),
                    Salt = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_IdentityDType", x => x.IdentityKey);
                });

            migrationBuilder.CreateTable(
                name: "BadgrAssertions",
                columns: table => new
                {
                    BadgrAssertionId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Id = table.Column<string>(nullable: false),
                    Type = table.Column<string>(nullable: true),
                    Acceptance = table.Column<string>(nullable: true),
                    OpenBadgeId = table.Column<string>(nullable: true),
                    Badgeclass = table.Column<string>(nullable: true),
                    BadgeClassOpenBadgeId = table.Column<string>(nullable: true),
                    Issuer = table.Column<string>(nullable: true),
                    IssuerOpenBadgeId = table.Column<string>(nullable: true),
                    Image = table.Column<string>(nullable: true),
                    RecipientIdentityKey = table.Column<int>(nullable: true),
                    IssuedOn = table.Column<DateTime>(nullable: true),
                    Narrative = table.Column<string>(nullable: true),
                    Revoked = table.Column<bool>(nullable: true),
                    RevocationReason = table.Column<string>(nullable: true),
                    Expires = table.Column<long>(nullable: true),
                    Pending = table.Column<bool>(nullable: false),
                    IssueStatus = table.Column<string>(nullable: true),
                    ValidationStatus = table.Column<string>(nullable: true),
                    AdditionalProperties = table.Column<string>(nullable: true),
                    SignedAssertion = table.Column<string>(nullable: true),
                    IssuerJson = table.Column<string>(nullable: true),
                    BadgeJson = table.Column<string>(nullable: true),
                    BadgeClassJson = table.Column<string>(nullable: true),
                    BadgrBackpackId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BadgrAssertions", x => x.BadgrAssertionId);
                    table.ForeignKey(
                        name: "FK_BadgrAssertions_BadgrBackpacks_BadgrBackpackId",
                        column: x => x.BadgrBackpackId,
                        principalTable: "BadgrBackpacks",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BadgrAssertions_IdentityDType_RecipientIdentityKey",
                        column: x => x.RecipientIdentityKey,
                        principalTable: "IdentityDType",
                        principalColumn: "IdentityKey",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_BadgrAssertions_BadgrBackpackId",
                table: "BadgrAssertions",
                column: "BadgrBackpackId");

            migrationBuilder.CreateIndex(
                name: "IX_BadgrAssertions_RecipientIdentityKey",
                table: "BadgrAssertions",
                column: "RecipientIdentityKey");

            migrationBuilder.CreateIndex(
                name: "IX_BadgrBackpacks_CredentialPackageId",
                table: "BadgrBackpacks",
                column: "CredentialPackageId",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BadgrAssertions");

            migrationBuilder.DropTable(
                name: "BadgrBackpacks");

            migrationBuilder.DropTable(
                name: "IdentityDType");
        }
    }
}
