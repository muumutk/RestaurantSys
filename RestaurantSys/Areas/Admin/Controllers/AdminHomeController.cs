using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RestaurantSys.Access.Data;
using RestaurantSys.Areas.Admin.Services;
using RestaurantSys.Models;
using System.Diagnostics;
using System.Security.Claims;

namespace RestaurantSys.Areas.Admin.Controllers
{
    [Area("Admin")]

    public class AdminHomeController : Controller
    {
        private readonly ILogger<AdminHomeController> _logger;
        private readonly InventoryWarningService _warningService;
        private readonly RestaurantSysContext _context;

        public AdminHomeController(ILogger<AdminHomeController> logger, InventoryWarningService inventoryWarningService,RestaurantSysContext restaurantSysContext)
        {
            _logger = logger;
            _warningService = inventoryWarningService;
            _context = restaurantSysContext;
        }

        public async Task<IActionResult> Index() // 在這裡加上 async
        {
            // 從 Session 讀取 JSON 字串
            var warningsJson = HttpContext.Session.GetString("ExpiringWarnings");

            List<string> warnings = new();
            if (!string.IsNullOrEmpty(warningsJson))
            {
                // 將 JSON 字串還原為 List<string>
                warnings = System.Text.Json.JsonSerializer.Deserialize<List<string>>(warningsJson);

                // 為了確保警示只顯示一次，讀取後可以將它從 Session 移除
                HttpContext.Session.Remove("ExpiringWarnings");
            }

            ViewBag.InventoryWarnings = warnings;

            //計算低庫存
            var lowStockCount = await _context.Stock.CountAsync(s => s.SafeStock > s.CurrentStock);
            ViewBag.LowStockCount = lowStockCount;

            // 計算在職員工總數
            var employeeComunt = await _context.Employee.CountAsync(e => e.IsEmployed == true);
            ViewBag.EmployeeCount = employeeComunt;

            // 計算菜單品項總數
            var menuCount = await _context.Dish.CountAsync(d => d.IsActive == true);
            ViewBag.MenuCount = menuCount;

            return View();
        }


        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
