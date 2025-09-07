using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace RestaurantSys.Controllers
{
    public class LogoutController : Controller
    {
        [Authorize(AuthenticationSchemes = "MemberLogin")]
        public async Task<IActionResult> Logout()
        {
            // 清除所有的登入 Cookie
            await HttpContext.SignOutAsync("MemberLogin");

            // 登出後導向首頁或菜單頁
            return RedirectToAction("Index", "Menu");
        }
    }
}
