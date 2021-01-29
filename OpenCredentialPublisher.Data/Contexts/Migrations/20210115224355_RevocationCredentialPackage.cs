using Microsoft.EntityFrameworkCore.Migrations;

namespace OpenCredentialPublisher.Data.Contexts.Migrations
{
    public partial class RevocationCredentialPackage : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "RevocationReason",
                table: "CredentialPackages",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "Revoked",
                table: "CredentialPackages",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RevocationReason",
                table: "CredentialPackages");

            migrationBuilder.DropColumn(
                name: "Revoked",
                table: "CredentialPackages");
        }
    }
}
