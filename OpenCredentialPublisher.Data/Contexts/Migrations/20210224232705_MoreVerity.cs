using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace OpenCredentialPublisher.Data.Contexts.Migrations
{
    public partial class MoreVerity : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "WalletRequests");

            migrationBuilder.DropTable(
                name: "WalletRequestSteps");

            migrationBuilder.DropTable(
                name: "WalletRequestTypes");

            migrationBuilder.AddColumn<string>(
                name: "WalletName",
                table: "WalletRelationships",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Attributes",
                table: "CredentialSchema",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ThreadId",
                table: "CredentialSchema",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "TypeName",
                table: "CredentialSchema",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ThreadId",
                table: "CredentialDefinitions",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "ConnectionRequests",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AgentContextId = table.Column<Guid>(nullable: true),
                    ThreadId = table.Column<string>(nullable: true),
                    UserId = table.Column<string>(nullable: false),
                    ConnectionRequestStep = table.Column<int>(nullable: false),
                    WalletRelationshipId = table.Column<int>(nullable: true),
                    CreatedOn = table.Column<DateTimeOffset>(nullable: false),
                    ModifiedOn = table.Column<DateTimeOffset>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ConnectionRequests", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ConnectionRequests_AgentContexts_AgentContextId",
                        column: x => x.AgentContextId,
                        principalTable: "AgentContexts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_ConnectionRequests_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_ConnectionRequests_WalletRelationships_WalletRelationshipId",
                        column: x => x.WalletRelationshipId,
                        principalTable: "WalletRelationships",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateTable(
                name: "ConnectionRequestSteps",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false),
                    Name = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ConnectionRequestSteps", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CredentialRequests",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    WalletRelationshipId = table.Column<int>(nullable: false),
                    ThreadId = table.Column<string>(nullable: true),
                    UserId = table.Column<string>(nullable: false),
                    CredentialRequestStep = table.Column<int>(nullable: false),
                    CredentialPackageId = table.Column<int>(nullable: false),
                    CredentialDefinitionId = table.Column<int>(nullable: true),
                    CredentialSchemaId = table.Column<int>(nullable: true),
                    CreatedOn = table.Column<DateTimeOffset>(nullable: false),
                    ModifiedOn = table.Column<DateTimeOffset>(nullable: true),
                    AgentContextId = table.Column<Guid>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CredentialRequests", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CredentialRequests_AgentContexts_AgentContextId",
                        column: x => x.AgentContextId,
                        principalTable: "AgentContexts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_CredentialRequests_CredentialDefinitions_CredentialDefinitionId",
                        column: x => x.CredentialDefinitionId,
                        principalTable: "CredentialDefinitions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CredentialRequests_CredentialPackages_CredentialPackageId",
                        column: x => x.CredentialPackageId,
                        principalTable: "CredentialPackages",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_CredentialRequests_CredentialSchema_CredentialSchemaId",
                        column: x => x.CredentialSchemaId,
                        principalTable: "CredentialSchema",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_CredentialRequests_WalletRelationships_Id",
                        column: x => x.Id,
                        principalTable: "WalletRelationships",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_CredentialRequests_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_CredentialRequests_WalletRelationships_WalletRelationshipId",
                        column: x => x.WalletRelationshipId,
                        principalTable: "WalletRelationships",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateTable(
                name: "CredentialRequestSteps",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false),
                    Name = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CredentialRequestSteps", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "ConnectionRequestSteps",
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
                    { 13, "Error" }
                });

            migrationBuilder.InsertData(
                table: "CredentialRequestSteps",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 6, "OfferSent" },
                    { 5, "SendingOffer" },
                    { 4, "ReadyToSend" },
                    { 0, "Initiated" },
                    { 2, "PendingSchema" },
                    { 1, "PendingAgent" },
                    { 7, "OfferAccepted" },
                    { 3, "PendingCredentialDefinition" },
                    { 13, "Error" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_ConnectionRequests_AgentContextId",
                table: "ConnectionRequests",
                column: "AgentContextId");

            migrationBuilder.CreateIndex(
                name: "IX_ConnectionRequests_ThreadId",
                table: "ConnectionRequests",
                column: "ThreadId",
                unique: true,
                filter: "[ThreadId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_ConnectionRequests_UserId",
                table: "ConnectionRequests",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_ConnectionRequests_WalletRelationshipId",
                table: "ConnectionRequests",
                column: "WalletRelationshipId");

            migrationBuilder.CreateIndex(
                name: "IX_CredentialRequests_AgentContextId",
                table: "CredentialRequests",
                column: "AgentContextId");

            migrationBuilder.CreateIndex(
                name: "IX_CredentialRequests_CredentialDefinitionId",
                table: "CredentialRequests",
                column: "CredentialDefinitionId");

            migrationBuilder.CreateIndex(
                name: "IX_CredentialRequests_CredentialPackageId",
                table: "CredentialRequests",
                column: "CredentialPackageId");

            migrationBuilder.CreateIndex(
                name: "IX_CredentialRequests_CredentialSchemaId",
                table: "CredentialRequests",
                column: "CredentialSchemaId");

            migrationBuilder.CreateIndex(
                name: "IX_CredentialRequests_UserId",
                table: "CredentialRequests",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_CredentialRequests_WalletRelationshipId",
                table: "CredentialRequests",
                column: "WalletRelationshipId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ConnectionRequests");

            migrationBuilder.DropTable(
                name: "ConnectionRequestSteps");

            migrationBuilder.DropTable(
                name: "CredentialRequests");

            migrationBuilder.DropTable(
                name: "CredentialRequestSteps");

            migrationBuilder.DropColumn(
                name: "WalletName",
                table: "WalletRelationships");

            migrationBuilder.DropColumn(
                name: "Attributes",
                table: "CredentialSchema");

            migrationBuilder.DropColumn(
                name: "ThreadId",
                table: "CredentialSchema");

            migrationBuilder.DropColumn(
                name: "TypeName",
                table: "CredentialSchema");

            migrationBuilder.DropColumn(
                name: "ThreadId",
                table: "CredentialDefinitions");

            migrationBuilder.CreateTable(
                name: "WalletRequests",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AgentContextId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CreatedOn = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    CredentialDefinitionId = table.Column<int>(type: "int", nullable: true),
                    CredentialPackageId = table.Column<int>(type: "int", nullable: true),
                    CredentialSchemaId = table.Column<int>(type: "int", nullable: true),
                    InviteUrl = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ModifiedOn = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    ThreadId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    WalletRelationshipId = table.Column<int>(type: "int", nullable: true),
                    WalletRequestStep = table.Column<int>(type: "int", nullable: false),
                    WalletRequestType = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WalletRequests", x => x.Id);
                    table.ForeignKey(
                        name: "FK_WalletRequests_AgentContexts_AgentContextId",
                        column: x => x.AgentContextId,
                        principalTable: "AgentContexts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
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

            migrationBuilder.CreateTable(
                name: "WalletRequestSteps",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WalletRequestSteps", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "WalletRequestTypes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WalletRequestTypes", x => x.Id);
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
    }
}
