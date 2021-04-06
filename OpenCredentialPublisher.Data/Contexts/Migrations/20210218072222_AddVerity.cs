using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace OpenCredentialPublisher.Data.Contexts.Migrations
{
    public partial class AddVerity : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AgentContexts",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    AgentName = table.Column<string>(nullable: true),
                    Network = table.Column<string>(nullable: true),
                    TokenHash = table.Column<string>(nullable: true),
                    ContextJson = table.Column<string>(nullable: true),
                    IssuerDid = table.Column<string>(nullable: true),
                    IssuerVerKey = table.Column<string>(nullable: true),
                    IssuerRegistered = table.Column<bool>(nullable: false),
                    Active = table.Column<bool>(nullable: false),
                    CreatedOn = table.Column<DateTimeOffset>(nullable: false),
                    ModifiedOn = table.Column<DateTimeOffset>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AgentContexts", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "WalletRequestSteps",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false),
                    Name = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WalletRequestSteps", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "WalletRequestTypes",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false),
                    Name = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WalletRequestTypes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CredentialSchema",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SchemaId = table.Column<string>(nullable: true),
                    AgentContextId = table.Column<Guid>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    Version = table.Column<string>(nullable: true),
                    Hash = table.Column<string>(nullable: true),
                    StatusId = table.Column<int>(nullable: false),
                    CreatedOn = table.Column<DateTimeOffset>(nullable: false),
                    ModifiedOn = table.Column<DateTimeOffset>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CredentialSchema", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CredentialSchema_AgentContexts_AgentContextId",
                        column: x => x.AgentContextId,
                        principalTable: "AgentContexts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "WalletRelationships",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RelationshipDid = table.Column<string>(nullable: true),
                    RelationshipVerKey = table.Column<string>(nullable: true),
                    InviteUrl = table.Column<string>(nullable: true),
                    IsConnected = table.Column<bool>(nullable: false),
                    AgentContextId = table.Column<Guid>(nullable: false),
                    UserId = table.Column<string>(nullable: false),
                    CreatedOn = table.Column<DateTimeOffset>(nullable: false),
                    ModifiedOn = table.Column<DateTimeOffset>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WalletRelationships", x => x.Id);
                    table.ForeignKey(
                        name: "FK_WalletRelationships_AgentContexts_AgentContextId",
                        column: x => x.AgentContextId,
                        principalTable: "AgentContexts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_WalletRelationships_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CredentialDefinitions",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CredentialSchemaId = table.Column<int>(nullable: false),
                    CredentialDefinitionId = table.Column<string>(nullable: true),
                    Name = table.Column<string>(nullable: true),
                    Tag = table.Column<string>(nullable: true),
                    StatusId = table.Column<int>(nullable: false),
                    CreatedOn = table.Column<DateTimeOffset>(nullable: false),
                    ModifiedOn = table.Column<DateTimeOffset>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CredentialDefinitions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CredentialDefinitions_CredentialSchema_CredentialSchemaId",
                        column: x => x.CredentialSchemaId,
                        principalTable: "CredentialSchema",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "WalletRequests",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AgentContextId = table.Column<Guid>(nullable: false),
                    ThreadId = table.Column<string>(nullable: true),
                    InviteUrl = table.Column<string>(nullable: true),
                    UserId = table.Column<string>(nullable: false),
                    WalletRequestType = table.Column<int>(nullable: false),
                    WalletRequestStep = table.Column<int>(nullable: false),
                    CredentialPackageId = table.Column<int>(nullable: true),
                    WalletRelationshipId = table.Column<int>(nullable: true),
                    CredentialDefinitionId = table.Column<int>(nullable: true),
                    CredentialSchemaId = table.Column<int>(nullable: true),
                    CreatedOn = table.Column<DateTimeOffset>(nullable: false),
                    ModifiedOn = table.Column<DateTimeOffset>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WalletRequests", x => x.Id);
                    table.ForeignKey(
                        name: "FK_WalletRequests_AgentContexts_AgentContextId",
                        column: x => x.AgentContextId,
                        principalTable: "AgentContexts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_WalletRequests_CredentialDefinitions_CredentialDefinitionId",
                        column: x => x.CredentialDefinitionId,
                        principalTable: "CredentialDefinitions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_WalletRequests_CredentialPackages_CredentialPackageId",
                        column: x => x.CredentialPackageId,
                        principalTable: "CredentialPackages",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_WalletRequests_CredentialSchema_CredentialSchemaId",
                        column: x => x.CredentialSchemaId,
                        principalTable: "CredentialSchema",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_WalletRequests_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_WalletRequests_WalletRelationships_WalletRelationshipId",
                        column: x => x.WalletRelationshipId,
                        principalTable: "WalletRelationships",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.InsertData(
                table: "WalletRequestSteps",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 0, "Initiated" },
                    { 1, "PendingAgent" },
                    { 2, "StartingInvitation" },
                    { 3, "RequestingInvitation" },
                    { 4, "InvitationGenerated" },
                    { 5, "InvitationAccepted" },
                    { 6, "InvitationCompleted" },
                    { 7, "CreatingSchema" },
                    { 8, "CreatingCredentialDefinition" },
                    { 9, "SendingOffer" },
                    { 10, "OfferAccepted" },
                    { 13, "Error" }
                });

            migrationBuilder.InsertData(
                table: "WalletRequestTypes",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 1, "Connection" },
                    { 2, "Credential" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_CredentialDefinitions_CredentialSchemaId",
                table: "CredentialDefinitions",
                column: "CredentialSchemaId");

            migrationBuilder.CreateIndex(
                name: "IX_CredentialSchema_AgentContextId",
                table: "CredentialSchema",
                column: "AgentContextId");

            migrationBuilder.CreateIndex(
                name: "IX_WalletRelationships_AgentContextId",
                table: "WalletRelationships",
                column: "AgentContextId");

            migrationBuilder.CreateIndex(
                name: "IX_WalletRelationships_UserId",
                table: "WalletRelationships",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_WalletRequests_AgentContextId",
                table: "WalletRequests",
                column: "AgentContextId");

            migrationBuilder.CreateIndex(
                name: "IX_WalletRequests_CredentialDefinitionId",
                table: "WalletRequests",
                column: "CredentialDefinitionId");

            migrationBuilder.CreateIndex(
                name: "IX_WalletRequests_CredentialPackageId",
                table: "WalletRequests",
                column: "CredentialPackageId");

            migrationBuilder.CreateIndex(
                name: "IX_WalletRequests_CredentialSchemaId",
                table: "WalletRequests",
                column: "CredentialSchemaId");

            migrationBuilder.CreateIndex(
                name: "IX_WalletRequests_ThreadId",
                table: "WalletRequests",
                column: "ThreadId",
                unique: true,
                filter: "[ThreadId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_WalletRequests_UserId",
                table: "WalletRequests",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_WalletRequests_WalletRelationshipId",
                table: "WalletRequests",
                column: "WalletRelationshipId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "WalletRequests");

            migrationBuilder.DropTable(
                name: "WalletRequestSteps");

            migrationBuilder.DropTable(
                name: "WalletRequestTypes");

            migrationBuilder.DropTable(
                name: "CredentialDefinitions");

            migrationBuilder.DropTable(
                name: "WalletRelationships");

            migrationBuilder.DropTable(
                name: "CredentialSchema");

            migrationBuilder.DropTable(
                name: "AgentContexts");
        }
    }
}
