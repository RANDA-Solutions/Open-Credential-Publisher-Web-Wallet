using Microsoft.EntityFrameworkCore.Migrations;

namespace OpenCredentialPublisher.Data.Contexts.Migrations
{
    public partial class SharingCredential : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CredentialRequestId",
                table: "Links",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Id",
                table: "BadgrAssertions",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.InsertData(
                table: "ShareTypes",
                columns: new[] { "Id", "Name" },
                values: new object[] { 3, "Wallet" });

            migrationBuilder.CreateIndex(
                name: "IX_Links_CredentialRequestId",
                table: "Links",
                column: "CredentialRequestId");

            migrationBuilder.AddForeignKey(
                name: "FK_Links_CredentialRequests_CredentialRequestId",
                table: "Links",
                column: "CredentialRequestId",
                principalTable: "CredentialRequests",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Links_CredentialRequests_CredentialRequestId",
                table: "Links");

            migrationBuilder.DropIndex(
                name: "IX_Links_CredentialRequestId",
                table: "Links");

            migrationBuilder.DeleteData(
                table: "ShareTypes",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DropColumn(
                name: "CredentialRequestId",
                table: "Links");

            migrationBuilder.AlterColumn<string>(
                name: "Id",
                table: "BadgrAssertions",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);
        }
    }
}
