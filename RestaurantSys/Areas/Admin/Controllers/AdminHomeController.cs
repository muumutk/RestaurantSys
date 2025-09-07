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
            // ���o�ثe�n�J���uID�]�̧A���n�J�޿�վ�^
            string? employeeId = User.Identity?.Name; // �αq Claims/Session ���o
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
