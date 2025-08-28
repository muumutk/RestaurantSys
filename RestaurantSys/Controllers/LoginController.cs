using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RestaurantSys.Access.Data;
using RestaurantSys.Models;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace RestaurantSys.Controllers
{
    public class LoginController : Controller
    {
        private readonly RestaurantSysContext _context;

        public LoginController(RestaurantSysContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Login()
        {
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(Member member)
        {
            
                if (member == null || string.IsNullOrEmpty(member.MemberTel) || string.IsNullOrEmpty(member.Password))
                {
                    ViewData["Error"] = "請輸入帳號和密碼";
                    return View();
                }

                var user = await _context.Member.FirstOrDefaultAsync(m => m.MemberTel == member.MemberTel && m.Password == ComputeSha256Hash(member.Password));

                    if (user != null)
                    {
                        var claims = new List<Claim>
                    {
                        new Claim(ClaimTypes.Name, user.MemberTel),
                        new Claim(ClaimTypes.Role, "Member"),
                         new Claim(ClaimTypes.Sid, user.MemberID),
                          new Claim(ClaimTypes.Name, user.Name)

                    };

                        var claimsIdentity = new ClaimsIdentity(claims, "MemberLogin");

                        var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);

                        await HttpContext.SignInAsync("MemberLogin", claimsPrincipal); //把資料寫入 Cookie 進行登入狀態管理


                        return RedirectToAction("Index", "Orders", new { area = "User" });
                    }
                

                    ViewData["Error"] = "電話號碼或密碼錯誤。";
                

                return View(member);

            }

        private static string ComputeSha256Hash(string rawData)
        {
            using (SHA256 sha256Hash = SHA256.Create())
            {
                // 計算雜湊值
                byte[] bytes = sha256Hash.ComputeHash(System.Text.Encoding.UTF8.GetBytes(rawData));

                // 將 byte 陣列轉換成十六進位字串
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