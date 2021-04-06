using Microsoft.EntityFrameworkCore.Migrations;

namespace OpenCredentialPublisher.Data.Contexts.Migrations
{
    public partial class AddedCredentialRequestSteps : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ErrorMessage",
                table: "CredentialRequests",
                nullable: true);

            migrationBuilder.InsertData(
                table: "CredentialRequestSteps",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 8, "CheckingRevocationStatus" },
                    { 9, "CredentialIsRevoked" },
                    { 10, "CredentialIsStillValid" },
                    { 14, "ErrorWritingSchema" },
                    { 15, "ErrorWritingCredentialDefinition" }
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "CredentialRequestSteps",
                keyColumn: "Id",
                keyValue: 8);

            migrationBuilder.DeleteData(
                table: "CredentialRequestSteps",
                keyColumn: "Id",
                keyValue: 9);

            migrationBuilder.DeleteData(
                table: "CredentialRequestSteps",
                keyColumn: "Id",
                keyValue: 10);

            migrationBuilder.DeleteData(
                table: "CredentialRequestSteps",
                keyColumn: "Id",
                keyValue: 14);

            migrationBuilder.DeleteData(
                table: "CredentialRequestSteps",
                keyColumn: "Id",
                keyValue: 15);

            migrationBuilder.DropColumn(
                name: "ErrorMessage",
                table: "CredentialRequests");
        }
    }
}
