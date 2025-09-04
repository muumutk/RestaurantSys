using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RestaurantSys.Access.Migrations
{
    /// <inheritdoc />
    public partial class dishCategoryModel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CategoryID",
                table: "Dish",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "DishCategoryID",
                table: "Dish",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "DishCategories",
                columns: table => new
                {
                    DishCategoryID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DishCategoryName = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DishCategoryID", x => x.DishCategoryID);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Dish_DishCategoryID",
                table: "Dish",
                column: "DishCategoryID");

            migrationBuilder.AddForeignKey(
                name: "FK_Dish_DishCategories_DishCategoryID",
                table: "Dish",
                column: "DishCategoryID",
                principalTable: "DishCategories",
                principalColumn: "DishCategoryID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Dish_DishCategories_DishCategoryID",
                table: "Dish");

            migrationBuilder.DropTable(
                name: "DishCategories");

            migrationBuilder.DropIndex(
                name: "IX_Dish_DishCategoryID",
                table: "Dish");

            migrationBuilder.DropColumn(
                name: "CategoryID",
                table: "Dish");

            migrationBuilder.DropColumn(
                name: "DishCategoryID",
                table: "Dish");
        }
    }
}
