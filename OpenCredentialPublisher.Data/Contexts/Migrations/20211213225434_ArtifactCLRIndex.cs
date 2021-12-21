using Microsoft.EntityFrameworkCore.Migrations;

namespace OpenCredentialPublisher.Data.Contexts.Migrations
{
    public partial class ArtifactCLRIndex : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Artifacts_ClrId",
                schema: "cred",
                table: "Artifacts",
                column: "ClrId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Artifacts_ClrId",
                schema: "cred",
                table: "Artifacts");
        }
    }
}
