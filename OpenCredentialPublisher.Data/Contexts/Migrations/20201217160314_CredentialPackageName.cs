using Microsoft.EntityFrameworkCore.Migrations;

namespace OpenCredentialPublisher.Data.Contexts.Migrations
{
    public partial class CredentialPackageName : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "CredentialPackages",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Name",
                table: "CredentialPackages");
        }
    }
}
