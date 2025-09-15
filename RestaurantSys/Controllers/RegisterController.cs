using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.CodeAnalysis.Scripting;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using RestaurantSys.Access.Data;
using RestaurantSys.Models;
using RestaurantSys.Services;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Runtime.Intrinsics.Arm;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace RestaurantSys.Controllers
{
    public class RegisterController : Controller
    {
        private readonly RestaurantSysContext _context;

        public RegisterController(RestaurantSysContext context)
        {
            _context = context;
        }

        public IActionResult Register()
        {
            return View(new Member());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(Member member, string confirmPassword)
        {
            ModelState.Remove("MemberID");

            // 檢查密碼和確認密碼是否相符
            if (member.Password != confirmPassword)
            {
                ModelState.AddModelError("Password", "密碼和確認密碼不相符。");
                // 由於 confirmPassword 並非 Member Model 的屬性，這裡加一個對應的錯誤訊息，方便在 view 顯示
                ModelState.AddModelError("ConfirmPassword", "密碼和確認密碼不相符。");
                return View(member);
            }

            if (ModelState.IsValid)
            {
                // 只在使用者有輸入電子郵件時，才進行重複性檢查
                if (!string.IsNullOrWhiteSpace(member.MEmail))
                {
                    bool emailExists = await _context.Member.AnyAsync(m => m.MEmail == member.MEmail);
                    if (emailExists)
                    {
                        ModelState.AddModelError("MEmail", "此電子郵件已被註冊。");
                        return View(member);
                    }
                }

                //使用SQL預存程序生成會員編號
                var registerDateParam = new SqlParameter("@RegisterDate", DateTime.Now);
                var newMemberIDParam = new SqlParameter
                {
                    ParameterName = "@NewMemberID",
                    SqlDbType = SqlDbType.VarChar,
                    Size = 20,
                    Direction = ParameterDirection.Output
                };

                await _context.Database.ExecuteSqlRawAsync(
                    "EXEC GetNextMemberID @RegisterDate , @NewMemberID OUT",
                    registerDateParam, newMemberIDParam);

                string newMemberID = (string)newMemberIDParam.Value;
                member.MemberID = newMemberID;


                // 將密碼進行 SHA256 雜湊處理
                member.Password = HashService.HashPasswordSHA256(member.Password);

                _context.Member.Add(member);
                await _context.SaveChangesAsync();

                // 手動登入會員
                var claims = new List<Claim>
                    {
                        new Claim(ClaimTypes.NameIdentifier, member.MemberID),
                        new Claim(ClaimTypes.Name,member.Name),
                        new Claim(ClaimTypes.Role, "Member")
                    };
                var claimsIdentity = new ClaimsIdentity(claims, "MemberLogin"); // 使用你設定的 Scheme Name
                var authProperties = new AuthenticationProperties
                {
                    IsPersistent = true // 記住登入狀態
                };
                await HttpContext.SignInAsync("MemberLogin", new ClaimsPrincipal(claimsIdentity), authProperties);

                //將狀態存入TempData
                TempData["ShowProfileModal"] = true;

                // 導向會員訂單頁面，此時會員已登入，不會被重新導向
                return RedirectToAction("Index", "Members", new { area = "User" });
            }

            return View(member);
        }

    }
}
