using Microsoft.EntityFrameworkCore.Migrations;

namespace OpenCredentialPublisher.Data.Contexts.Migrations
{
    public partial class FixPublicKeysNSystemIdentifiers : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Identifiers",
                schema: "cred",
                table: "Profiles",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PublicKey",
                schema: "cred",
                table: "Profiles",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Identifiers",
                schema: "cred",
                table: "Achievements",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Identifiers",
                schema: "cred",
                table: "Profiles");

            migrationBuilder.DropColumn(
                name: "PublicKey",
                schema: "cred",
                table: "Profiles");

            migrationBuilder.DropColumn(
                name: "Identifiers",
                schema: "cred",
                table: "Achievements");
        }
    }
}
