using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RestaurantSys.Access.Migrations
{
    /// <inheritdoc />
    public partial class newDaliyStockUsage : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "DailyStockUsage",
                columns: table => new
                {
                    UsageID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DishID = table.Column<int>(type: "int", nullable: false),
                    ItemID = table.Column<int>(type: "int", nullable: false),
                    QuantityUsed = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    UsageDate = table.Column<DateTime>(type: "date", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UsageID", x => x.UsageID);
                    table.ForeignKey(
                        name: "FK_DailyStockUsage_Dish_DishID",
                        column: x => x.DishID,
                        principalTable: "Dish",
                        principalColumn: "DishID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DailyStockUsage_Stock_ItemID",
                        column: x => x.ItemID,
                        principalTable: "Stock",
                        principalColumn: "ItemID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_DailyStockUsage_DishID",
                table: "DailyStockUsage",
                column: "DishID");

            migrationBuilder.CreateIndex(
                name: "IX_DailyStockUsage_ItemID",
                table: "DailyStockUsage",
                column: "ItemID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DailyStockUsage");
        }
    }
}
