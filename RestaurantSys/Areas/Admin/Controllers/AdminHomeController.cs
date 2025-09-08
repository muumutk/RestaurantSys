using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
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

        public AdminHomeController(ILogger<AdminHomeController> logger, InventoryWarningService inventoryWarningService)
        {
            _logger = logger;
            _warningService = inventoryWarningService;
        }

        public IActionResult Index()
        {
            // �q Session Ū�� JSON �r��
            var warningsJson = HttpContext.Session.GetString("ExpiringWarnings");

            List<string> warnings = new();
            if (!string.IsNullOrEmpty(warningsJson))
            {
                // �N JSON �r���٭쬰 List<string>
                warnings = System.Text.Json.JsonSerializer.Deserialize<List<string>>(warningsJson);

                // ���F�T�Oĵ�ܥu��ܤ@���AŪ����i�H�N���q Session ����
                HttpContext.Session.Remove("ExpiringWarnings");
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
