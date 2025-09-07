using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace RestaurantSys.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class EmployeeLogoutController : Controller
    {
        public async Task<IActionResult> Logout()
        {
            // 只清除 Employee 的登入 Cookie
            await HttpContext.SignOutAsync("EmployeeLogin");

            // 登出後導向員工登入頁面
            return RedirectToAction("Login", "EmployeeLogin");
        }
    }
}