using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SpringBootCloneApp.Migrations
{
    /// <inheritdoc />
    public partial class AddingPrimaryKeyToTagsAndOrigins : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_food_tags_food_id",
                table: "food_tags");

            migrationBuilder.DropIndex(
                name: "IX_food_origins_food_id",
                table: "food_origins");

            migrationBuilder.RenameColumn(
                name: "tags",
                table: "food_tags",
                newName: "tag");

            migrationBuilder.RenameColumn(
                name: "origins",
                table: "food_origins",
                newName: "origin");

            migrationBuilder.AddPrimaryKey(
                name: "PK_food_tags",
                table: "food_tags",
                columns: new[] { "food_id", "tag" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_food_origins",
                table: "food_origins",
                columns: new[] { "food_id", "origin" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_food_tags",
                table: "food_tags");

            migrationBuilder.DropPrimaryKey(
                name: "PK_food_origins",
                table: "food_origins");

            migrationBuilder.RenameColumn(
                name: "tag",
                table: "food_tags",
                newName: "tags");

            migrationBuilder.RenameColumn(
                name: "origin",
                table: "food_origins",
                newName: "origins");

            migrationBuilder.CreateIndex(
                name: "IX_food_tags_food_id",
                table: "food_tags",
                column: "food_id");

            migrationBuilder.CreateIndex(
                name: "IX_food_origins_food_id",
                table: "food_origins",
                column: "food_id");
        }
    }
}
