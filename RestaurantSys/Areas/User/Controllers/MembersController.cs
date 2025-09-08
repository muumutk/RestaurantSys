using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using RestaurantSys.Access.Data;
using RestaurantSys.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace RestaurantSys.Areas.User.Controllers
{
    [Area("User")]
    [Authorize(Roles = "Member")]
    public class MembersController : Controller
    {
        private readonly IWebHostEnvironment _hostingEnvironment;
        private readonly RestaurantSysContext _context;

        public MembersController(RestaurantSysContext context, IWebHostEnvironment hostingEnvironment)
        {
            _context = context;
            _hostingEnvironment = hostingEnvironment;
        }

        // GET: User/Members
        public async Task<IActionResult> Index()
        {
            // 取得當前登入會員的 MemberID
            var currentMemberID = User.FindFirstValue(ClaimTypes.NameIdentifier);

            // 如果會員ID為空，導向登入頁面或顯示錯誤
            if (string.IsNullOrEmpty(currentMemberID))
            {
                return RedirectToAction("MemberLogin", "Login");
            }

            //根據當前會員ID，從資料庫查詢對應的會員資料
            var member = await _context.Member
                .FirstOrDefaultAsync(m => m.MemberID == currentMemberID);

            if (member == null)
            {
                return NotFound();
            }

            return View(member);
        }

        public IActionResult ShowMyCart()
        {
            return View();
        }

        //上傳會員中心照片
        [HttpPost]
        public async Task<IActionResult> UploadAvatar(IFormFile avatarFile)
        {
            try
            {
                if (avatarFile == null || avatarFile.Length == 0)
                {
                    return BadRequest("沒有上傳任何檔案。");
                }

                // 檢查檔案類型
                var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".gif" };
                var fileExtension = Path.GetExtension(avatarFile.FileName)?.ToLowerInvariant();
                if (string.IsNullOrEmpty(fileExtension) || !allowedExtensions.Contains(fileExtension))
                {
                    return BadRequest("不支援的檔案類型，請上傳 .jpg, .jpeg, .png 或 .gif 檔案。");
                }

                // 取得當前登入會員的 ID
                var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (string.IsNullOrEmpty(userId))
                {
                    return Unauthorized("找不到會員 ID，請確認是否已登入。");
                }

                // 準備檔案儲存路徑
                var uploadsFolder = Path.Combine(_hostingEnvironment.WebRootPath, "avatars");
                if (!Directory.Exists(uploadsFolder))
                {
                    Directory.CreateDirectory(uploadsFolder);
                }

                var fileName = $"{userId}{fileExtension}";
                var filePath = Path.Combine(uploadsFolder, fileName);

                // 將上傳的檔案儲存到指定路徑
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await avatarFile.CopyToAsync(stream);
                }

                // 查詢會員資料，使用與 Index 方法相同的屬性名稱
                var member = await _context.Member.FirstOrDefaultAsync(m => m.MemberID == userId);
                if (member == null)
                {
                    return NotFound("找不到該會員。");
                }

                // 更新會員的頭像 URL
                member.AvatarUrl = $"/avatars/{fileName}";
                await _context.SaveChangesAsync();

                return Ok(new { message = "頭像上傳成功！", avatarUrl = member.AvatarUrl });
            }
            catch (Exception ex)
            {
                // 回傳詳細錯誤訊息，以便前端除錯
                return StatusCode(500, $"上傳過程中發生伺服器錯誤：{ex.Message}");
            }
        }

        //刪除會員頭像
        [HttpPost]
        public async Task<IActionResult> DeleteAvatar()
        {
            try
            {
                var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (string.IsNullOrEmpty(userId))
                {
                    return Unauthorized("找不到會員 ID，請確認是否已登入。");
                }

                // 在資料庫中找到該會員
                var member = await _context.Member.FirstOrDefaultAsync(m => m.MemberID == userId);
                if (member == null)
                {
                    return NotFound("找不到該會員。");
                }

                // 檢查會員是否有頭像路徑
                if (string.IsNullOrEmpty(member.AvatarUrl))
                {
                    return BadRequest("會員沒有頭像，無需刪除。");
                }

                // 取得檔案的實體路徑
                var filePath = Path.Combine(_hostingEnvironment.WebRootPath, member.AvatarUrl.TrimStart('/'));

                // 檢查檔案是否存在並刪除
                if (System.IO.File.Exists(filePath))
                {
                    System.IO.File.Delete(filePath);
                }

                // 清空資料庫中的頭像 URL
                member.AvatarUrl = null;
                await _context.SaveChangesAsync();

                // 回傳成功訊息
                return Ok(new { message = "頭像已成功刪除！" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"刪除過程中發生伺服器錯誤：{ex.Message}");
            }
        }


        // GET: User/Members/Details/5
        public async Task<IActionResult> Details()
        {
            var currentMemberID = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (string.IsNullOrEmpty(currentMemberID))
            {
                return NotFound();
            }

            var member = await _context.Member
                .FirstOrDefaultAsync(m => m.MemberID == currentMemberID);

            if (member == null)
            {
                return NotFound();
            }

            return View(member);
        }

        // GET: User/Members/Edit
        public async Task<IActionResult> Edit()
        {
            var currentMemberID = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(currentMemberID))
            {
                return NotFound();
            }

            var member = await _context.Member.FindAsync(currentMemberID);
            if (member == null)
            {
                return NotFound();
            }

            return View(member);
        }

        // POST: User/Members/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit([Bind("MemberID,Name,MemberTel,City,Address,Birthday,title,MEmail")] Member member)
        {
            // 驗證提交的 MemberID 是否與當前登入者一致，防止使用者竄改資料
            var currentMemberID = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (member.MemberID != currentMemberID)
            {
                return Forbid(); // 返回 403 Forbidden
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var existingMember = await _context.Member.FindAsync(member.MemberID);
                    if (existingMember == null) return NotFound();

                    // 只更新允許的欄位，不動 Password
                    existingMember.Name = member.Name;
                    existingMember.MemberTel = member.MemberTel;
                    existingMember.City = member.City;
                    existingMember.Address = member.Address;
                    existingMember.Birthday = member.Birthday;
                    existingMember.title = member.title;
                    existingMember.MEmail = member.MEmail;

                    await _context.SaveChangesAsync();

                    // PRG (Post-Redirect-Get)：避免表單重送，並確保畫面是最新資料
                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!MemberExists(member.MemberID))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
            }

            // 驗證失敗：回傳完整的資料（不要只用 POST 回來的 member）
            var dbMember = await _context.Member.FindAsync(member.MemberID);
            return View(dbMember ?? member);
        }

        private bool MemberExists(string id)
        {
            // 這裡也應該加上安全性檢查，但由於已經在上面過濾，這行程式碼可能用不到
            return _context.Member.Any(e => e.MemberID == id);
        }
    }
}
