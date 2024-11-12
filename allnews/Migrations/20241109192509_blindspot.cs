using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace allnews.Migrations
{
    /// <inheritdoc />
    public partial class blindspot : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsBlindSpot",
                table: "Articles",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsBlindSpot",
                table: "Articles");
        }
    }
}
