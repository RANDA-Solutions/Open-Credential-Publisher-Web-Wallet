using Microsoft.EntityFrameworkCore.Migrations;

namespace OpenCredentialPublisher.Data.Contexts.Migrations
{
    public partial class ClrArtifact : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "CredentialRequestSteps",
                columns: new[] { "Id", "Name" },
                values: new object[] { 11, "PendingSchemaEndorsement" });

            migrationBuilder.InsertData(
                table: "CredentialRequestSteps",
                columns: new[] { "Id", "Name" },
                values: new object[] { 12, "PendingCredentialDefinitionEndorsement" });

            migrationBuilder.InsertData(
                table: "Statuses",
                columns: new[] { "Id", "Name" },
                values: new object[] { 19, "NeedsEndorsement" });

            migrationBuilder.AddForeignKey(
                name: "FK_Artifacts_Clrs_ClrId",
                schema: "cred",
                table: "Artifacts",
                column: "ClrId",
                principalSchema: "cred",
                principalTable: "Clrs",
                principalColumn: "ClrId",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Artifacts_Clrs_ClrId",
                schema: "cred",
                table: "Artifacts");

            migrationBuilder.DeleteData(
                table: "CredentialRequestSteps",
                keyColumn: "Id",
                keyValue: 11);

            migrationBuilder.DeleteData(
                table: "CredentialRequestSteps",
                keyColumn: "Id",
                keyValue: 12);

            migrationBuilder.DeleteData(
                table: "Statuses",
                keyColumn: "Id",
                keyValue: 19);
        }
    }
}
