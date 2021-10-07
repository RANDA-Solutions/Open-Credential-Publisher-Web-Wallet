using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace OpenCredentialPublisher.Data.Contexts.Migrations
{
    public partial class AngularConversion : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BadgrBackpacks_CredentialPackages_CredentialPackageId",
                table: "BadgrBackpacks");

            migrationBuilder.DropForeignKey(
                name: "FK_Clrs_Authorizations_AuthorizationForeignKey",
                table: "Clrs");

            migrationBuilder.DropForeignKey(
                name: "FK_Clrs_ClrSets_ClrSetId",
                table: "Clrs");

            migrationBuilder.DropForeignKey(
                name: "FK_Clrs_CredentialPackages_CredentialPackageId",
                table: "Clrs");

            migrationBuilder.DropForeignKey(
                name: "FK_Clrs_VerifiableCredentials_VerifiableCredentialId",
                table: "Clrs");

            migrationBuilder.DropForeignKey(
                name: "FK_ClrSets_CredentialPackages_CredentialPackageId",
                table: "ClrSets");

            migrationBuilder.DropForeignKey(
                name: "FK_ClrSets_VerifiableCredentials_VerifiableCredentialId",
                table: "ClrSets");

            migrationBuilder.DropForeignKey(
                name: "FK_Recipients_AspNetUsers_UserId",
                table: "Recipients");

            migrationBuilder.DropForeignKey(
                name: "FK_VerifiableCredentials_CredentialPackages_CredentialPackageId",
                table: "VerifiableCredentials");

            migrationBuilder.DropIndex(
                name: "IX_ClrSets_CredentialPackageId",
                table: "ClrSets");

            migrationBuilder.DropIndex(
                name: "IX_Clrs_ClrSetId",
                table: "Clrs");

            migrationBuilder.DropIndex(
                name: "IX_Clrs_CredentialPackageId",
                table: "Clrs");

            migrationBuilder.DropColumn(
                name: "Created",
                table: "Authorizations");

            migrationBuilder.DropColumn(
                name: "Modified",
                table: "Authorizations");

            migrationBuilder.RenameColumn(
                name: "CredentialPackageId",
                table: "VerifiableCredentials",
                newName: "ParentCredentialPackageId");

            migrationBuilder.RenameIndex(
                name: "IX_VerifiableCredentials_CredentialPackageId",
                table: "VerifiableCredentials",
                newName: "IX_VerifiableCredentials_ParentCredentialPackageId");

            migrationBuilder.RenameColumn(
                name: "VerifiableCredentialId",
                table: "ClrSets",
                newName: "VParenterifiableCredentialId");

            migrationBuilder.RenameColumn(
                name: "CredentialPackageId",
                table: "ClrSets",
                newName: "ParentVerifiableCredentialId");

            migrationBuilder.RenameIndex(
                name: "IX_ClrSets_VerifiableCredentialId",
                table: "ClrSets",
                newName: "IX_ClrSets_VParenterifiableCredentialId");

            migrationBuilder.RenameColumn(
                name: "VerifiableCredentialId",
                table: "Clrs",
                newName: "ParentVerifiableCredentialId");

            migrationBuilder.RenameColumn(
                name: "ClrSetId",
                table: "Clrs",
                newName: "ParentCredentialPackageId");

            migrationBuilder.RenameIndex(
                name: "IX_Clrs_VerifiableCredentialId",
                table: "Clrs",
                newName: "IX_Clrs_ParentVerifiableCredentialId");

            migrationBuilder.RenameColumn(
                name: "CredentialPackageId",
                table: "BadgrBackpacks",
                newName: "ParentCredentialPackageId");

            migrationBuilder.RenameIndex(
                name: "IX_BadgrBackpacks_CredentialPackageId",
                table: "BadgrBackpacks",
                newName: "IX_BadgrBackpacks_ParentCredentialPackageId");

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "VerifiableCredentials",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "VerifiableCredentials",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "ModifiedAt",
                table: "VerifiableCredentials",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "Sources",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<bool>(
                name: "IsDeletable",
                table: "Sources",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "Sources",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "ModifiedAt",
                table: "Sources",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "Recipients",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Recipients",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Email",
                table: "Recipients",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Id",
                table: "DiscoveryDocumentModel",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<string>(
                name: "ApiBase",
                table: "DiscoveryDocumentModel",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Key",
                table: "DiscoveryDocumentModel",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "TokenRevocationUrl",
                table: "DiscoveryDocumentModel",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Version",
                table: "DiscoveryDocumentModel",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "CredentialPackages",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "ClrSets",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "ClrSets",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "ModifiedAt",
                table: "ClrSets",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<int>(
                name: "ParentCredentialPackageId",
                table: "ClrSets",
                type: "int",
                nullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "CredentialPackageId",
                table: "Clrs",
                type: "int",
                nullable: true,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "Clrs",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "Clrs",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "ModifiedAt",
                table: "Clrs",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<int>(
                name: "ParentClrSetId",
                table: "Clrs",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "BadgrBackpacks",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "BadgrBackpacks",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "ModifiedAt",
                table: "BadgrBackpacks",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AlterColumn<string>(
                name: "Type",
                table: "BadgrAssertions",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Id",
                table: "BadgrAssertions",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            //migrationBuilder.AlterColumn<DateTime>(
            //    name: "Expires",
            //    table: "BadgrAssertions",
            //    type: "datetime2",
            //    nullable: true,
            //    oldClrType: typeof(long),
            //    oldType: "bigint",
            //    oldNullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "BadgrAssertions",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "BadgrAssertions",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "ModifiedAt",
                table: "BadgrAssertions",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AlterColumn<DateTime>(
                name: "ValidTo",
                table: "Authorizations",
                type: "datetime2",
                nullable: false,
                oldClrType: typeof(DateTimeOffset),
                oldType: "datetimeoffset");

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "Authorizations",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "ModifiedAt",
                table: "Authorizations",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.CreateTable(
                name: "ClrAssertions",
                columns: table => new
                {
                    ClrAssertionId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ClrForeignKey = table.Column<int>(type: "int", nullable: false),
                    SignedAssertion = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Id = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Type = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Achievement = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreditsEarned = table.Column<float>(type: "real", nullable: true),
                    ActivityEndDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Endorsements = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Evidence = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Expires = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Image = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IssuedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LicenseNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Narrative = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Recipient = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Results = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RevocationReason = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Revoked = table.Column<bool>(type: "bit", nullable: true),
                    Role = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SignedEndorsements = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Source = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ActivityStartDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Term = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Verification = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AdditionalProperties = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AssertionClrs = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AssertionKey = table.Column<int>(type: "int", nullable: false),
                    Context = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsSigned = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ClrAssertions", x => x.ClrAssertionId);
                    table.ForeignKey(
                        name: "FK_ClrAssertions_Clrs_ClrForeignKey",
                        column: x => x.ClrForeignKey,
                        principalTable: "Clrs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });


            migrationBuilder.CreateIndex(
                name: "IX_ClrSets_ParentCredentialPackageId",
                table: "ClrSets",
                column: "ParentCredentialPackageId",
                unique: true,
                filter: "[ParentCredentialPackageId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Clrs_CredentialPackageId",
                table: "Clrs",
                column: "CredentialPackageId");

            migrationBuilder.CreateIndex(
                name: "IX_Clrs_ParentClrSetId",
                table: "Clrs",
                column: "ParentClrSetId");

            migrationBuilder.CreateIndex(
                name: "IX_Clrs_ParentCredentialPackageId",
                table: "Clrs",
                column: "ParentCredentialPackageId",
                unique: false,
                filter: "[ParentCredentialPackageId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_ClrAssertions_ClrForeignKey",
                table: "ClrAssertions",
                column: "ClrForeignKey");

            migrationBuilder.AddForeignKey(
                name: "FK_BadgrBackpacks_CredentialPackages_ParentCredentialPackageId",
                table: "BadgrBackpacks",
                column: "ParentCredentialPackageId",
                principalTable: "CredentialPackages",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Clrs_Authorizations_AuthorizationForeignKey",
                table: "Clrs",
                column: "AuthorizationForeignKey",
                principalTable: "Authorizations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Clrs_ClrSets_ParentClrSetId",
                table: "Clrs",
                column: "ParentClrSetId",
                principalTable: "ClrSets",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Clrs_CredentialPackages_CredentialPackageId",
                table: "Clrs",
                column: "CredentialPackageId",
                principalTable: "CredentialPackages",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            //migrationBuilder.AddForeignKey(
            //    name: "FK_Clrs_CredentialPackages_ParentCredentialPackageId",
            //    table: "Clrs",
            //    column: "ParentCredentialPackageId",
            //    principalTable: "CredentialPackages",
            //    principalColumn: "Id",
            //    onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Clrs_VerifiableCredentials_ParentVerifiableCredentialId",
                table: "Clrs",
                column: "ParentVerifiableCredentialId",
                principalTable: "VerifiableCredentials",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ClrSets_CredentialPackages_ParentCredentialPackageId",
                table: "ClrSets",
                column: "ParentCredentialPackageId",
                principalTable: "CredentialPackages",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ClrSets_VerifiableCredentials_VParenterifiableCredentialId",
                table: "ClrSets",
                column: "VParenterifiableCredentialId",
                principalTable: "VerifiableCredentials",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Recipients_AspNetUsers_UserId",
                table: "Recipients",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_VerifiableCredentials_CredentialPackages_ParentCredentialPackageId",
                table: "VerifiableCredentials",
                column: "ParentCredentialPackageId",
                principalTable: "CredentialPackages",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
                    }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BadgrBackpacks_CredentialPackages_ParentCredentialPackageId",
                table: "BadgrBackpacks");

            migrationBuilder.DropForeignKey(
                name: "FK_Clrs_Authorizations_AuthorizationForeignKey",
                table: "Clrs");

            migrationBuilder.DropForeignKey(
                name: "FK_Clrs_ClrSets_ParentClrSetId",
                table: "Clrs");

            migrationBuilder.DropForeignKey(
                name: "FK_Clrs_CredentialPackages_CredentialPackageId",
                table: "Clrs");

            migrationBuilder.DropForeignKey(
                name: "FK_Clrs_CredentialPackages_ParentCredentialPackageId",
                table: "Clrs");

            migrationBuilder.DropForeignKey(
                name: "FK_Clrs_VerifiableCredentials_ParentVerifiableCredentialId",
                table: "Clrs");

            migrationBuilder.DropForeignKey(
                name: "FK_ClrSets_CredentialPackages_ParentCredentialPackageId",
                table: "ClrSets");

            migrationBuilder.DropForeignKey(
                name: "FK_ClrSets_VerifiableCredentials_VParenterifiableCredentialId",
                table: "ClrSets");

            migrationBuilder.DropForeignKey(
                name: "FK_Recipients_AspNetUsers_UserId",
                table: "Recipients");

            migrationBuilder.DropForeignKey(
                name: "FK_VerifiableCredentials_CredentialPackages_ParentCredentialPackageId",
                table: "VerifiableCredentials");
                        

            migrationBuilder.DropTable(
                name: "ClrAssertions");

            migrationBuilder.DropIndex(
                name: "IX_ClrSets_ParentCredentialPackageId",
                table: "ClrSets");

            migrationBuilder.DropIndex(
                name: "IX_Clrs_CredentialPackageId",
                table: "Clrs");

            migrationBuilder.DropIndex(
                name: "IX_Clrs_ParentClrSetId",
                table: "Clrs");

            migrationBuilder.DropIndex(
                name: "IX_Clrs_ParentCredentialPackageId",
                table: "Clrs");

            migrationBuilder.DeleteData(
                table: "Statuses",
                keyColumn: "Id",
                keyValue: 14);

            migrationBuilder.DeleteData(
                table: "Statuses",
                keyColumn: "Id",
                keyValue: 15);

            migrationBuilder.DeleteData(
                table: "Statuses",
                keyColumn: "Id",
                keyValue: 16);

            migrationBuilder.DeleteData(
                table: "Statuses",
                keyColumn: "Id",
                keyValue: 17);

            migrationBuilder.DeleteData(
                table: "Statuses",
                keyColumn: "Id",
                keyValue: 18);

            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "VerifiableCredentials");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "VerifiableCredentials");

            migrationBuilder.DropColumn(
                name: "ModifiedAt",
                table: "VerifiableCredentials");

            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "Sources");

            migrationBuilder.DropColumn(
                name: "IsDeletable",
                table: "Sources");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "Sources");

            migrationBuilder.DropColumn(
                name: "ModifiedAt",
                table: "Sources");

            migrationBuilder.DropColumn(
                name: "ApiBase",
                table: "DiscoveryDocumentModel");

            migrationBuilder.DropColumn(
                name: "Key",
                table: "DiscoveryDocumentModel");

            migrationBuilder.DropColumn(
                name: "TokenRevocationUrl",
                table: "DiscoveryDocumentModel");

            migrationBuilder.DropColumn(
                name: "Version",
                table: "DiscoveryDocumentModel");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "CredentialPackages");

            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "ClrSets");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "ClrSets");

            migrationBuilder.DropColumn(
                name: "ModifiedAt",
                table: "ClrSets");

            migrationBuilder.DropColumn(
                name: "ParentCredentialPackageId",
                table: "ClrSets");

            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "Clrs");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "Clrs");

            migrationBuilder.DropColumn(
                name: "ModifiedAt",
                table: "Clrs");

            migrationBuilder.DropColumn(
                name: "ParentClrSetId",
                table: "Clrs");

            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "BadgrBackpacks");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "BadgrBackpacks");

            migrationBuilder.DropColumn(
                name: "ModifiedAt",
                table: "BadgrBackpacks");

            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "BadgrAssertions");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "BadgrAssertions");

            migrationBuilder.DropColumn(
                name: "ModifiedAt",
                table: "BadgrAssertions");

            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "Authorizations");

            migrationBuilder.DropColumn(
                name: "ModifiedAt",
                table: "Authorizations");

            migrationBuilder.RenameColumn(
                name: "ParentCredentialPackageId",
                table: "VerifiableCredentials",
                newName: "CredentialPackageId");

            migrationBuilder.RenameIndex(
                name: "IX_VerifiableCredentials_ParentCredentialPackageId",
                table: "VerifiableCredentials",
                newName: "IX_VerifiableCredentials_CredentialPackageId");

            migrationBuilder.RenameColumn(
                name: "VParenterifiableCredentialId",
                table: "ClrSets",
                newName: "VerifiableCredentialId");

            migrationBuilder.RenameColumn(
                name: "ParentVerifiableCredentialId",
                table: "ClrSets",
                newName: "CredentialPackageId");

            migrationBuilder.RenameIndex(
                name: "IX_ClrSets_VParenterifiableCredentialId",
                table: "ClrSets",
                newName: "IX_ClrSets_VerifiableCredentialId");

            migrationBuilder.RenameColumn(
                name: "ParentVerifiableCredentialId",
                table: "Clrs",
                newName: "VerifiableCredentialId");

            migrationBuilder.RenameColumn(
                name: "ParentCredentialPackageId",
                table: "Clrs",
                newName: "ClrSetId");

            migrationBuilder.RenameIndex(
                name: "IX_Clrs_ParentVerifiableCredentialId",
                table: "Clrs",
                newName: "IX_Clrs_VerifiableCredentialId");

            migrationBuilder.RenameColumn(
                name: "ParentCredentialPackageId",
                table: "BadgrBackpacks",
                newName: "CredentialPackageId");

            migrationBuilder.RenameIndex(
                name: "IX_BadgrBackpacks_ParentCredentialPackageId",
                table: "BadgrBackpacks",
                newName: "IX_BadgrBackpacks_CredentialPackageId");

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "Recipients",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Recipients",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "Email",
                table: "Recipients",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<int>(
                name: "Id",
                table: "DiscoveryDocumentModel",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "CredentialPackageId",
                table: "Clrs",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<string>(
                name: "Type",
                table: "BadgrAssertions",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "Id",
                table: "BadgrAssertions",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<long>(
                name: "Expires",
                table: "BadgrAssertions",
                type: "bigint",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "ValidTo",
                table: "Authorizations",
                type: "datetimeoffset",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "Created",
                table: "Authorizations",
                type: "datetimeoffset",
                nullable: false,
                defaultValue: new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)));

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "Modified",
                table: "Authorizations",
                type: "datetimeoffset",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_ClrSets_CredentialPackageId",
                table: "ClrSets",
                column: "CredentialPackageId",
                unique: true,
                filter: "[CredentialPackageId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Clrs_ClrSetId",
                table: "Clrs",
                column: "ClrSetId");

            migrationBuilder.CreateIndex(
                name: "IX_Clrs_CredentialPackageId",
                table: "Clrs",
                column: "CredentialPackageId",
                unique: true,
                filter: "[CredentialPackageId] IS NOT NULL");

            migrationBuilder.AddForeignKey(
                name: "FK_BadgrBackpacks_CredentialPackages_CredentialPackageId",
                table: "BadgrBackpacks",
                column: "CredentialPackageId",
                principalTable: "CredentialPackages",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Clrs_Authorizations_AuthorizationForeignKey",
                table: "Clrs",
                column: "AuthorizationForeignKey",
                principalTable: "Authorizations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Clrs_ClrSets_ClrSetId",
                table: "Clrs",
                column: "ClrSetId",
                principalTable: "ClrSets",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Clrs_CredentialPackages_CredentialPackageId",
                table: "Clrs",
                column: "CredentialPackageId",
                principalTable: "CredentialPackages",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Clrs_VerifiableCredentials_VerifiableCredentialId",
                table: "Clrs",
                column: "VerifiableCredentialId",
                principalTable: "VerifiableCredentials",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ClrSets_CredentialPackages_CredentialPackageId",
                table: "ClrSets",
                column: "CredentialPackageId",
                principalTable: "CredentialPackages",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ClrSets_VerifiableCredentials_VerifiableCredentialId",
                table: "ClrSets",
                column: "VerifiableCredentialId",
                principalTable: "VerifiableCredentials",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Recipients_AspNetUsers_UserId",
                table: "Recipients",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_VerifiableCredentials_CredentialPackages_CredentialPackageId",
                table: "VerifiableCredentials",
                column: "CredentialPackageId",
                principalTable: "CredentialPackages",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
