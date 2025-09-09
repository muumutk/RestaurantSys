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

        public async Task<IActionResult> Index() // �b�o�̥[�W async
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

            //�p��C�w�s
            var lowStockCount = await _context.Stock.CountAsync(s => s.SafeStock > s.CurrentStock);
            ViewBag.LowStockCount = lowStockCount;

            // �p��b¾���u�`��
            var employeeComunt = await _context.Employee.CountAsync(e => e.IsEmployed == true);
            ViewBag.EmployeeCount = employeeComunt;

            // �p����~���`��
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
