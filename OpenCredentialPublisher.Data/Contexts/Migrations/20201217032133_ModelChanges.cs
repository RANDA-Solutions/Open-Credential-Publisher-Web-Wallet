using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace OpenCredentialPublisher.Data.Contexts.Migrations
{
    public partial class ModelChanges : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "Created",
                table: "Authorizations",
                nullable: false,
                defaultValue: new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)));

            migrationBuilder.AddColumn<string>(
                name: "Endpoint",
                table: "Authorizations",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Method",
                table: "Authorizations",
                nullable: true);

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "Modified",
                table: "Authorizations",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Created",
                table: "Authorizations");

            migrationBuilder.DropColumn(
                name: "Endpoint",
                table: "Authorizations");

            migrationBuilder.DropColumn(
                name: "Method",
                table: "Authorizations");

            migrationBuilder.DropColumn(
                name: "Modified",
                table: "Authorizations");
        }
    }
}
