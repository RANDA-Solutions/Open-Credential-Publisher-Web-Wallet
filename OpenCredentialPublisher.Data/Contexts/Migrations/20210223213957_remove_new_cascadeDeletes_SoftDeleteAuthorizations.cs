using Microsoft.EntityFrameworkCore.Migrations;

namespace OpenCredentialPublisher.Data.Contexts.Migrations
{
    public partial class remove_new_cascadeDeletes_SoftDeleteAuthorizations : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Shares_Links_LinkId",
                table: "Shares");

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "Authorizations",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddForeignKey(
                name: "FK_Shares_Links_LinkId",
                table: "Shares",
                column: "LinkId",
                principalTable: "Links",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Shares_Links_LinkId",
                table: "Shares");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "Authorizations");

            migrationBuilder.AddForeignKey(
                name: "FK_Shares_Links_LinkId",
                table: "Shares",
                column: "LinkId",
                principalTable: "Links",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
