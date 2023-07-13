using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace OpenCredentialPublisher.Data.Contexts.Migrations
{
    public partial class LastLoggedIn : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "LastLoggedInDate",
                table: "AspNetUsers",
                type: "datetime2",
                nullable: true);

            migrationBuilder.Sql(@"
                create VIEW vwUsersNeverLoggedIn
                as
                SELECT 
	                Id,
                    UserName,
                    Email,
                    PhoneNumber,
                    LockoutEnd,
                    LockoutEnabled,
                    AccessFailedCount,
                    CreatedDate
                FROM 
	                dbo.AspNetUsers
                WHERE LastLoggedInDate IS NULL AND EmailConfirmed = 0
            ");

            migrationBuilder.Sql(@"
                CREATE VIEW vwEmailedCredentials
                as
                SELECT 
	                U.Id AS [UserId],
	                U.Email AS [From],
	                M.Recipient AS [To], 
	                M.Subject,
	                C.ClrId,
	                C.Name AS [CLR Name],
	                M.CreatedAt AS [Sent],
	                L.DisplayCount 
                FROM dbo.Messages M
	                JOIN dbo.Shares S ON S.Id = M.ShareId
	                JOIN dbo.Links L ON L.Id = S.LinkId
	                JOIN dbo.AspNetUsers U ON U.Id = L.UserId
	                JOIN cred.Clrs C ON C.ClrId = L.ClrForeignKey
                WHERE M.ShareId IS NOT NULL 
            ");

            migrationBuilder.Sql(@"
                CREATE VIEW vwAccountEmails
                AS
                SELECT 
	                U.Id AS UserId,
	                M.Id AS MessageId,
                    M.Recipient,
                    M.Body,
                    M.Subject,
                    M.SendAttempts,
                    M.StatusId,
                    M.CreatedAt,
                    M.ShareId,
                    M.ProofRequestId,
                    M.IsDeleted,
                    M.ModifiedAt
                FROM 
	                dbo.Messages M
	                JOIN dbo.AspNetUsers U ON U.Email = M.Recipient
                WHERE 
	                ShareId IS NULL 
	                AND ProofRequestId IS NULL
            ");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LastLoggedInDate",
                table: "AspNetUsers");
        }
    }
}
