using Microsoft.EntityFrameworkCore.Migrations;

namespace OpenCredentialPublisher.Data.Contexts.Migrations
{
    public partial class Revocation : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "RevocationReason",
                table: "VerifiableCredentials",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "Revoked",
                table: "VerifiableCredentials",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RevocationReason",
                table: "VerifiableCredentials");

            migrationBuilder.DropColumn(
                name: "Revoked",
                table: "VerifiableCredentials");
        }
    }
}
