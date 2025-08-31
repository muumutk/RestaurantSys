using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.CodeAnalysis.Scripting;
using Microsoft.EntityFrameworkCore;
using RestaurantSys.Access.Data;
using RestaurantSys.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Intrinsics.Arm;
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

                // 取得當前日期
                var now = DateTime.Now;
                string yearMonth = $"{now.ToString("yy")}{now.ToString("MM")}";

                // 查詢今天註冊的最後一個會員
                var lastMember = await _context.Member
                    .Where(m => m.MemberID.StartsWith($"M{yearMonth}"))
                    .OrderByDescending(m => m.MemberID)
                    .FirstOrDefaultAsync();

                string newSerialNumber = "0001";
                if (lastMember != null)
                {
                    // 解析流水號並加一
                    string lastSerialNumberStr = lastMember.MemberID.Substring(6); // 取得流水號部分
                    int lastSerialNumber = int.Parse(lastSerialNumberStr);
                    newSerialNumber = (lastSerialNumber + 1).ToString("D4"); // D4 表示格式化成四位數
                }

                // 組裝新的會員編號
                member.MemberID = $"M{yearMonth}{newSerialNumber}";

                // 將密碼進行 SHA256 雜湊處理
                member.Password = HashPasswordSHA256(member.Password);

                _context.Member.Add(member);
                await _context.SaveChangesAsync();

                return RedirectToAction("Index", "Orders", new { area = "User" });
            }

            return View(member);
        }

        private static string HashPasswordSHA256(string rawData)
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
