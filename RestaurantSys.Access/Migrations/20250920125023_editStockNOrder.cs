using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RestaurantSys.Access.Migrations
{
    /// <inheritdoc />
    public partial class editStockNOrder : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Order_Employee_EmployeeID",
                table: "Order");

            migrationBuilder.DropIndex(
                name: "IX_Order_EmployeeID",
                table: "Order");

            migrationBuilder.DropColumn(
                name: "EmployeeID",
                table: "Order");

            migrationBuilder.AlterColumn<decimal>(
                name: "SafeStock",
                table: "Stock",
                type: "decimal(18,2)",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "CurrentStock",
                table: "Stock",
                type: "decimal(18,2)",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "SafeStock",
                table: "Stock",
                type: "int",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "CurrentStock",
                table: "Stock",
                type: "int",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "EmployeeID",
                table: "Order",
                type: "nvarchar(8)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Order_EmployeeID",
                table: "Order",
                column: "EmployeeID");

            migrationBuilder.AddForeignKey(
                name: "FK_Order_Employee_EmployeeID",
                table: "Order",
                column: "EmployeeID",
                principalTable: "Employee",
                principalColumn: "EmployeeID");
        }
    }
}
