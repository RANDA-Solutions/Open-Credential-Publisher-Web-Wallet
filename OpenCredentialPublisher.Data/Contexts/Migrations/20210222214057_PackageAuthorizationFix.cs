using Microsoft.EntityFrameworkCore.Migrations;

namespace OpenCredentialPublisher.Data.Contexts.Migrations
{
    public partial class PackageAuthorizationFix : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CredentialPackages_Authorizations_AuthorizationId",
                table: "CredentialPackages");

            migrationBuilder.DropIndex(
                name: "IX_CredentialPackages_AuthorizationId",
                table: "CredentialPackages");

            migrationBuilder.DropColumn(
                name: "AuthorizationId",
                table: "CredentialPackages");

            migrationBuilder.CreateIndex(
                name: "IX_CredentialPackages_AuthorizationForeignKey",
                table: "CredentialPackages",
                column: "AuthorizationForeignKey");

            migrationBuilder.AddForeignKey(
                name: "FK_CredentialPackages_Authorizations_AuthorizationForeignKey",
                table: "CredentialPackages",
                column: "AuthorizationForeignKey",
                principalTable: "Authorizations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CredentialPackages_Authorizations_AuthorizationForeignKey",
                table: "CredentialPackages");

            migrationBuilder.DropIndex(
                name: "IX_CredentialPackages_AuthorizationForeignKey",
                table: "CredentialPackages");

            migrationBuilder.AddColumn<string>(
                name: "AuthorizationId",
                table: "CredentialPackages",
                type: "nvarchar(450)",
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
    }
}
