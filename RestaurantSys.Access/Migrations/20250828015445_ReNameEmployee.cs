using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RestaurantSys.Access.Migrations
{
    /// <inheritdoc />
    public partial class ReNameEmployee : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // 1. 先移除外鍵
            migrationBuilder.DropForeignKey(
                name: "FK_Order_Employee_EmployeeID",
                table: "Order");

            // 2. 再移除主鍵
            migrationBuilder.DropPrimaryKey(
                name: "PK_StaffID",
                table: "Employee");

            // 3. 刪除舊欄位
            migrationBuilder.DropColumn(
                name: "EmployeefID",
                table: "Order");

            // 4. 新增新主鍵
            migrationBuilder.AddPrimaryKey(
                name: "PK_EmployeeID",
                table: "Employee",
                column: "EmployeeID");

            // 5. 重新建立外鍵
            migrationBuilder.AddForeignKey(
                name: "FK_Order_Employee_EmployeeID",
                table: "Order",
                column: "EmployeeID",
                principalTable: "Employee",
                principalColumn: "EmployeeID",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // 1. 先移除外鍵
            migrationBuilder.DropForeignKey(
                name: "FK_Order_Employee_EmployeeID",
                table: "Order");

            // 2. 移除新主鍵
            migrationBuilder.DropPrimaryKey(
                name: "PK_EmployeeID",
                table: "Employee");

            // 3. 還原舊欄位
            migrationBuilder.AddColumn<string>(
                name: "EmployeefID",
                table: "Order",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            // 4. 新增舊主鍵
            migrationBuilder.AddPrimaryKey(
                name: "PK_StaffID",
                table: "Employee",
                column: "EmployeeID");

            // 5. 重新建立外鍵
            migrationBuilder.AddForeignKey(
                name: "FK_Order_Employee_EmployeeID",
                table: "Order",
                column: "EmployeeID",
                principalTable: "Employee",
                principalColumn: "EmployeeID",
                onDelete: ReferentialAction.Cascade);
        }
    }
}