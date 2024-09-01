using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SpringBootCloneApp.Migrations
{
    /// <inheritdoc />
    public partial class AddingIndexesToOriginAndTag : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_food_tags_tag",
                table: "food_tags",
                column: "tag");

            migrationBuilder.CreateIndex(
                name: "IX_food_origins_origin",
                table: "food_origins",
                column: "origin");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_food_tags_tag",
                table: "food_tags");

            migrationBuilder.DropIndex(
                name: "IX_food_origins_origin",
                table: "food_origins");
        }
    }
}
