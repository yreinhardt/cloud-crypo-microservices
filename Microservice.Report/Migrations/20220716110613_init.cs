using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Microservice.Report.Migrations
{
    public partial class init : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "report",
                columns: table => new
                {
                    ReportId = table.Column<Guid>(type: "uuid", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    PortfolioId = table.Column<Guid>(type: "uuid", nullable: false),
                    UserId = table.Column<Guid>(type: "uuid", nullable: false),
                    Portfolioname = table.Column<string>(type: "text", nullable: false),
                    WinLossRatioAbsolute = table.Column<float>(type: "real", nullable: false),
                    WinLossRatioPercentage = table.Column<float>(type: "real", nullable: false),
                    AssetCount = table.Column<int>(type: "integer", nullable: false),
                    HighestPerformanceAsset = table.Column<string>(type: "text", nullable: false),
                    LowestPerformanceAsset = table.Column<string>(type: "text", nullable: false),
                    AssetList = table.Column<List<string>>(type: "text[]", nullable: false),
                    AssetPerformanceAbsolute = table.Column<List<float>>(type: "real[]", nullable: false),
                    AssetPerformancePercentage = table.Column<List<float>>(type: "real[]", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_report", x => x.ReportId);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "report");
        }
    }
}
