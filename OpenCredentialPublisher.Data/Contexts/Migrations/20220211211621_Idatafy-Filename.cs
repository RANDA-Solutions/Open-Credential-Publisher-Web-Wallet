using Microsoft.EntityFrameworkCore.Migrations;

namespace OpenCredentialPublisher.Data.Contexts.Migrations
{
    public partial class IdatafyFilename : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "UploadName",
                schema: "idatafy",
                table: "SmartResumes",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UploadName",
                schema: "idatafy",
                table: "SmartResumes");
        }
    }
}
