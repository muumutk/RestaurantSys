using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RestaurantSys.Access.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Dish",
                columns: table => new
                {
                    DishID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DishName = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    PhotoPath = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    DishPrice = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Note = table.Column<string>(type: "nvarchar(40)", maxLength: 40, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DishID", x => x.DishID);
                });

            migrationBuilder.CreateTable(
                name: "Employee",
                columns: table => new
                {
                    EmployeeID = table.Column<string>(type: "nvarchar(8)", maxLength: 8, nullable: false),
                    Name = table.Column<string>(type: "nvarchar(40)", maxLength: 40, nullable: false),
                    EmployeeTel = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    Address = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Birthday = table.Column<DateTime>(type: "date", nullable: false),
                    HireDate = table.Column<DateTime>(type: "date", nullable: false),
                    IsEmployed = table.Column<bool>(type: "bit", nullable: false),
                    Password = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StaffID", x => x.EmployeeID);
                });

            migrationBuilder.CreateTable(
                name: "Member",
                columns: table => new
                {
                    MemberID = table.Column<string>(type: "nvarchar(9)", maxLength: 9, nullable: false),
                    Name = table.Column<string>(type: "nvarchar(40)", maxLength: 40, nullable: false),
                    City = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    Address = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Birthday = table.Column<DateTime>(type: "date", nullable: false),
                    title = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    Password = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MemberID", x => x.MemberID);
                });

            migrationBuilder.CreateTable(
                name: "Supplier",
                columns: table => new
                {
                    SupplierID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SupplierName = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    ContactPerson = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    SupplierTel = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    Address = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SupplierID", x => x.SupplierID);
                });

            migrationBuilder.CreateTable(
                name: "MemberTel",
                columns: table => new
                {
                    SN = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MemTel = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    MemberID = table.Column<string>(type: "nvarchar(9)", maxLength: 9, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SN", x => x.SN);
                    table.ForeignKey(
                        name: "FK_MemberTel_Member_MemberID",
                        column: x => x.MemberID,
                        principalTable: "Member",
                        principalColumn: "MemberID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Order",
                columns: table => new
                {
                    OrderID = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    OrderDate = table.Column<DateTime>(type: "date", nullable: false),
                    PickUpTime = table.Column<DateTime>(type: "date", nullable: false),
                    Note = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    MemberID = table.Column<string>(type: "nvarchar(9)", nullable: false),
                    EmployeefID = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    EmployeeID = table.Column<string>(type: "nvarchar(8)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrderID", x => x.OrderID);
                    table.ForeignKey(
                        name: "FK_Order_Employee_EmployeeID",
                        column: x => x.EmployeeID,
                        principalTable: "Employee",
                        principalColumn: "EmployeeID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Order_Member_MemberID",
                        column: x => x.MemberID,
                        principalTable: "Member",
                        principalColumn: "MemberID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Stock",
                columns: table => new
                {
                    ItemID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ItemName = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    CurrentStock = table.Column<int>(type: "int", nullable: false),
                    SafeStock = table.Column<int>(type: "int", nullable: false),
                    Unit = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    ItemPrice = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false, defaultValue: true),
                    SupplierID = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StockID", x => x.ItemID);
                    table.ForeignKey(
                        name: "FK_Stock_Supplier_SupplierID",
                        column: x => x.SupplierID,
                        principalTable: "Supplier",
                        principalColumn: "SupplierID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "OrderDetail",
                columns: table => new
                {
                    OrderID = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    DishID = table.Column<int>(type: "int", nullable: false),
                    Price = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    GetTime = table.Column<DateTime>(type: "date", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrderDetail", x => new { x.OrderID, x.DishID });
                    table.ForeignKey(
                        name: "FK_OrderDetail_Dish_DishID",
                        column: x => x.DishID,
                        principalTable: "Dish",
                        principalColumn: "DishID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_OrderDetail_Order_OrderID",
                        column: x => x.OrderID,
                        principalTable: "Order",
                        principalColumn: "OrderID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "DishIngredient",
                columns: table => new
                {
                    DishID = table.Column<int>(type: "int", nullable: false),
                    ItemID = table.Column<int>(type: "int", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false, defaultValue: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DishIngredient", x => new { x.DishID, x.ItemID });
                    table.ForeignKey(
                        name: "FK_DishIngredient_Dish_DishID",
                        column: x => x.DishID,
                        principalTable: "Dish",
                        principalColumn: "DishID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DishIngredient_Stock_ItemID",
                        column: x => x.ItemID,
                        principalTable: "Stock",
                        principalColumn: "ItemID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_DishIngredient_ItemID",
                table: "DishIngredient",
                column: "ItemID");

            migrationBuilder.CreateIndex(
                name: "IX_MemberTel_MemberID",
                table: "MemberTel",
                column: "MemberID");

            migrationBuilder.CreateIndex(
                name: "IX_Order_EmployeeID",
                table: "Order",
                column: "EmployeeID");

            migrationBuilder.CreateIndex(
                name: "IX_Order_MemberID",
                table: "Order",
                column: "MemberID");

            migrationBuilder.CreateIndex(
                name: "IX_OrderDetail_DishID",
                table: "OrderDetail",
                column: "DishID");

            migrationBuilder.CreateIndex(
                name: "IX_Stock_SupplierID",
                table: "Stock",
                column: "SupplierID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DishIngredient");

            migrationBuilder.DropTable(
                name: "MemberTel");

            migrationBuilder.DropTable(
                name: "OrderDetail");

            migrationBuilder.DropTable(
                name: "Stock");

            migrationBuilder.DropTable(
                name: "Dish");

            migrationBuilder.DropTable(
                name: "Order");

            migrationBuilder.DropTable(
                name: "Supplier");

            migrationBuilder.DropTable(
                name: "Employee");

            migrationBuilder.DropTable(
                name: "Member");
        }
    }
}
