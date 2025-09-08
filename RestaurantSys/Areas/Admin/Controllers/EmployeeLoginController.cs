using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RestaurantSys.Access.Data;
using RestaurantSys.Areas.Admin.Services;
using RestaurantSys.Areas.Admin.ViewModels;
using RestaurantSys.Services;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;

namespace RestaurantSys.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class EmployeeLoginController : Controller
    {
        private readonly RestaurantSysContext _context;
        private readonly InventoryWarningService _inventoryWarningService;

        public EmployeeLoginController(RestaurantSysContext context , InventoryWarningService inventoryWarningService)
        {
            _context = context;
            _inventoryWarningService = inventoryWarningService;
        }

        public IActionResult Login()
        {
            return View();
        }

        // POST: /EmployeeLogin/Login
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(VMEmployeeLogin model)
        {
            if (string.IsNullOrEmpty(model.EmployeeID) || string.IsNullOrEmpty(model.Password))
            {
                ViewData["Error"] = "請輸入帳號和密碼";
                return View(model);
            }

            var hashedPassword = HashService.HashPasswordSHA256(model.Password);

            var employee = await _context.Employee.FirstOrDefaultAsync(
                e => e.EmployeeID == model.EmployeeID && e.Password == hashedPassword
            );

            if (employee != null)
            {
                //// 判斷角色，不是Admin就導向Employee
                string role = employee.EmployeeID.StartsWith("Admin") ? "Admin" : "Employee";

                var claims = new List<Claim>
                    {
                        new Claim(ClaimTypes.NameIdentifier, employee.EmployeeID),
                        new Claim(ClaimTypes.Role, role),
                        new Claim(ClaimTypes.Name, employee.EName)
                    };

                var claimsIdentity = new ClaimsIdentity(claims, "EmployeeLogin");
                await HttpContext.SignInAsync("EmployeeLogin", new ClaimsPrincipal(claimsIdentity));

                //成功登入後，呼叫服務並將結果儲存到 TempData
                var warnings = await _inventoryWarningService.CheckAndLogExpiringBatchesAsync(employee.EmployeeID);
                if (warnings.Any())
                {
                    HttpContext.Session.SetString("ExpiringWarnings", System.Text.Json.JsonSerializer.Serialize(warnings));
                }

                // 所有登入成功的用戶都導向到 Admin Area 的首頁
                return RedirectToAction("Index", "AdminHome", new { area = "Admin" });
            }

            ViewData["Error"] = "帳號或密碼錯誤。";
            return View(model);
        }


    }
}
