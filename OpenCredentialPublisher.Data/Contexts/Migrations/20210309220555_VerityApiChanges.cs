using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace OpenCredentialPublisher.Data.Contexts.Migrations
{
    public partial class VerityApiChanges : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CredentialSchema_AgentContexts_AgentContextId",
                table: "CredentialSchema");

            migrationBuilder.DropIndex(
                name: "IX_CredentialSchema_AgentContextId",
                table: "CredentialSchema");

            migrationBuilder.AddColumn<Guid>(
                name: "AgentContextId",
                table: "CredentialDefinitions",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.Sql(@"Update CD
                set CD.AgentContextId = CS.AgentContextId
                FROM dbo.CredentialDefinitions CD
                JOIN dbo.CredentialSchema CS on CD.CredentialSchemaId = CS.Id
            ");

            migrationBuilder.DropColumn(
                name: "AgentContextId",
                table: "CredentialSchema");

            migrationBuilder.AddColumn<string>(
                name: "ApiKey",
                table: "AgentContexts",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DomainDid",
                table: "AgentContexts",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "EndpointUrl",
                table: "AgentContexts",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ProvisioningTokenId",
                table: "AgentContexts",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SdkVerKey",
                table: "AgentContexts",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SdkVerKeyId",
                table: "AgentContexts",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ThreadId",
                table: "AgentContexts",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "VerityAgentVerKey",
                table: "AgentContexts",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "VerityPublicDid",
                table: "AgentContexts",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "VerityPublicVerKey",
                table: "AgentContexts",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "VerityUrl",
                table: "AgentContexts",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Version",
                table: "AgentContexts",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "WalletKey",
                table: "AgentContexts",
                nullable: true);

            migrationBuilder.Sql(
                @"Update dbo.AgentContexts set WalletKey = [Key]");

            migrationBuilder.DropColumn(
                name: "Key",
                table: "AgentContexts");

            migrationBuilder.AddColumn<string>(
                name: "WalletPath",
                table: "AgentContexts",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "ProvisioningToken",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SponseeId = table.Column<string>(nullable: true),
                    SponsorId = table.Column<string>(nullable: true),
                    Nonce = table.Column<string>(nullable: true),
                    Timestamp = table.Column<string>(nullable: true),
                    Sig = table.Column<string>(nullable: true),
                    SponsorVerKey = table.Column<string>(nullable: true),
                    CreatedOn = table.Column<DateTimeOffset>(nullable: false),
                    ModifiedOn = table.Column<DateTimeOffset>(nullable: true),
                    AgentContextId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProvisioningToken", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProvisioningToken_AgentContexts_AgentContextId",
                        column: x => x.AgentContextId,
                        principalTable: "AgentContexts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CredentialDefinitions_AgentContextId",
                table: "CredentialDefinitions",
                column: "AgentContextId");

            migrationBuilder.CreateIndex(
                name: "IX_ProvisioningToken_AgentContextId",
                table: "ProvisioningToken",
                column: "AgentContextId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_CredentialDefinitions_AgentContexts_AgentContextId",
                table: "CredentialDefinitions",
                column: "AgentContextId",
                principalTable: "AgentContexts",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CredentialDefinitions_AgentContexts_AgentContextId",
                table: "CredentialDefinitions");

            migrationBuilder.DropTable(
                name: "ProvisioningToken");

            migrationBuilder.DropIndex(
                name: "IX_CredentialDefinitions_AgentContextId",
                table: "CredentialDefinitions");

            migrationBuilder.DropColumn(
                name: "AgentContextId",
                table: "CredentialDefinitions");

            migrationBuilder.DropColumn(
                name: "ApiKey",
                table: "AgentContexts");

            migrationBuilder.DropColumn(
                name: "DomainDid",
                table: "AgentContexts");

            migrationBuilder.DropColumn(
                name: "EndpointUrl",
                table: "AgentContexts");

            migrationBuilder.DropColumn(
                name: "ProvisioningTokenId",
                table: "AgentContexts");

            migrationBuilder.DropColumn(
                name: "SdkVerKey",
                table: "AgentContexts");

            migrationBuilder.DropColumn(
                name: "SdkVerKeyId",
                table: "AgentContexts");

            migrationBuilder.DropColumn(
                name: "ThreadId",
                table: "AgentContexts");

            migrationBuilder.DropColumn(
                name: "VerityAgentVerKey",
                table: "AgentContexts");

            migrationBuilder.DropColumn(
                name: "VerityPublicDid",
                table: "AgentContexts");

            migrationBuilder.DropColumn(
                name: "VerityPublicVerKey",
                table: "AgentContexts");

            migrationBuilder.DropColumn(
                name: "VerityUrl",
                table: "AgentContexts");

            migrationBuilder.DropColumn(
                name: "Version",
                table: "AgentContexts");

            migrationBuilder.DropColumn(
                name: "WalletKey",
                table: "AgentContexts");

            migrationBuilder.DropColumn(
                name: "WalletPath",
                table: "AgentContexts");

            migrationBuilder.AddColumn<Guid>(
                name: "AgentContextId",
                table: "CredentialSchema",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<string>(
                name: "Key",
                table: "AgentContexts",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_CredentialSchema_AgentContextId",
                table: "CredentialSchema",
                column: "AgentContextId");

            migrationBuilder.AddForeignKey(
                name: "FK_CredentialSchema_AgentContexts_AgentContextId",
                table: "CredentialSchema",
                column: "AgentContextId",
                principalTable: "AgentContexts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
