using Microsoft.EntityFrameworkCore.Migrations;

namespace OpenCredentialPublisher.Data.Contexts.Migrations
{
    public partial class AngularDoubleCheck : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddForeignKey(
                name: "FK_Clrs_CredentialPackages_ParentCredentialPackageId",
                table: "Clrs",
                column: "ParentCredentialPackageId",
                principalTable: "CredentialPackages",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Clrs_CredentialPackages_ParentCredentialPackageId",
                table: "Clrs");
        }
    }
}
