using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RestaurantSys.Access.Migrations
{
    /// <inheritdoc />
    public partial class editFK : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_StockBatchWarningLog_StockBatch_StockBatchBatchID",
                table: "StockBatchWarningLog");

            migrationBuilder.DropIndex(
                name: "IX_StockBatchWarningLog_StockBatchBatchID",
                table: "StockBatchWarningLog");

            migrationBuilder.DropColumn(
                name: "StockBatchBatchID",
                table: "StockBatchWarningLog");

            migrationBuilder.CreateIndex(
                name: "IX_StockBatchWarningLog_BatchID",
                table: "StockBatchWarningLog",
                column: "BatchID");

            migrationBuilder.AddForeignKey(
                name: "FK_StockBatchWarningLog_StockBatch_BatchID",
                table: "StockBatchWarningLog",
                column: "BatchID",
                principalTable: "StockBatch",
                principalColumn: "BatchID",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_StockBatchWarningLog_StockBatch_BatchID",
                table: "StockBatchWarningLog");

            migrationBuilder.DropIndex(
                name: "IX_StockBatchWarningLog_BatchID",
                table: "StockBatchWarningLog");

            migrationBuilder.AddColumn<int>(
                name: "StockBatchBatchID",
                table: "StockBatchWarningLog",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_StockBatchWarningLog_StockBatchBatchID",
                table: "StockBatchWarningLog",
                column: "StockBatchBatchID");

            migrationBuilder.AddForeignKey(
                name: "FK_StockBatchWarningLog_StockBatch_StockBatchBatchID",
                table: "StockBatchWarningLog",
                column: "StockBatchBatchID",
                principalTable: "StockBatch",
                principalColumn: "BatchID");
        }
    }
}
