using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace OpenCredentialPublisher.Data.Contexts.Migrations
{
    public partial class MoreSharing : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "Links",
                nullable: true,
                defaultValue: "");

            migrationBuilder.Sql(@"
                Update UL
                set UL.UserId = AU.Id
                from dbo.Links UL
                join dbo.AspnetUsers AU on UL.Username = AU.Username
            ");

            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "CredentialPackages",
                nullable: true,
                defaultValue: "");

            migrationBuilder.Sql(@"
                Update UL
                set UL.UserId = AU.Id
                from dbo.CredentialPackages UL
                join dbo.AspnetUsers AU on UL.Username = AU.Username
            ");

            migrationBuilder.AlterColumn<string>(name: "UserId", table: "Links", nullable: false);
            migrationBuilder.AlterColumn<string>(name: "UserId", table: "CredentialPackages", nullable: false);

            migrationBuilder.DropColumn(
                name: "UserName",
                table: "Links");

            migrationBuilder.DropColumn(
                name: "UserName",
                table: "CredentialPackages");

            migrationBuilder.AddColumn<bool>(
                name: "RequiresAccessKey",
                table: "Links",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateTable(
                name: "Recipients",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    Email = table.Column<string>(nullable: true),
                    CreatedOn = table.Column<DateTimeOffset>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Recipients", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Recipients_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Statuses",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false),
                    Name = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Statuses", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Shares",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    LinkId = table.Column<string>(nullable: true),
                    RecipientId = table.Column<int>(nullable: false),
                    AccessKey = table.Column<string>(nullable: true),
                    UseCount = table.Column<int>(nullable: false),
                    CreatedOn = table.Column<DateTimeOffset>(nullable: false),
                    StatusId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Shares", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Shares_Links_LinkId",
                        column: x => x.LinkId,
                        principalTable: "Links",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Shares_Recipients_RecipientId",
                        column: x => x.RecipientId,
                        principalTable: "Recipients",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Messages",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Recipient = table.Column<string>(nullable: true),
                    Body = table.Column<string>(nullable: true),
                    Subject = table.Column<string>(nullable: true),
                    SendAttempts = table.Column<int>(nullable: false),
                    StatusId = table.Column<int>(nullable: false),
                    CreatedOn = table.Column<DateTimeOffset>(nullable: false),
                    ShareId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Messages", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Messages_Shares_ShareId",
                        column: x => x.ShareId,
                        principalTable: "Shares",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.InsertData(
                table: "Statuses",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 1, "Pending" },
                    { 2, "Accepted" },
                    { 3, "Used" },
                    { 4, "Expired" },
                    { 5, "Rejected" },
                    { 6, "Created" },
                    { 7, "Deleted" },
                    { 8, "Visible" },
                    { 9, "Hidden" },
                    { 10, "Submitted" },
                    { 11, "Active" },
                    { 12, "Sent" },
                    { 13, "Error" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Links_UserId",
                table: "Links",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_CredentialPackages_UserId",
                table: "CredentialPackages",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Messages_ShareId",
                table: "Messages",
                column: "ShareId");

            migrationBuilder.CreateIndex(
                name: "IX_Recipients_UserId",
                table: "Recipients",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Shares_LinkId",
                table: "Shares",
                column: "LinkId");

            migrationBuilder.CreateIndex(
                name: "IX_Shares_RecipientId",
                table: "Shares",
                column: "RecipientId");

            migrationBuilder.AddForeignKey(
                name: "FK_CredentialPackages_AspNetUsers_UserId",
                table: "CredentialPackages",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Links_AspNetUsers_UserId",
                table: "Links",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CredentialPackages_AspNetUsers_UserId",
                table: "CredentialPackages");

            migrationBuilder.DropForeignKey(
                name: "FK_Links_AspNetUsers_UserId",
                table: "Links");

            migrationBuilder.DropTable(
                name: "Messages");

            migrationBuilder.DropTable(
                name: "Statuses");

            migrationBuilder.DropTable(
                name: "Shares");

            migrationBuilder.DropTable(
                name: "Recipients");

            migrationBuilder.DropIndex(
                name: "IX_Links_UserId",
                table: "Links");

            migrationBuilder.DropIndex(
                name: "IX_CredentialPackages_UserId",
                table: "CredentialPackages");

            migrationBuilder.DropColumn(
                name: "RequiresAccessKey",
                table: "Links");

            migrationBuilder.AddColumn<string>(
                name: "UserName",
                table: "Links",
                type: "nvarchar(max)",
                nullable: true,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "UserName",
                table: "CredentialPackages",
                type: "nvarchar(max)",
                nullable: true,
                defaultValue: "");

            migrationBuilder.Sql(@"
                Update UL
                set UL.Username = AU.Username
                from dbo.Links UL
                join dbo.AspnetUsers AU on UL.UserId = AU.Id
            ");

            migrationBuilder.Sql(@"
                Update UL
                set UL.Username = AU.Username
                from dbo.CredentialPackages UL
                join dbo.AspnetUsers AU on UL.UserId = AU.Id
            ");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Links");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "CredentialPackages");

            migrationBuilder.AlterColumn<string>(name: "Username", table: "Links", nullable: false);
            migrationBuilder.AlterColumn<string>(name: "Username", table: "CredentialPackages", nullable: false);
        }
    }
}
