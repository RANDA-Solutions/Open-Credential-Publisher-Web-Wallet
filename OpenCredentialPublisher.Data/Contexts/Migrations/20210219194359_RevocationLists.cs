using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace OpenCredentialPublisher.Data.Contexts.Migrations
{
    public partial class RevocationLists : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_VerifiableCredentials_CredentialPackages_CredentialPackageId",
                table: "VerifiableCredentials");

            migrationBuilder.CreateTable(
                name: "Revocations",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(nullable: false),
                    SourceId = table.Column<int>(nullable: false),
                    IssuerId = table.Column<string>(nullable: true),
                    RevokedId = table.Column<string>(nullable: true),
                    DateCreated = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Revocations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Revocations_Sources_SourceId",
                        column: x => x.SourceId,
                        principalTable: "Sources",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Revocations_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Revocations_SourceId",
                table: "Revocations",
                column: "SourceId");

            migrationBuilder.CreateIndex(
                name: "IX_Revocations_UserId",
                table: "Revocations",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_VerifiableCredentials_CredentialPackages_CredentialPackageId",
                table: "VerifiableCredentials",
                column: "CredentialPackageId",
                principalTable: "CredentialPackages",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_VerifiableCredentials_CredentialPackages_CredentialPackageId",
                table: "VerifiableCredentials");

            migrationBuilder.DropTable(
                name: "Revocations");

            migrationBuilder.AddForeignKey(
                name: "FK_VerifiableCredentials_CredentialPackages_CredentialPackageId",
                table: "VerifiableCredentials",
                column: "CredentialPackageId",
                principalTable: "CredentialPackages",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
