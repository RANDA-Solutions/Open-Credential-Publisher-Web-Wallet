using Microsoft.EntityFrameworkCore.Migrations;

namespace OpenCredentialPublisher.Data.Contexts.Migrations
{
    public partial class FixVCBadFKColumn : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ClrSets_VerifiableCredentials_VParenterifiableCredentialId",
                table: "ClrSets");

            migrationBuilder.DropIndex(
                name: "IX_ClrSets_VParenterifiableCredentialId",
                table: "ClrSets");

            migrationBuilder.DropColumn(
                name: "ParentVerifiableCredentialId",
                table: "ClrSets");

            migrationBuilder.RenameColumn(
                name: "VParenterifiableCredentialId",
                table: "ClrSets",
                newName: "ParentVerifiableCredentialId");

            migrationBuilder.CreateIndex(
                name: "IX_ClrSets_ParentVerifiableCredentialId",
                table: "ClrSets",
                column: "ParentVerifiableCredentialId",
                filter: "[ParentVerifiableCredentialId] IS NOT NULL");

            migrationBuilder.AddForeignKey(
                name: "FK_ClrSets_VerifiableCredentials_ParentVerifiableCredentialId",
                table: "ClrSets",
                column: "ParentVerifiableCredentialId",
                principalTable: "VerifiableCredentials",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ClrSets_VerifiableCredentials_ParentVerifiableCredentialId",
                table: "ClrSets");

            migrationBuilder.DropIndex(
                name: "IX_ClrSets_ParentVerifiableCredentialId",
                table: "ClrSets");

            migrationBuilder.RenameColumn(
                name: "ParentVerifiableCredentialId",
                table: "ClrSets",
                newName: "VParenterifiableCredentialId");

            migrationBuilder.AddColumn<int>(
                name: "ParentVerifiableCredentialId",
                table: "ClrSets",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_ClrSets_VParenterifiableCredentialId",
                table: "ClrSets",
                column: "VParenterifiableCredentialId");
            migrationBuilder.AddForeignKey(
                name: "FK_ClrSets_VerifiableCredentials_VParenterifiableCredentialId",
                table: "ClrSets",
                column: "ParentVerifiableCredentialId",
                principalTable: "VerifiableCredentials",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

        }
    }
}
