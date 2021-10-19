using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Migrations.Operations;
using Microsoft.EntityFrameworkCore.Migrations.Operations.Builders;

namespace OpenCredentialPublisher.Data.Contexts.Migrations
{
    public partial class BreakoutAssertions : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"EXEC sp_rename 'PK_ClrAssertions', 'PK_OldClrAssertions';");

            migrationBuilder.DropForeignKey(
                name: "FK_ClrAssertions_Clrs_ClrForeignKey",
                table: "ClrAssertions");

            migrationBuilder.DropIndex(
                name: "IX_ClrAssertions_ClrForeignKey",
                table: "ClrAssertions");

            migrationBuilder.RenameTable(
                name: "ClrAssertions",
                newName: "OldClrAssertions");

            migrationBuilder.CreateTable(
                name: "ClrAssertions",
                columns: table => new
                {
                    ClrAssertionId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ClrId = table.Column<int>(type: "int", nullable: false),
                    AssertionId = table.Column<int>(type: "int", nullable: false),
                    Order = table.Column<int>(type: "int", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ClrAssertions", x => x.ClrAssertionId);
                });


            migrationBuilder.CreateTable(
                name: "Assertions",
                columns: table => new
                {
                    AssertionId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
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
                    Context = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsSigned = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Assertions", x => x.AssertionId);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ClrAssertions_AssertionId",
                table: "ClrAssertions",
                column: "AssertionId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ClrAssertions_ClrId",
                table: "ClrAssertions",
                column: "ClrId");

            migrationBuilder.AddForeignKey(
                name: "FK_ClrAssertions_Assertions_AssertionId",
                table: "ClrAssertions",
                column: "AssertionId",
                principalTable: "Assertions",
                principalColumn: "AssertionId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ClrAssertions_Clrs_ClrId",
                table: "ClrAssertions",
                column: "ClrId",
                principalTable: "Clrs",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.Sql(@"
SET IDENTITY_INSERT dbo.Assertions ON;
INSERT INTO dbo.Assertions(
        [AssertionId]
      ,[SignedAssertion]
      ,[IsDeleted]
      ,[CreatedAt]
      ,[ModifiedAt]
      ,[Id]
      ,[Type]
      ,[Achievement]
      ,[CreditsEarned]
      ,[ActivityEndDate]
      ,[Endorsements]
      ,[Evidence]
      ,[Expires]
      ,[Image]
      ,[IssuedOn]
      ,[LicenseNumber]
      ,[Narrative]
      ,[Recipient]
      ,[Results]
      ,[RevocationReason]
      ,[Revoked]
      ,[Role]
      ,[SignedEndorsements]
      ,[Source]
      ,[ActivityStartDate]
      ,[Term]
      ,[Verification]
      ,[AdditionalProperties]
      ,[Context]
      ,[IsSigned]
)
SELECT[ClrAssertionId]
      ,[SignedAssertion]
      ,[IsDeleted]
      ,[CreatedAt]
      ,[ModifiedAt]
      ,[Id]
      ,[Type]
      ,[Achievement]
      ,[CreditsEarned]
      ,[ActivityEndDate]
      ,[Endorsements]
      ,[Evidence]
      ,[Expires]
      ,[Image]
      ,[IssuedOn]
      ,[LicenseNumber]
      ,[Narrative]
      ,[Recipient]
      ,[Results]
      ,[RevocationReason]
      ,[Revoked]
      ,[Role]
      ,[SignedEndorsements]
      ,[Source]
      ,[ActivityStartDate]
      ,[Term]
      ,[Verification]
      ,[AdditionalProperties]
      ,[Context]
      ,[IsSigned]
FROM [dbo].[OldClrAssertions];

SET IDENTITY_INSERT dbo.Assertions OFF;

--This will set the[Order] column sequentially in the same order the Assertions were originally added
-- accross the entire universe, which should still be the correct order within each CLR subset...

INSERT INTO dbo.ClrAssertions(ClrId, AssertionId, [Order], IsDeleted, CreatedAt, ModifiedAt)
SELECT OCA.ClrForeignKey, A.AssertionId, ROW_NUMBER() OVER(ORDER BY A.AssertionId ASC), 0, A.CreatedAt, A.ModifiedAt FROM dbo.Assertions A
INNER JOIN dbo.OldClrAssertions OCA ON OCA.ClrAssertionId = A.AssertionId
ORDER BY A.AssertionId"
                );
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

            migrationBuilder.DropTable(
                name: "Assertions");

            migrationBuilder.DropTable(
                name: "ClrAssertions");

            migrationBuilder.RenameTable(
                name: "OldClrAssertions",
                newName: "ClrAssertions");

            migrationBuilder.CreateIndex(
                name: "IX_ClrAssertions_ClrForeignKey",
                table: "ClrAssertions",
                column: "ClrForeignKey");

            migrationBuilder.AddForeignKey(
                name: "FK_ClrAssertions_Clrs_ClrForeignKey",
                table: "ClrAssertions",
                column: "ClrForeignKey",
                principalTable: "Clrs",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
