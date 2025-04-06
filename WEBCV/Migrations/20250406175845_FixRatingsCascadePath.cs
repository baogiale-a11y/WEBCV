using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WEBCV.Migrations
{
    /// <inheritdoc />
    public partial class FixRatingsCascadePath : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Ratings_AspNetUsers_RatedUserId",
                table: "Ratings");

            migrationBuilder.AddForeignKey(
                name: "FK_Ratings_AspNetUsers_RatedUserId",
                table: "Ratings",
                column: "RatedUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Ratings_AspNetUsers_RatedUserId",
                table: "Ratings");

            migrationBuilder.AddForeignKey(
                name: "FK_Ratings_AspNetUsers_RatedUserId",
                table: "Ratings",
                column: "RatedUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
