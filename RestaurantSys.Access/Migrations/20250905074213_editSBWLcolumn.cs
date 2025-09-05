using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RestaurantSys.Access.Migrations
{
    /// <inheritdoc />
    public partial class editSBWLcolumn : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "EmployeeID",
                table: "StockBatchWarningLog",
                type: "nvarchar(8)",
                nullable: true);

            migrationBuilder.DropForeignKey(
                name: "FK_StockBatchWarningLog_StockBatch_BatchID",
                table: "StockBatchWarningLog");

            migrationBuilder.AlterColumn<string>(
                name: "EmployeeID",
                table: "StockBatchWarningLog",
                type: "nvarchar(8)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(8)");

            migrationBuilder.AlterColumn<int>(
                name: "BatchID",
                table: "StockBatchWarningLog",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_StockBatchWarningLog_Employee_EmployeeID",
                table: "StockBatchWarningLog",
                column: "EmployeeID",
                principalTable: "Employee",
                principalColumn: "EmployeeID");

            migrationBuilder.AddForeignKey(
                name: "FK_StockBatchWarningLog_StockBatch_BatchID",
                table: "StockBatchWarningLog",
                column: "BatchID",
                principalTable: "StockBatch",
                principalColumn: "BatchID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_StockBatchWarningLog_Employee_EmployeeID",
                table: "StockBatchWarningLog");

            migrationBuilder.DropForeignKey(
                name: "FK_StockBatchWarningLog_StockBatch_BatchID",
                table: "StockBatchWarningLog");

            migrationBuilder.AlterColumn<string>(
                name: "EmployeeID",
                table: "StockBatchWarningLog",
                type: "nvarchar(8)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(8)",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "BatchID",
                table: "StockBatchWarningLog",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_StockBatchWarningLog_Employee_EmployeeID",
                table: "StockBatchWarningLog",
                column: "EmployeeID",
                principalTable: "Employee",
                principalColumn: "EmployeeID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_StockBatchWarningLog_StockBatch_BatchID",
                table: "StockBatchWarningLog",
                column: "BatchID",
                principalTable: "StockBatch",
                principalColumn: "BatchID",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
