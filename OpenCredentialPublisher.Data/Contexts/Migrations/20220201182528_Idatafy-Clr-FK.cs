using Microsoft.EntityFrameworkCore.Migrations;

namespace OpenCredentialPublisher.Data.Contexts.Migrations
{
    public partial class IdatafyClrFK : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Clrs_SmartResumes_SmartResumeId",
                schema: "cred",
                table: "Clrs");

            migrationBuilder.DropIndex(
                name: "IX_Clrs_SmartResumeId",
                schema: "cred",
                table: "Clrs");

            migrationBuilder.DropColumn(
                name: "SmartResumeId",
                schema: "cred",
                table: "Clrs");

            migrationBuilder.CreateIndex(
                name: "IX_SmartResumes_ClrId",
                schema: "idatafy",
                table: "SmartResumes",
                column: "ClrId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_SmartResumes_Clrs_ClrId",
                schema: "idatafy",
                table: "SmartResumes",
                column: "ClrId",
                principalSchema: "cred",
                principalTable: "Clrs",
                principalColumn: "ClrId",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SmartResumes_Clrs_ClrId",
                schema: "idatafy",
                table: "SmartResumes");

            migrationBuilder.DropIndex(
                name: "IX_SmartResumes_ClrId",
                schema: "idatafy",
                table: "SmartResumes");

            migrationBuilder.AddColumn<int>(
                name: "SmartResumeId",
                schema: "cred",
                table: "Clrs",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Clrs_SmartResumeId",
                schema: "cred",
                table: "Clrs",
                column: "SmartResumeId",
                unique: true,
                filter: "[SmartResumeId] IS NOT NULL");

            migrationBuilder.AddForeignKey(
                name: "FK_Clrs_SmartResumes_SmartResumeId",
                schema: "cred",
                table: "Clrs",
                column: "SmartResumeId",
                principalSchema: "idatafy",
                principalTable: "SmartResumes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
