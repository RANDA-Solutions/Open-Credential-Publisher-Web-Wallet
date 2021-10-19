using Microsoft.EntityFrameworkCore.Migrations;

namespace OpenCredentialPublisher.Data.Contexts.Migrations
{
    public partial class NullFix1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Clrs_Verifications_VerificationId",
                schema: "cred",
                table: "Clrs");

            migrationBuilder.DropIndex(
                name: "IX_Clrs_VerificationId",
                schema: "cred",
                table: "Clrs");

            migrationBuilder.AlterColumn<int>(
                name: "VerificationId",
                schema: "cred",
                table: "Clrs",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.CreateIndex(
                name: "IX_Clrs_VerificationId",
                schema: "cred",
                table: "Clrs",
                column: "VerificationId",
                unique: true,
                filter: "[VerificationId] IS NOT NULL");

            migrationBuilder.AddForeignKey(
                name: "FK_Clrs_Verifications_VerificationId",
                schema: "cred",
                table: "Clrs",
                column: "VerificationId",
                principalSchema: "cred",
                principalTable: "Verifications",
                principalColumn: "VerificationId",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Clrs_Verifications_VerificationId",
                schema: "cred",
                table: "Clrs");

            migrationBuilder.DropIndex(
                name: "IX_Clrs_VerificationId",
                schema: "cred",
                table: "Clrs");

            migrationBuilder.AlterColumn<int>(
                name: "VerificationId",
                schema: "cred",
                table: "Clrs",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Clrs_VerificationId",
                schema: "cred",
                table: "Clrs",
                column: "VerificationId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Clrs_Verifications_VerificationId",
                schema: "cred",
                table: "Clrs",
                column: "VerificationId",
                principalSchema: "cred",
                principalTable: "Verifications",
                principalColumn: "VerificationId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
