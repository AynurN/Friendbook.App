using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Friendbook.Data.Migrations
{
    /// <inheritdoc />
    public partial class isVisibleAddedToTheAppUser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsVisible",
                table: "AspNetUsers",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsVisible",
                table: "AspNetUsers");
        }
    }
}
