﻿using Microsoft.EntityFrameworkCore.Migrations;

namespace OpenCredentialPublisher.Data.Contexts.Migrations
{
    public partial class authcascade_setnull_packasge : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CredentialPackages_Authorizations_AuthorizationForeignKey",
                table: "CredentialPackages");

            migrationBuilder.AddForeignKey(
                name: "FK_CredentialPackages_Authorizations_AuthorizationForeignKey",
                table: "CredentialPackages",
                column: "AuthorizationForeignKey",
                principalTable: "Authorizations",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CredentialPackages_Authorizations_AuthorizationForeignKey",
                table: "CredentialPackages");

            migrationBuilder.AddForeignKey(
                name: "FK_CredentialPackages_Authorizations_AuthorizationForeignKey",
                table: "CredentialPackages",
                column: "AuthorizationForeignKey",
                principalTable: "Authorizations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}