using Microsoft.EntityFrameworkCore.Migrations;

namespace OpenCredentialPublisher.Data.Contexts.Migrations
{
    public partial class FixWalletRelationship : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CredentialRequests_WalletRelationships_Id",
                table: "CredentialRequests");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddForeignKey(
                name: "FK_CredentialRequests_WalletRelationships_Id",
                table: "CredentialRequests",
                column: "Id",
                principalTable: "WalletRelationships",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
