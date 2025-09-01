using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RestaurantSys.Access.Migrations
{
    /// <inheritdoc />
    public partial class deleteWrongColumn : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_StockBatch_Stock_StockItemID",
                table: "StockBatch");

            migrationBuilder.DropIndex(
                name: "IX_StockBatch_StockItemID",
                table: "StockBatch");

            migrationBuilder.DropColumn(
                name: "StockItemID",
                table: "StockBatch");

            migrationBuilder.CreateIndex(
                name: "IX_StockBatch_ItemID",
                table: "StockBatch",
                column: "ItemID");

            migrationBuilder.AddForeignKey(
                name: "FK_StockBatch_Stock_ItemID",
                table: "StockBatch",
                column: "ItemID",
                principalTable: "Stock",
                principalColumn: "ItemID",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_StockBatch_Stock_ItemID",
                table: "StockBatch");

            migrationBuilder.DropIndex(
                name: "IX_StockBatch_ItemID",
                table: "StockBatch");

            migrationBuilder.AddColumn<int>(
                name: "StockItemID",
                table: "StockBatch",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_StockBatch_StockItemID",
                table: "StockBatch",
                column: "StockItemID");

            migrationBuilder.AddForeignKey(
                name: "FK_StockBatch_Stock_StockItemID",
                table: "StockBatch",
                column: "StockItemID",
                principalTable: "Stock",
                principalColumn: "ItemID",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
