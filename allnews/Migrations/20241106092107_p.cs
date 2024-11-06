using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace allnews.Migrations
{
    /// <inheritdoc />
    public partial class p : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "SubArticleId",
                table: "Publishers",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Publishers_SubArticleId",
                table: "Publishers",
                column: "SubArticleId");

            migrationBuilder.AddForeignKey(
                name: "FK_Publishers_SubArticles_SubArticleId",
                table: "Publishers",
                column: "SubArticleId",
                principalTable: "SubArticles",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Publishers_SubArticles_SubArticleId",
                table: "Publishers");

            migrationBuilder.DropIndex(
                name: "IX_Publishers_SubArticleId",
                table: "Publishers");

            migrationBuilder.DropColumn(
                name: "SubArticleId",
                table: "Publishers");
        }
    }
}
