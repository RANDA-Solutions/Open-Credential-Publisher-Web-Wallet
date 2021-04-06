using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace OpenCredentialPublisher.Data.Contexts.Migrations
{
    public partial class HttpClientLogging : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "HttpClientLogs",
                columns: table => new
                {
                    HttpClientLogId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    User = table.Column<string>(nullable: true),
                    Machine = table.Column<string>(nullable: true),
                    RequestIpAddress = table.Column<string>(nullable: true),
                    RequestContentType = table.Column<string>(nullable: true),
                    RequestContentBody = table.Column<string>(nullable: true),
                    RequestUri = table.Column<string>(nullable: true),
                    RequestMethod = table.Column<string>(nullable: true),
                    RequestRouteTemplate = table.Column<string>(nullable: true),
                    RequestRouteData = table.Column<string>(nullable: true),
                    RequestHeaders = table.Column<string>(nullable: true),
                    RequestTimestamp = table.Column<DateTime>(nullable: true),
                    ResponseContentType = table.Column<string>(nullable: true),
                    ResponseContentBody = table.Column<string>(nullable: true),
                    ResponseStatusCode = table.Column<int>(nullable: true),
                    ResponseHeaders = table.Column<string>(nullable: true),
                    ResponseTimestamp = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HttpClientLogs", x => x.HttpClientLogId);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "HttpClientLogs");
        }
    }
}
