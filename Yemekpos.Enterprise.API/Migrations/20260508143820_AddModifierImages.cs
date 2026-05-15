using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Yemekpos.Enterprise.API.Migrations
{
    /// <inheritdoc />
    public partial class AddModifierImages : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ImageUrl",
                table: "Modifiers",
                type: "text",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ImageUrl",
                table: "Modifiers");
        }
    }
}
