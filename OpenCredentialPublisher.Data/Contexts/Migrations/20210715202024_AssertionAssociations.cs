using Microsoft.EntityFrameworkCore.Migrations;

namespace OpenCredentialPublisher.Data.Contexts.Migrations
{
    public partial class AssertionAssociations : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsSelfPublished",
                schema: "cred",
                table: "Assertions",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "ParentAssertionId",
                schema: "cred",
                table: "Assertions",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Assertions_ParentAssertionId",
                schema: "cred",
                table: "Assertions",
                column: "ParentAssertionId");

            migrationBuilder.AddForeignKey(
                name: "FK_Assertions_Assertions_ParentAssertionId",
                schema: "cred",
                table: "Assertions",
                column: "ParentAssertionId",
                principalSchema: "cred",
                principalTable: "Assertions",
                principalColumn: "AssertionId",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Assertions_Assertions_ParentAssertionId",
                schema: "cred",
                table: "Assertions");

            migrationBuilder.DropIndex(
                name: "IX_Assertions_ParentAssertionId",
                schema: "cred",
                table: "Assertions");

            migrationBuilder.DropColumn(
                name: "IsSelfPublished",
                schema: "cred",
                table: "Assertions");

            migrationBuilder.DropColumn(
                name: "ParentAssertionId",
                schema: "cred",
                table: "Assertions");
        }
    }
}
