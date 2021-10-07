using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace OpenCredentialPublisher.Data.Contexts.Migrations
{
    public partial class AugmentArtifactModel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "AssertionId",
                schema: "clr",
                table: "Artifacts",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ClrId",
                schema: "clr",
                table: "Artifacts",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "ClrIssuedOn",
                schema: "clr",
                table: "Artifacts",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ClrName",
                schema: "clr",
                table: "Artifacts",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "EvidenceName",
                schema: "clr",
                table: "Artifacts",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AssertionId",
                schema: "clr",
                table: "Artifacts");

            migrationBuilder.DropColumn(
                name: "ClrId",
                schema: "clr",
                table: "Artifacts");

            migrationBuilder.DropColumn(
                name: "ClrIssuedOn",
                schema: "clr",
                table: "Artifacts");

            migrationBuilder.DropColumn(
                name: "ClrName",
                schema: "clr",
                table: "Artifacts");

            migrationBuilder.DropColumn(
                name: "EvidenceName",
                schema: "clr",
                table: "Artifacts");
        }
    }
}
