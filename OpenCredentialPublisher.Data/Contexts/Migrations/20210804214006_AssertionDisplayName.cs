using Microsoft.EntityFrameworkCore.Migrations;

namespace OpenCredentialPublisher.Data.Contexts.Migrations
{
    public partial class AssertionDisplayName : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "DisplayName",
                schema: "cred",
                table: "Assertions",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.InsertData(
                table: "ProofRequestSteps",
                columns: new[] { "Id", "Name" },
                values: new object[] { 8, "ReceivingProofResponse" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "ProofRequestSteps",
                keyColumn: "Id",
                keyValue: 8);

            migrationBuilder.DropColumn(
                name: "DisplayName",
                schema: "cred",
                table: "Assertions");
        }
    }
}
