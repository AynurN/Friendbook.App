using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Friendbook.Data.Migrations
{
    /// <inheritdoc />
    public partial class PostAddedToPostImage : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PostImages_AspNetUsers_AppUserId",
                table: "PostImages");

            migrationBuilder.DropForeignKey(
                name: "FK_PostImages_Posts_PostId",
                table: "PostImages");

            migrationBuilder.DropIndex(
                name: "IX_PostImages_AppUserId",
                table: "PostImages");

            migrationBuilder.DropColumn(
                name: "AppUserId",
                table: "PostImages");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "PostImages");

            migrationBuilder.AlterColumn<int>(
                name: "PostId",
                table: "PostImages",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_PostImages_Posts_PostId",
                table: "PostImages",
                column: "PostId",
                principalTable: "Posts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PostImages_Posts_PostId",
                table: "PostImages");

            migrationBuilder.AlterColumn<int>(
                name: "PostId",
                table: "PostImages",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<string>(
                name: "AppUserId",
                table: "PostImages",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "PostImages",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_PostImages_AppUserId",
                table: "PostImages",
                column: "AppUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_PostImages_AspNetUsers_AppUserId",
                table: "PostImages",
                column: "AppUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_PostImages_Posts_PostId",
                table: "PostImages",
                column: "PostId",
                principalTable: "Posts",
                principalColumn: "Id");
        }
    }
}
