using Microsoft.EntityFrameworkCore.Migrations;

namespace OpenCredentialPublisher.Data.Contexts.Migrations
{
    public partial class VerityThreads : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "VerityThreads",
                columns: table => new
                {
                    ThreadId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    FlowTypeId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VerityThreads", x => x.ThreadId);
                });

            migrationBuilder.InsertData(
                table: "ProofRequestSteps",
                columns: new[] { "Id", "Name" },
                values: new object[] { 6, "RequestedRelationship" });

            migrationBuilder.InsertData(
                table: "ProofRequestSteps",
                columns: new[] { "Id", "Name" },
                values: new object[] { 7, "CreatedRelationship" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "VerityThreads");

            migrationBuilder.DeleteData(
                table: "ProofRequestSteps",
                keyColumn: "Id",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "ProofRequestSteps",
                keyColumn: "Id",
                keyValue: 7);
        }
    }
}
