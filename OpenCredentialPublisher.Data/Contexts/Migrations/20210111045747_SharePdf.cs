using Microsoft.EntityFrameworkCore.Migrations;

namespace OpenCredentialPublisher.Data.Contexts.Migrations
{
    public partial class SharePdf : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Shares_Recipients_RecipientId",
                table: "Shares");

            migrationBuilder.AlterColumn<int>(
                name: "RecipientId",
                table: "Shares",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<int>(
                name: "ShareTypeId",
                table: "Shares",
                nullable: false,
                defaultValue: 1);

            migrationBuilder.CreateTable(
                name: "ShareTypes",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false),
                    Name = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ShareTypes", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "ShareTypes",
                columns: new[] { "Id", "Name" },
                values: new object[] { 1, "Email" });

            migrationBuilder.InsertData(
                table: "ShareTypes",
                columns: new[] { "Id", "Name" },
                values: new object[] { 2, "Pdf" });

            migrationBuilder.AddForeignKey(
                name: "FK_Shares_Recipients_RecipientId",
                table: "Shares",
                column: "RecipientId",
                principalTable: "Recipients",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Shares_Recipients_RecipientId",
                table: "Shares");

            migrationBuilder.DropTable(
                name: "ShareTypes");

            migrationBuilder.DropColumn(
                name: "ShareTypeId",
                table: "Shares");

            migrationBuilder.AlterColumn<int>(
                name: "RecipientId",
                table: "Shares",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Shares_Recipients_RecipientId",
                table: "Shares",
                column: "RecipientId",
                principalTable: "Recipients",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
