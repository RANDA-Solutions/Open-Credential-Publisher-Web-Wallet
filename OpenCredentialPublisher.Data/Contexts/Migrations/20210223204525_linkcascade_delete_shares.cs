using Microsoft.EntityFrameworkCore.Migrations;

namespace OpenCredentialPublisher.Data.Contexts.Migrations
{
    public partial class linkcascade_delete_shares : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Shares_Links_LinkId",
                table: "Shares");

            migrationBuilder.AddForeignKey(
                name: "FK_Shares_Links_LinkId",
                table: "Shares",
                column: "LinkId",
                principalTable: "Links",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Shares_Links_LinkId",
                table: "Shares");

            migrationBuilder.AddForeignKey(
                name: "FK_Shares_Links_LinkId",
                table: "Shares",
                column: "LinkId",
                principalTable: "Links",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
