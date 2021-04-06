using Microsoft.EntityFrameworkCore.Migrations;

namespace OpenCredentialPublisher.Data.Contexts.Migrations
{
    public partial class BadgrIdentity : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BadgrAssertions_IdentityDType_RecipientIdentityKey",
                table: "BadgrAssertions");

            migrationBuilder.DropTable(
                name: "IdentityDType");

            migrationBuilder.DropIndex(
                name: "IX_BadgrAssertions_RecipientIdentityKey",
                table: "BadgrAssertions");

            migrationBuilder.DropColumn(
                name: "RecipientIdentityKey",
                table: "BadgrAssertions");

            migrationBuilder.AddColumn<string>(
                name: "RecipientIdentity",
                table: "BadgrAssertions",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "BadgrIdentityDType",
                columns: table => new
                {
                    Identity = table.Column<string>(nullable: false),
                    Type = table.Column<string>(nullable: false),
                    Hashed = table.Column<bool>(nullable: false),
                    PlainTextIdentity = table.Column<string>(nullable: false),
                    Salt = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BadgrIdentityDType", x => x.Identity);
                });

            migrationBuilder.CreateIndex(
                name: "IX_BadgrAssertions_RecipientIdentity",
                table: "BadgrAssertions",
                column: "RecipientIdentity");

            migrationBuilder.AddForeignKey(
                name: "FK_BadgrAssertions_BadgrIdentityDType_RecipientIdentity",
                table: "BadgrAssertions",
                column: "RecipientIdentity",
                principalTable: "BadgrIdentityDType",
                principalColumn: "Identity",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BadgrAssertions_BadgrIdentityDType_RecipientIdentity",
                table: "BadgrAssertions");

            migrationBuilder.DropTable(
                name: "BadgrIdentityDType");

            migrationBuilder.DropIndex(
                name: "IX_BadgrAssertions_RecipientIdentity",
                table: "BadgrAssertions");

            migrationBuilder.DropColumn(
                name: "RecipientIdentity",
                table: "BadgrAssertions");

            migrationBuilder.AddColumn<int>(
                name: "RecipientIdentityKey",
                table: "BadgrAssertions",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "IdentityDType",
                columns: table => new
                {
                    IdentityKey = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Hashed = table.Column<bool>(type: "bit", nullable: false),
                    Id = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Identity = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Salt = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Type = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_IdentityDType", x => x.IdentityKey);
                });

            migrationBuilder.CreateIndex(
                name: "IX_BadgrAssertions_RecipientIdentityKey",
                table: "BadgrAssertions",
                column: "RecipientIdentityKey");

            migrationBuilder.AddForeignKey(
                name: "FK_BadgrAssertions_IdentityDType_RecipientIdentityKey",
                table: "BadgrAssertions",
                column: "RecipientIdentityKey",
                principalTable: "IdentityDType",
                principalColumn: "IdentityKey",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
