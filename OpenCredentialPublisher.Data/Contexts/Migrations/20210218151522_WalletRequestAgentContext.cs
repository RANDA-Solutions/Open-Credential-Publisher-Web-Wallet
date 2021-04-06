using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace OpenCredentialPublisher.Data.Contexts.Migrations
{
    public partial class WalletRequestAgentContext : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_WalletRequests_AgentContexts_AgentContextId",
                table: "WalletRequests");

            migrationBuilder.AlterColumn<Guid>(
                name: "AgentContextId",
                table: "WalletRequests",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AddForeignKey(
                name: "FK_WalletRequests_AgentContexts_AgentContextId",
                table: "WalletRequests",
                column: "AgentContextId",
                principalTable: "AgentContexts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_WalletRequests_AgentContexts_AgentContextId",
                table: "WalletRequests");

            migrationBuilder.AlterColumn<Guid>(
                name: "AgentContextId",
                table: "WalletRequests",
                type: "uniqueidentifier",
                nullable: false,
                oldClrType: typeof(Guid),
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_WalletRequests_AgentContexts_AgentContextId",
                table: "WalletRequests",
                column: "AgentContextId",
                principalTable: "AgentContexts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
