using Microsoft.EntityFrameworkCore.Migrations;

namespace OpenCredentialPublisher.Data.Contexts.Migrations
{
    public partial class OCP_Idatafy : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SmartResumes_AspNetUsers_UserId",
                schema: "idatafy",
                table: "SmartResumes");

            migrationBuilder.DropColumn(
                name: "IsReady",
                schema: "idatafy",
                table: "SmartResumes");

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                schema: "idatafy",
                table: "SmartResumes",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Message",
                schema: "idatafy",
                table: "SmartResumes",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "StatusId",
                schema: "idatafy",
                table: "SmartResumes",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "StatusMessage",
                schema: "idatafy",
                table: "SmartResumes",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_SmartResumes_AspNetUsers_UserId",
                schema: "idatafy",
                table: "SmartResumes",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SmartResumes_AspNetUsers_UserId",
                schema: "idatafy",
                table: "SmartResumes");

            migrationBuilder.DropColumn(
                name: "Message",
                schema: "idatafy",
                table: "SmartResumes");

            migrationBuilder.DropColumn(
                name: "StatusId",
                schema: "idatafy",
                table: "SmartResumes");

            migrationBuilder.DropColumn(
                name: "StatusMessage",
                schema: "idatafy",
                table: "SmartResumes");

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                schema: "idatafy",
                table: "SmartResumes",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AddColumn<bool>(
                name: "IsReady",
                schema: "idatafy",
                table: "SmartResumes",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddForeignKey(
                name: "FK_SmartResumes_AspNetUsers_UserId",
                schema: "idatafy",
                table: "SmartResumes",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
