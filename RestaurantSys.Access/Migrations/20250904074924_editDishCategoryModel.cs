using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RestaurantSys.Access.Migrations
{
    /// <inheritdoc />
    public partial class editDishCategoryModel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Dish_DishCategories_DishCategoryID",
                table: "Dish");

            migrationBuilder.RenameTable(
                name: "DishCategories",
                newName: "DishCategory");

            migrationBuilder.AddForeignKey(
                name: "FK_Dish_DishCategory_DishCategoryID",
                table: "Dish",
                column: "DishCategoryID",
                principalTable: "DishCategory",
                principalColumn: "DishCategoryID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Dish_DishCategory_DishCategoryID",
                table: "Dish");

            migrationBuilder.RenameTable(
                name: "DishCategory",
                newName: "DishCategories");

            migrationBuilder.AddForeignKey(
                name: "FK_Dish_DishCategories_DishCategoryID",
                table: "Dish",
                column: "DishCategoryID",
                principalTable: "DishCategories",
                principalColumn: "DishCategoryID");
        }
    }
}
