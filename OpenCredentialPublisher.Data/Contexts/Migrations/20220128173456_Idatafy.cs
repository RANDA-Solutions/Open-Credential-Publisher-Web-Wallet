using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace OpenCredentialPublisher.Data.Contexts.Migrations
{
    public partial class Idatafy : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "idatafy");

            migrationBuilder.AddColumn<int>(
                name: "SmartResumeId",
                schema: "cred",
                table: "Clrs",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "SmartResumes",
                schema: "idatafy",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    ClrId = table.Column<int>(type: "int", nullable: false),
                    SmartResumeUrl = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsReady = table.Column<bool>(type: "bit", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SmartResumes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SmartResumes_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Clrs_SmartResumeId",
                schema: "cred",
                table: "Clrs",
                column: "SmartResumeId",
                unique: true,
                filter: "[SmartResumeId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_SmartResumes_UserId",
                schema: "idatafy",
                table: "SmartResumes",
                column: "UserId");

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

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Clrs_SmartResumes_SmartResumeId",
                schema: "cred",
                table: "Clrs");

            migrationBuilder.DropTable(
                name: "SmartResumes",
                schema: "idatafy");

            migrationBuilder.DropIndex(
                name: "IX_Clrs_SmartResumeId",
                schema: "cred",
                table: "Clrs");

            migrationBuilder.DropColumn(
                name: "SmartResumeId",
                schema: "cred",
                table: "Clrs");
        }
    }
}
