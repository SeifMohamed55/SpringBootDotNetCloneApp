using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SpringBootCloneApp.Migrations
{
    /// <inheritdoc />
    public partial class AddingIndexToFoodNameCol : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_food_name",
                table: "food",
                column: "name");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_food_name",
                table: "food");
        }
    }
}
