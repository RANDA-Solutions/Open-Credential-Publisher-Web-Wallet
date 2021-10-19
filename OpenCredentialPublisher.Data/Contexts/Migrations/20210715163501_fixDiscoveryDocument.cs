using Microsoft.EntityFrameworkCore.Migrations;

namespace OpenCredentialPublisher.Data.Contexts.Migrations
{
    public partial class fixDiscoveryDocument : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DiscoveryDocumentModel",
                schema: "cred");

            migrationBuilder.CreateTable(
                name: "DiscoveryDocumentModel",
                schema: "cred",
                columns: table => new
                {
                    DiscoveryDocumentId = table.Column<int>(nullable: false)
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
                    Id = table.Column<string>(nullable: true),
                    SourceForeignKey = table.Column<int>(nullable: false),
                    ApiBase = table.Column<string>(nullable: true),
                    TokenRevocationUrl = table.Column<string>(nullable: true),
                    Version = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DiscoveryDocumentModel", x => x.DiscoveryDocumentId);
                    table.ForeignKey(
                        name: "FK_DiscoveryDocumentModel_Sources_SourceForeignKey",
                        column: x => x.SourceForeignKey,
                        principalTable: "Sources",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                 name: "DiscoveryDocumentModel",
                schema: "cred");

            migrationBuilder.CreateTable(
                name: "DiscoveryDocumentModel",
                schema: "cred",
                columns: table => new
                {
                    DiscoveryDocumentId = table.Column<int>(nullable: false)
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
                    Id = table.Column<string>(nullable: true),
                    SourceForeignKey = table.Column<int>(nullable: false),
                    ApiBase = table.Column<string>(nullable: true),
                    TokenRevocationUrl = table.Column<string>(nullable: true),
                    Version = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DiscoveryDocumentModel", x => x.DiscoveryDocumentId);
                    table.ForeignKey(
                        name: "FK_DiscoveryDocumentModel_Sources_SourceForeignKey",
                        column: x => x.SourceForeignKey,
                        principalTable: "Sources",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });
        }
    }
}
