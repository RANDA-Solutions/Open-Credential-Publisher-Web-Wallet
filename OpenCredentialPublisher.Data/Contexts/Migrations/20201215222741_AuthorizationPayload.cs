using Microsoft.EntityFrameworkCore.Migrations;

namespace OpenCredentialPublisher.Data.Contexts.Migrations
{
    public partial class AuthorizationPayload : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Payload",
                table: "Authorizations",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "Authorizations",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Authorizations_UserId",
                table: "Authorizations",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Authorizations_AspNetUsers_UserId",
                table: "Authorizations",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Authorizations_AspNetUsers_UserId",
                table: "Authorizations");

            migrationBuilder.DropIndex(
                name: "IX_Authorizations_UserId",
                table: "Authorizations");

            migrationBuilder.DropColumn(
                name: "Payload",
                table: "Authorizations");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Authorizations");
        }
    }
}
