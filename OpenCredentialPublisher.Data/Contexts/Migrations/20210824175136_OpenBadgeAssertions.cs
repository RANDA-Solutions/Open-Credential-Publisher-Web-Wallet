using Microsoft.EntityFrameworkCore.Migrations;

namespace OpenCredentialPublisher.Data.Contexts.Migrations
{
    public partial class OpenBadgeAssertions : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsBadgr",
                schema: "cred",
                table: "BadgrAssertions",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsValidJson",
                schema: "cred",
                table: "BadgrAssertions",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "Json",
                schema: "cred",
                table: "BadgrAssertions",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsBadgr",
                schema: "cred",
                table: "BadgrAssertions");

            migrationBuilder.DropColumn(
                name: "IsValidJson",
                schema: "cred",
                table: "BadgrAssertions");

            migrationBuilder.DropColumn(
                name: "Json",
                schema: "cred",
                table: "BadgrAssertions");
        }
    }
}
