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
        private readonly RestaurantSysContext _context;

        public MembersController(RestaurantSysContext context)
        {
            _context = context;
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
                    _context.Update(member);
                    await _context.SaveChangesAsync();
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
                return RedirectToAction(nameof(Index));
            }
            return View(member);
        }

        private bool MemberExists(string id)
        {
            // 這裡也應該加上安全性檢查，但由於已經在上面過濾，這行程式碼可能用不到
            return _context.Member.Any(e => e.MemberID == id);
        }
    }
}
