using Microsoft.EntityFrameworkCore.Migrations;

namespace OpenCredentialPublisher.Data.Contexts.Migrations
{
    public partial class UpdateToRevocationModel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Revocations_Sources_SourceId",
                table: "Revocations");

            migrationBuilder.AlterColumn<int>(
                name: "SourceId",
                table: "Revocations",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<string>(
                name: "RevocationListId",
                table: "Revocations",
                nullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Revocations_Sources_SourceId",
                table: "Revocations",
                column: "SourceId",
                principalTable: "Sources",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Revocations_Sources_SourceId",
                table: "Revocations");

            migrationBuilder.DropColumn(
                name: "RevocationListId",
                table: "Revocations");

            migrationBuilder.AlterColumn<int>(
                name: "SourceId",
                table: "Revocations",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Revocations_Sources_SourceId",
                table: "Revocations",
                column: "SourceId",
                principalTable: "Sources",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
