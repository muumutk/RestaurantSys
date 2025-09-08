using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RestaurantSys.Access.Data;
using RestaurantSys.Models;
using RestaurantSys.Services;
using RestaurantSys.ViewModels;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace RestaurantSys.Controllers
{
    public class MemberLoginController : Controller
    {
        private readonly RestaurantSysContext _context;

        public MemberLoginController(RestaurantSysContext context)
        {
            _context = context;
        }

        // GET: /MemberLogin/Login
        public IActionResult Login()
        {
            return View();
        }

        // POST: /MemberLogin/Login
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(VMMemberLogin model)
        {
            if (string.IsNullOrEmpty(model.EmailOrPhone) || string.IsNullOrEmpty(model.Password))
            {
                ViewData["Error"] = "請輸入帳號和密碼";
                return View(model);
            }

            var hashedPassword = HashService.HashPasswordSHA256(model.Password);

            var user = await _context.Member.FirstOrDefaultAsync(
                m => (m.MemberTel == model.EmailOrPhone || m.MEmail == model.EmailOrPhone) && m.Password == hashedPassword
            );

            if (user != null)
            {
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.NameIdentifier, user.MemberID),
                    new Claim(ClaimTypes.Role, "Member"),
                    new Claim(ClaimTypes.Name, user.Name)
                };

                var claimsIdentity = new ClaimsIdentity(claims, "MemberLogin");
                await HttpContext.SignInAsync("MemberLogin", new ClaimsPrincipal(claimsIdentity));

                return RedirectToAction("Index", "Members", new { area = "User" });
            }

            ViewData["Error"] = "帳號或密碼錯誤。";
            return View(model);
        }

    }
    }