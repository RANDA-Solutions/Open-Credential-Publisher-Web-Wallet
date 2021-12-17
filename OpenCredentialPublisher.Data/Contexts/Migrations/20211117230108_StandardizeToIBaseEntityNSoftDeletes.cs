using Microsoft.EntityFrameworkCore.Migrations;
using System;

namespace OpenCredentialPublisher.Data.Contexts.Migrations
{
    public partial class StandardizeToIBaseEntityNSoftDeletes : Migration
    {

        protected override void Up(MigrationBuilder migrationBuilder)
        {
            //F-ed up

            //migrationBuilder.DropColumn(
            //    name: "IsDeleted",
            //    table: "Messages");

            //migrationBuilder.DropColumn(
            //    name: "IsDeleted",
            //    table: "CredentialRequests");

            //migrationBuilder.DropColumn(
            //   name: "IsDeleted",
            //   table: "Revocations");

            //migrationBuilder.DropColumn(
            //   name: "ModifiedAt",
            //   table: "Revocations");

            //End F-ed up

            migrationBuilder.RenameColumn(
                name: "CreatedOn",
                table: "WalletRelationships",
                newName: "CreatedAt");

            migrationBuilder.RenameColumn(
                name: "ModifiedOn",
                table: "WalletRelationships",
                newName: "ModifiedAt");

            migrationBuilder.RenameColumn(
                name: "CreatedOn",
                table: "Shares",
                newName: "CreatedAt");

            migrationBuilder.RenameColumn(
                name: "CreatedOn",
                table: "Recipients",
                newName: "CreatedAt");

            migrationBuilder.RenameColumn(
                name: "CreatedOn",
                table: "ProvisioningToken",
                newName: "CreatedAt");

            migrationBuilder.RenameColumn(
                name: "ModifiedOn",
                table: "ProvisioningToken",
                newName: "ModifiedAt");

            migrationBuilder.RenameColumn(
                name: "CreatedOn",
                table: "ProofResponses",
                newName: "CreatedAt");

            migrationBuilder.RenameColumn(
                name: "CreatedOn",
                table: "ProofRequests",
                newName: "CreatedAt");

            migrationBuilder.RenameColumn(
                name: "ModifiedOn",
                table: "ProofRequests",
                newName: "ModifiedAt");

            migrationBuilder.RenameColumn(
                name: "CreatedOn",
                table: "Messages",
                newName: "CreatedAt");

            migrationBuilder.RenameColumn(
                name: "CreatedOn",
                table: "CredentialSchema",
                newName: "CreatedAt");

            migrationBuilder.RenameColumn(
                name: "ModifiedOn",
                table: "CredentialSchema",
                newName: "ModifiedAt");

            migrationBuilder.RenameColumn(
                name: "CreatedOn",
                table: "CredentialRequests",
                newName: "CreatedAt");

            migrationBuilder.RenameColumn(
                name: "ModifiedOn",
                table: "CredentialRequests",
                newName: "ModifiedAt");

            migrationBuilder.RenameColumn(
                name: "CreatedOn",
                table: "CredentialDefinitions",
                newName: "CreatedAt");

            migrationBuilder.RenameColumn(
                name: "ModifiedOn",
                table: "CredentialDefinitions",
                newName: "ModifiedAt");

            migrationBuilder.RenameColumn(
                name: "CreatedOn",
                table: "ConnectionRequests",
                newName: "CreatedAt");

            migrationBuilder.RenameColumn(
                name: "ModifiedOn",
                table: "ConnectionRequests",
                newName: "ModifiedAt");

            migrationBuilder.RenameColumn(
                name: "CreatedOn",
                table: "AgentContexts",
                newName: "CreatedAt");

            migrationBuilder.RenameColumn(
                name: "ModifiedOn",
                table: "AgentContexts",
                newName: "ModifiedAt");

            migrationBuilder.RenameColumn(
                name: "DateCreated",
                table: "Revocations",
                newName: "CreatedAt");


            //DataFix 1

            migrationBuilder.Sql(@"UPDATE dbo.WalletRelationships SET ModifiedAt = CreatedAt WHERE ModifiedAt IS NULL");
            migrationBuilder.Sql(@"UPDATE dbo.Links SET ModifiedAt = CreatedAt WHERE ModifiedAt IS NULL");
            migrationBuilder.Sql(@"UPDATE dbo.ProvisioningToken SET ModifiedAt = CreatedAt WHERE ModifiedAt IS NULL");
            migrationBuilder.Sql(@"UPDATE dbo.ProofRequests SET ModifiedAt = CreatedAt WHERE ModifiedAt IS NULL");
            migrationBuilder.Sql(@"UPDATE dbo.CredentialSchema SET ModifiedAt = CreatedAt WHERE ModifiedAt IS NULL");
            migrationBuilder.Sql(@"UPDATE dbo.CredentialRequests SET ModifiedAt = CreatedAt WHERE ModifiedAt IS NULL");
            migrationBuilder.Sql(@"UPDATE dbo.CredentialDefinitions SET ModifiedAt = CreatedAt WHERE ModifiedAt IS NULL");
            migrationBuilder.Sql(@"UPDATE dbo.ConnectionRequests SET ModifiedAt = CreatedAt WHERE ModifiedAt IS NULL");
            migrationBuilder.Sql(@"UPDATE dbo.AgentContexts SET ModifiedAt = CreatedAt WHERE ModifiedAt IS NULL");


            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedAt",
                table: "WalletRelationships",
                type: "datetime2",
                nullable: false,
                oldClrType: typeof(DateTimeOffset),
                oldType: "datetimeoffset");

            migrationBuilder.AddColumn<bool>(
                 name: "IsDeleted",
                 table: "WalletRelationships",
                 type: "bit",
                 nullable: false,
                 defaultValue: false);

            migrationBuilder.AlterColumn<DateTime>(
                name: "ModifiedAt",
                table: "WalletRelationships",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1980, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                oldClrType: typeof(DateTimeOffset),
                oldType: "datetimeoffset",
                oldNullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "Shares",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "ModifiedAt",
                table: "Shares",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1980, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedAt",
                table: "Recipients",
                type: "datetime2",
                nullable: false,
                oldClrType: typeof(DateTimeOffset),
                oldType: "datetimeoffset");

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "Recipients",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "ModifiedAt",
                table: "Recipients",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1980, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedAt",
                table: "ProvisioningToken",
                type: "datetime2",
                nullable: false,
                oldClrType: typeof(DateTimeOffset),
                oldType: "datetimeoffset");

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "ProvisioningToken",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AlterColumn<DateTime>(
                name: "ModifiedAt",
                table: "ProvisioningToken",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1980, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                oldClrType: typeof(DateTimeOffset),
                oldType: "datetimeoffset",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedAt",
                table: "ProofResponses",
                type: "datetime2",
                nullable: false,
                oldClrType: typeof(DateTimeOffset),
                oldType: "datetimeoffset");

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "ProofResponses",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "ModifiedAt",
                table: "ProofResponses",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1980, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedAt",
                table: "ProofRequests",
                type: "datetime2",
                nullable: false,
                oldClrType: typeof(DateTimeOffset),
                oldType: "datetimeoffset");

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "ProofRequests",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AlterColumn<DateTime>(
                name: "ModifiedAt",
                table: "ProofRequests",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1980, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                oldClrType: typeof(DateTimeOffset),
                oldType: "datetimeoffset",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedAt",
                table: "Messages",
                type: "datetime2",
                nullable: false,
                oldClrType: typeof(DateTimeOffset),
                oldType: "datetimeoffset");

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "Messages",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "ModifiedAt",
                table: "Messages",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1980, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedAt",
                table: "CredentialSchema",
                type: "datetime2",
                nullable: false,
                oldClrType: typeof(DateTimeOffset),
                oldType: "datetimeoffset");

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "CredentialSchema",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AlterColumn<DateTime>(
                name: "ModifiedAt",
                table: "CredentialSchema",
                type: "datetime2",
                nullable: false,
                oldClrType: typeof(DateTimeOffset),
                oldType: "datetimeoffset",
                defaultValue: new DateTime(1980, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedAt",
                table: "CredentialRequests",
                type: "datetime2",
                nullable: false,
                oldClrType: typeof(DateTimeOffset),
                oldType: "datetimeoffset");

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "CredentialRequests",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AlterColumn<DateTime>(
                name: "ModifiedAt",
                table: "CredentialRequests",
                type: "datetime2",
                nullable: false,
                oldClrType: typeof(DateTimeOffset),
                oldType: "datetimeoffset",
                defaultValue: new DateTime(1980, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedAt",
                table: "CredentialDefinitions",
                type: "datetime2",
                nullable: false,
                oldClrType: typeof(DateTimeOffset),
                oldType: "datetimeoffset");

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "CredentialDefinitions",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AlterColumn<DateTime>(
                name: "ModifiedAt",
                table: "CredentialDefinitions",
                type: "datetime2",
                nullable: false,
                oldClrType: typeof(DateTimeOffset),
                oldType: "datetimeoffset",
                defaultValue: new DateTime(1980, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedAt",
                table: "ConnectionRequests",
                type: "datetime2",
                nullable: false,
                oldClrType: typeof(DateTimeOffset),
                oldType: "datetimeoffset");

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "ConnectionRequests",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AlterColumn<DateTime>(
                name: "ModifiedAt",
                table: "ConnectionRequests",
                type: "datetime2",
                nullable: false,
                oldClrType: typeof(DateTimeOffset),
                oldType: "datetimeoffset",
                defaultValue: new DateTime(1980, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedAt",
                table: "AgentContexts",
                type: "datetime2",
                nullable: false,
                oldClrType: typeof(DateTimeOffset),
                oldType: "datetimeoffset");

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "AgentContexts",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AlterColumn<DateTime>(
                name: "ModifiedAt",
                table: "AgentContexts",
                type: "datetime2",
                nullable: false,
                oldClrType: typeof(DateTimeOffset),
                oldType: "datetimeoffset",
                defaultValue: new DateTime(1980, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedAt",
                table: "Revocations",
                type: "datetime2",
                nullable: false,
                oldClrType: typeof(DateTimeOffset),
                oldType: "datetimeoffset");

            migrationBuilder.AddColumn<DateTime>(
                name: "ModifiedAt",
                table: "Revocations",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1980, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "Revocations",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedAt",
                table: "LoginProofRequests",
                type: "datetime2",
                nullable: false,
                oldClrType: typeof(DateTimeOffset),
                oldType: "datetimeoffset");

            migrationBuilder.AddColumn<DateTime>(
                name: "ModifiedAt",
                table: "LoginProofRequests",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AlterColumn<DateTime>(
                name: "ModifiedAt",
                table: "Links",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1980, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                oldClrType: typeof(DateTimeOffset),
                oldType: "datetimeoffset",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedAt",
                table: "Links",
                type: "datetime2",
                nullable: false,
                oldClrType: typeof(DateTimeOffset),
                oldType: "datetimeoffset");

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedAt",
                table: "EmailVerifications",
                type: "datetime2",
                nullable: false,
                oldClrType: typeof(DateTimeOffset),
                oldType: "datetimeoffset");

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "EmailVerifications",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "ModifiedAt",
                table: "EmailVerifications",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1980, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            //DataFix 2

            migrationBuilder.Sql(@"UPDATE dbo.WalletRelationships SET ModifiedAt = CreatedAt WHERE ModifiedAt = '1980-01-01'");
            migrationBuilder.Sql(@"UPDATE dbo.Links SET ModifiedAt = CreatedAt WHERE ModifiedAt = '1980-01-01'");
            migrationBuilder.Sql(@"UPDATE dbo.Shares SET ModifiedAt = CreatedAt WHERE ModifiedAt = '1980-01-01'");
            migrationBuilder.Sql(@"UPDATE dbo.Recipients SET ModifiedAt = CreatedAt WHERE ModifiedAt = '1980-01-01'");
            migrationBuilder.Sql(@"UPDATE dbo.ProvisioningToken SET ModifiedAt = CreatedAt WHERE ModifiedAt = '1980-01-01'");
            migrationBuilder.Sql(@"UPDATE dbo.ProofResponses SET ModifiedAt = CreatedAt WHERE ModifiedAt = '1980-01-01'");
            migrationBuilder.Sql(@"UPDATE dbo.ProofRequests SET ModifiedAt = CreatedAt WHERE ModifiedAt = '1980-01-01'");
            migrationBuilder.Sql(@"UPDATE dbo.Messages SET ModifiedAt = CreatedAt WHERE ModifiedAt = '1980-01-01'");
            migrationBuilder.Sql(@"UPDATE dbo.CredentialSchema SET ModifiedAt = CreatedAt WHERE ModifiedAt = '1980-01-01'");
            migrationBuilder.Sql(@"UPDATE dbo.CredentialRequests SET ModifiedAt = CreatedAt WHERE ModifiedAt = '1980-01-01'");
            migrationBuilder.Sql(@"UPDATE dbo.CredentialDefinitions SET ModifiedAt = CreatedAt WHERE ModifiedAt = '1980-01-01'");
            migrationBuilder.Sql(@"UPDATE dbo.ConnectionRequests SET ModifiedAt = CreatedAt WHERE ModifiedAt = '1980-01-01'");
            migrationBuilder.Sql(@"UPDATE dbo.AgentContexts SET ModifiedAt = CreatedAt WHERE ModifiedAt = '1980-01-01'");
            migrationBuilder.Sql(@"UPDATE dbo.Revocations SET ModifiedAt = CreatedAt WHERE ModifiedAt = '1980-01-01'");
            migrationBuilder.Sql(@"UPDATE dbo.LoginProofRequests SET ModifiedAt = CreatedAt WHERE ModifiedAt = '1980-01-01'");
            migrationBuilder.Sql(@"UPDATE dbo.EmailVerifications SET ModifiedAt = CreatedAt WHERE ModifiedAt = '1980-01-01'");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "Shares");

            migrationBuilder.DropColumn(
                name: "ModifiedAt",
                table: "Shares");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "Recipients");

            migrationBuilder.DropColumn(
                name: "ModifiedAt",
                table: "Recipients");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "ProvisioningToken");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "ProofResponses");

            migrationBuilder.DropColumn(
                name: "ModifiedAt",
                table: "ProofResponses");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "ProofRequests");

            migrationBuilder.DropColumn(
                name: "ModifiedAt",
                table: "Messages");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "Messages");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "CredentialSchema");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "CredentialDefinitions");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "ConnectionRequests");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "AgentContexts");

            migrationBuilder.DropColumn(
               name: "IsDeleted",
               table: "Revocations");

            migrationBuilder.DropColumn(
               name: "ModifiedAt",
               table: "Revocations");

            migrationBuilder.DropColumn(
                name: "ModifiedAt",
                table: "LoginProofRequests");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "EmailVerifications");

            migrationBuilder.DropColumn(
                name: "ModifiedAt",
                table: "EmailVerifications");


            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "CreatedAt",
                table: "WalletRelationships",
                type: "datetimeoffset",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "ModifiedAt",
                table: "WalletRelationships",
                type: "datetimeoffset",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "CreatedAt",
                table: "Recipients",
                type: "datetimeoffset",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "CreatedAt",
                table: "ProvisioningToken",
                type: "datetimeoffset",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");


            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "ModifiedAt",
                table: "ProvisioningToken",
                type: "datetimeoffset",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "CreatedAt",
                table: "ProofResponses",
                type: "datetimeoffset",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "CreatedAt",
                table: "ProofRequests",
                type: "datetimeoffset",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "ModifiedAt",
                table: "ProofRequests",
                type: "datetimeoffset",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetimeoffset",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "CreatedAt",
                table: "Messages",
                type: "datetimeoffset",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "CreatedAt",
                table: "CredentialSchema",
                type: "datetimeoffset",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "ModifiedAt",
                table: "CredentialSchema",
                type: "datetimeoffset",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetimeoffset");

            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "CreatedAt",
                table: "CredentialRequests",
                type: "datetimeoffset",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "ModifiedAt",
                table: "CredentialRequests",
                type: "datetimeoffset",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetimeoffset");

            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "CreatedAt",
                table: "CredentialDefinitions",
                type: "datetimeoffset",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "ModifiedAt",
                table: "CredentialDefinitions",
                type: "datetimeoffset",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetimeoffset");

            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "CreatedAt",
                table: "ConnectionRequests",
                type: "datetimeoffset",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "ModifiedAt",
                table: "ConnectionRequests",
                type: "datetimeoffset",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetimeoffset");

            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "CreatedAt",
                table: "AgentContexts",
                type: "datetimeoffset",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "ModifiedAt",
                table: "AgentContexts",
                type: "datetimeoffset",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetimeoffset");

            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "CreatedAt",
                table: "Revocations",
                type: "datetimeoffset",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "CreatedAt",
                table: "LoginProofRequests",
                type: "datetimeoffset",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "ModifiedAt",
                table: "Links",
                type: "datetimeoffset",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetimeoffset",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "CreatedAt",
                table: "Links",
                type: "datetimeoffset",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "CreatedAt",
                table: "EmailVerifications",
                type: "datetimeoffset",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.RenameColumn(
               name: "CreatedAt",
               table: "WalletRelationships",
               newName: "CreatedOn");

            migrationBuilder.RenameColumn(
                name: "ModifiedAt",
                table: "WalletRelationships",
                newName: "ModifiedOn");

            migrationBuilder.RenameColumn(
                name: "CreatedAt",
                table: "Shares",
                newName: "CreatedOn");

            migrationBuilder.RenameColumn(
                name: "CreatedAt",
                table: "Recipients",
                newName: "CreatedOn");

            migrationBuilder.RenameColumn(
                name: "CreatedAt",
                table: "ProvisioningToken",
                newName: "CreatedOn");

            migrationBuilder.RenameColumn(
                name: "ModifiedAt",
                table: "ProvisioningToken",
                newName: "ModifiedOn");

            migrationBuilder.RenameColumn(
                name: "CreatedAt",
                table: "ProofResponses",
                newName: "CreatedOn");

            migrationBuilder.RenameColumn(
                name: "CreatedAt",
                table: "ProofRequests",
                newName: "CreatedOn");

            migrationBuilder.RenameColumn(
                name: "ModifiedAt",
                table: "ProofRequests",
                newName: "ModifiedOn");

            migrationBuilder.RenameColumn(
                name: "CreatedAt",
                table: "Messages",
                newName: "CreatedOn");

            migrationBuilder.RenameColumn(
                name: "CreatedAt",
                table: "CredentialSchema",
                newName: "CreatedOn");

            migrationBuilder.RenameColumn(
                name: "ModifiedAt",
                table: "CredentialSchema",
                newName: "ModifiedOn");

            migrationBuilder.RenameColumn(
                name: "CreatedAt",
                table: "CredentialRequests",
                newName: "CreatedOn");

            migrationBuilder.RenameColumn(
                name: "ModifiedAt",
                table: "CredentialRequests",
                newName: "ModifiedOn");

            migrationBuilder.RenameColumn(
                name: "CreatedAt",
                table: "CredentialDefinitions",
                newName: "CreatedOn");

            migrationBuilder.RenameColumn(
                name: "ModifiedAt",
                table: "CredentialDefinitions",
                newName: "ModifiedOn");

            migrationBuilder.RenameColumn(
                name: "CreatedAt",
                table: "ConnectionRequests",
                newName: "CreatedOn");

            migrationBuilder.RenameColumn(
                name: "ModifiedAt",
                table: "ConnectionRequests",
                newName: "ModifiedOn");

            migrationBuilder.RenameColumn(
                name: "CreatedAt",
                table: "AgentContexts",
                newName: "CreatedOn");

            migrationBuilder.RenameColumn(
                name: "ModifiedAt",
                table: "AgentContexts",
                newName: "ModifiedOn");

            migrationBuilder.RenameColumn(
                name: "CreatedAt",
                table: "Revocations",
                newName: "DateCreated");




        }
    }
}
