using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace OpenCredentialPublisher.Data.Contexts.Migrations
{
    public partial class Identity : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AspNetRoles",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    Name = table.Column<string>(maxLength: 256, nullable: true),
                    NormalizedName = table.Column<string>(maxLength: 256, nullable: true),
                    ConcurrencyStamp = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUsers",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    UserName = table.Column<string>(maxLength: 256, nullable: true),
                    NormalizedUserName = table.Column<string>(maxLength: 256, nullable: true),
                    Email = table.Column<string>(maxLength: 256, nullable: true),
                    NormalizedEmail = table.Column<string>(maxLength: 256, nullable: true),
                    EmailConfirmed = table.Column<bool>(nullable: false),
                    PasswordHash = table.Column<string>(nullable: true),
                    SecurityStamp = table.Column<string>(nullable: true),
                    ConcurrencyStamp = table.Column<string>(nullable: true),
                    PhoneNumber = table.Column<string>(nullable: true),
                    PhoneNumberConfirmed = table.Column<bool>(nullable: false),
                    TwoFactorEnabled = table.Column<bool>(nullable: false),
                    LockoutEnd = table.Column<DateTimeOffset>(nullable: true),
                    LockoutEnabled = table.Column<bool>(nullable: false),
                    AccessFailedCount = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUsers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Certificates",
                columns: table => new
                {
                    Host = table.Column<string>(nullable: false),
                    IssuedByName = table.Column<string>(nullable: true),
                    IssuedToName = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Certificates", x => x.Host);
                });

            migrationBuilder.CreateTable(
                name: "CredentialPackages",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TypeId = table.Column<int>(nullable: false),
                    CreatedAt = table.Column<DateTime>(nullable: false),
                    ModifiedAt = table.Column<DateTime>(nullable: false),
                    UserName = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CredentialPackages", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Sources",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ClientId = table.Column<string>(nullable: true),
                    ClientSecret = table.Column<string>(nullable: true),
                    Name = table.Column<string>(nullable: true),
                    Scope = table.Column<string>(nullable: true),
                    Url = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sources", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AspNetRoleClaims",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RoleId = table.Column<string>(nullable: false),
                    ClaimType = table.Column<string>(nullable: true),
                    ClaimValue = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoleClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetRoleClaims_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserClaims",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(nullable: false),
                    ClaimType = table.Column<string>(nullable: true),
                    ClaimValue = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetUserClaims_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserLogins",
                columns: table => new
                {
                    LoginProvider = table.Column<string>(nullable: false),
                    ProviderKey = table.Column<string>(nullable: false),
                    ProviderDisplayName = table.Column<string>(nullable: true),
                    UserId = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserLogins", x => new { x.LoginProvider, x.ProviderKey });
                    table.ForeignKey(
                        name: "FK_AspNetUserLogins_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserRoles",
                columns: table => new
                {
                    UserId = table.Column<string>(nullable: false),
                    RoleId = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserRoles", x => new { x.UserId, x.RoleId });
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserTokens",
                columns: table => new
                {
                    UserId = table.Column<string>(nullable: false),
                    LoginProvider = table.Column<string>(nullable: false),
                    Name = table.Column<string>(nullable: false),
                    Value = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserTokens", x => new { x.UserId, x.LoginProvider, x.Name });
                    table.ForeignKey(
                        name: "FK_AspNetUserTokens_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "VerifiableCredentials",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CredentialPackageId = table.Column<int>(nullable: false),
                    CredentialsCount = table.Column<int>(nullable: false),
                    IssuedOn = table.Column<DateTime>(nullable: false),
                    Identifier = table.Column<string>(nullable: true),
                    Json = table.Column<string>(nullable: true),
                    Issuer = table.Column<string>(nullable: true),
                    Name = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VerifiableCredentials", x => x.Id);
                    table.ForeignKey(
                        name: "FK_VerifiableCredentials_CredentialPackages_CredentialPackageId",
                        column: x => x.CredentialPackageId,
                        principalTable: "CredentialPackages",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Authorizations",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    AccessToken = table.Column<string>(nullable: true),
                    AuthorizationCode = table.Column<string>(nullable: true),
                    CodeVerifier = table.Column<string>(nullable: true),
                    RefreshToken = table.Column<string>(nullable: true),
                    Scopes = table.Column<string>(nullable: true),
                    SourceForeignKey = table.Column<int>(nullable: false),
                    UserName = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Authorizations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Authorizations_Sources_SourceForeignKey",
                        column: x => x.SourceForeignKey,
                        principalTable: "Sources",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "DiscoveryDocumentModel",
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

            migrationBuilder.CreateTable(
                name: "ClrSets",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CredentialPackageId = table.Column<int>(nullable: true),
                    VerifiableCredentialId = table.Column<int>(nullable: true),
                    ClrsCount = table.Column<int>(nullable: false),
                    Identifier = table.Column<string>(nullable: true),
                    Json = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ClrSets", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ClrSets_CredentialPackages_CredentialPackageId",
                        column: x => x.CredentialPackageId,
                        principalTable: "CredentialPackages",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ClrSets_VerifiableCredentials_VerifiableCredentialId",
                        column: x => x.VerifiableCredentialId,
                        principalTable: "VerifiableCredentials",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Clrs",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CredentialPackageId = table.Column<int>(nullable: true),
                    VerifiableCredentialId = table.Column<int>(nullable: true),
                    ClrSetId = table.Column<int>(nullable: true),
                    AssertionsCount = table.Column<int>(nullable: false),
                    AuthorizationForeignKey = table.Column<string>(nullable: true),
                    IssuedOn = table.Column<DateTime>(nullable: false),
                    Identifier = table.Column<string>(nullable: true),
                    Json = table.Column<string>(nullable: true),
                    LearnerName = table.Column<string>(nullable: true),
                    Name = table.Column<string>(nullable: true),
                    PublisherName = table.Column<string>(nullable: true),
                    RefreshedAt = table.Column<DateTime>(nullable: false),
                    SignedClr = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Clrs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Clrs_Authorizations_AuthorizationForeignKey",
                        column: x => x.AuthorizationForeignKey,
                        principalTable: "Authorizations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Clrs_ClrSets_ClrSetId",
                        column: x => x.ClrSetId,
                        principalTable: "ClrSets",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Clrs_CredentialPackages_CredentialPackageId",
                        column: x => x.CredentialPackageId,
                        principalTable: "CredentialPackages",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Clrs_VerifiableCredentials_VerifiableCredentialId",
                        column: x => x.VerifiableCredentialId,
                        principalTable: "VerifiableCredentials",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Links",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    ClrForeignKey = table.Column<int>(nullable: false),
                    DisplayCount = table.Column<int>(nullable: false),
                    Nickname = table.Column<string>(nullable: true),
                    UserName = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Links", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Links_Clrs_ClrForeignKey",
                        column: x => x.ClrForeignKey,
                        principalTable: "Clrs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AspNetRoleClaims_RoleId",
                table: "AspNetRoleClaims",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "RoleNameIndex",
                table: "AspNetRoles",
                column: "NormalizedName",
                unique: true,
                filter: "[NormalizedName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserClaims_UserId",
                table: "AspNetUserClaims",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserLogins_UserId",
                table: "AspNetUserLogins",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserRoles_RoleId",
                table: "AspNetUserRoles",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "EmailIndex",
                table: "AspNetUsers",
                column: "NormalizedEmail");

            migrationBuilder.CreateIndex(
                name: "UserNameIndex",
                table: "AspNetUsers",
                column: "NormalizedUserName",
                unique: true,
                filter: "[NormalizedUserName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Authorizations_SourceForeignKey",
                table: "Authorizations",
                column: "SourceForeignKey");

            migrationBuilder.CreateIndex(
                name: "IX_Clrs_AuthorizationForeignKey",
                table: "Clrs",
                column: "AuthorizationForeignKey");

            migrationBuilder.CreateIndex(
                name: "IX_Clrs_ClrSetId",
                table: "Clrs",
                column: "ClrSetId");

            migrationBuilder.CreateIndex(
                name: "IX_Clrs_CredentialPackageId",
                table: "Clrs",
                column: "CredentialPackageId",
                unique: true,
                filter: "[CredentialPackageId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Clrs_VerifiableCredentialId",
                table: "Clrs",
                column: "VerifiableCredentialId");

            migrationBuilder.CreateIndex(
                name: "IX_ClrSets_CredentialPackageId",
                table: "ClrSets",
                column: "CredentialPackageId",
                unique: true,
                filter: "[CredentialPackageId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_ClrSets_VerifiableCredentialId",
                table: "ClrSets",
                column: "VerifiableCredentialId");

            migrationBuilder.CreateIndex(
                name: "IX_DiscoveryDocumentModel_SourceForeignKey",
                table: "DiscoveryDocumentModel",
                column: "SourceForeignKey",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Links_ClrForeignKey",
                table: "Links",
                column: "ClrForeignKey");

            migrationBuilder.CreateIndex(
                name: "IX_VerifiableCredentials_CredentialPackageId",
                table: "VerifiableCredentials",
                column: "CredentialPackageId",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AspNetRoleClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserLogins");

            migrationBuilder.DropTable(
                name: "AspNetUserRoles");

            migrationBuilder.DropTable(
                name: "AspNetUserTokens");

            migrationBuilder.DropTable(
                name: "Certificates");

            migrationBuilder.DropTable(
                name: "DiscoveryDocumentModel");

            migrationBuilder.DropTable(
                name: "Links");

            migrationBuilder.DropTable(
                name: "AspNetRoles");

            migrationBuilder.DropTable(
                name: "AspNetUsers");

            migrationBuilder.DropTable(
                name: "Clrs");

            migrationBuilder.DropTable(
                name: "Authorizations");

            migrationBuilder.DropTable(
                name: "ClrSets");

            migrationBuilder.DropTable(
                name: "Sources");

            migrationBuilder.DropTable(
                name: "VerifiableCredentials");

            migrationBuilder.DropTable(
                name: "CredentialPackages");
        }
    }
}
