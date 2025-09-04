using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RestaurantSys.Access.Migrations
{
    /// <inheritdoc />
    public partial class editColumn : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Dish_DishCategory_DishCategoryID",
                table: "Dish");

            migrationBuilder.DropColumn(
                name: "CategoryID",
                table: "Dish");

            migrationBuilder.AlterColumn<int>(
                name: "DishCategoryID",
                table: "Dish",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.Sql("UPDATE Dish SET DishCategoryID = 1 WHERE DishCategoryID = 0");

            migrationBuilder.AddForeignKey(
                name: "FK_Dish_DishCategory_DishCategoryID",
                table: "Dish",
                column: "DishCategoryID",
                principalTable: "DishCategory",
                principalColumn: "DishCategoryID",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Dish_DishCategory_DishCategoryID",
                table: "Dish");

            migrationBuilder.AlterColumn<int>(
                name: "DishCategoryID",
                table: "Dish",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<int>(
                name: "CategoryID",
                table: "Dish",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddForeignKey(
                name: "FK_Dish_DishCategory_DishCategoryID",
                table: "Dish",
                column: "DishCategoryID",
                principalTable: "DishCategory",
                principalColumn: "DishCategoryID");
        }
    }
}
