using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace allnews.Migrations
{
    /// <inheritdoc />
    public partial class articledetails : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "TrendingScore",
                table: "Articles",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<DateTime>(
                name: "UploadDate",
                table: "Articles",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TrendingScore",
                table: "Articles");

            migrationBuilder.DropColumn(
                name: "UploadDate",
                table: "Articles");
        }
    }
}
