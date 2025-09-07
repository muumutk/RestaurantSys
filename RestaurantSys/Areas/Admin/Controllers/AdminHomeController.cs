using Microsoft.AspNetCore.Mvc;
using RestaurantSys.Areas.Admin.Services;
using RestaurantSys.Models;
using System.Diagnostics;

namespace RestaurantSys.Areas.Admin.Controllers
{
    [Area("Admin")]

    public class AdminHomeController : Controller
    {
        private readonly ILogger<AdminHomeController> _logger;
        private readonly InventoryWarningService _warningService;

        public AdminHomeController(ILogger<AdminHomeController> logger, InventoryWarningService inventoryWarningService)
        {
            _logger = logger;
            _warningService = inventoryWarningService;
        }

        public async Task<IActionResult> Index()
        {
            // 取得目前登入員工ID（依你的登入邏輯調整）
            string? employeeId = User.Identity?.Name; // 或從 Claims/Session 取得
            List<string> warnings = new();
            if (!string.IsNullOrEmpty(employeeId))
            {
                warnings = await _warningService.CheckAndLogExpiringBatchesAsync(employeeId);
            }
            ViewBag.InventoryWarnings = warnings;
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
