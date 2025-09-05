using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RestaurantSys.Access.Migrations
{
    /// <inheritdoc />
    public partial class newModel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "StockBatchWarningLog",
                columns: table => new
                {
                    StockBatchWarningLogID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    WarningSentDate = table.Column<DateTime>(type: "datetime", nullable: false),
                    BatchID = table.Column<int>(type: "int", nullable: false),
                    StockBatchBatchID = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StockBatchWarningLogID", x => x.StockBatchWarningLogID);
                    table.ForeignKey(
                        name: "FK_StockBatchWarningLog_StockBatch_StockBatchBatchID",
                        column: x => x.StockBatchBatchID,
                        principalTable: "StockBatch",
                        principalColumn: "BatchID");
                });

            migrationBuilder.CreateIndex(
                name: "IX_StockBatchWarningLog_StockBatchBatchID",
                table: "StockBatchWarningLog",
                column: "StockBatchBatchID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "StockBatchWarningLog");
        }
    }
}
