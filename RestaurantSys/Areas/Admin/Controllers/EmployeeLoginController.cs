using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RestaurantSys.Access.Data;
using RestaurantSys.Areas.Admin.ViewModels;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace RestaurantSys.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class EmployeeLoginController : Controller
    {
        private readonly RestaurantSysContext _context;
        public EmployeeLoginController(RestaurantSysContext context)
        {
            _context = context;
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

            var hashedPassword = ComputeSha256Hash(model.Password);

            var employee = await _context.Employee.FirstOrDefaultAsync(
                e => e.EmployeeID == model.EmployeeID && e.Password == hashedPassword
            );

            if (employee != null)
            {
                // 判斷角色，雖然現在都導向到 Admin，但保留這個邏輯有利於未來擴充
                string role = employee.EmployeeID.StartsWith("Admin") ? "Admin" : "Backend";

                var claims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, employee.EmployeeID),
            new Claim(ClaimTypes.Role, role),
            new Claim(ClaimTypes.Name, employee.EName)
        };

                var claimsIdentity = new ClaimsIdentity(claims, "EmployeeLogin");
                await HttpContext.SignInAsync("EmployeeLogin", new ClaimsPrincipal(claimsIdentity));

                // 所有登入成功的用戶都導向到 Admin Area 的首頁
                return RedirectToAction("Index", "Home", new { area = "Admin" });
            }

            ViewData["Error"] = "帳號或密碼錯誤。";
            return View(model);
        }
        // 你提供的 SHA256 雜湊函式
        private static string ComputeSha256Hash(string rawData)
        {
            using (SHA256 sha256Hash = SHA256.Create())
            {
                byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(rawData));
                StringBuilder builder = new StringBuilder();
                for (int i = 0; i < bytes.Length; i++)
                {
                    builder.Append(bytes[i].ToString("x2"));
                }
                return builder.ToString();
            }
        }

    }
}
