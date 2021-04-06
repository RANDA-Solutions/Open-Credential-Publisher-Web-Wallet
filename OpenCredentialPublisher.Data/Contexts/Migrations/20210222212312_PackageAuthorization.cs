using Microsoft.EntityFrameworkCore.Migrations;

namespace OpenCredentialPublisher.Data.Contexts.Migrations
{
    public partial class PackageAuthorization : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "AuthorizationForeignKey",
                table: "CredentialPackages",
                maxLength: 450,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "AuthorizationId",
                table: "CredentialPackages",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_CredentialPackages_AuthorizationId",
                table: "CredentialPackages",
                column: "AuthorizationId");

            migrationBuilder.AddForeignKey(
                name: "FK_CredentialPackages_Authorizations_AuthorizationId",
                table: "CredentialPackages",
                column: "AuthorizationId",
                principalTable: "Authorizations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CredentialPackages_Authorizations_AuthorizationId",
                table: "CredentialPackages");

            migrationBuilder.DropIndex(
                name: "IX_CredentialPackages_AuthorizationId",
                table: "CredentialPackages");

            migrationBuilder.DropColumn(
                name: "AuthorizationForeignKey",
                table: "CredentialPackages");

            migrationBuilder.DropColumn(
                name: "AuthorizationId",
                table: "CredentialPackages");
        }
    }
}
