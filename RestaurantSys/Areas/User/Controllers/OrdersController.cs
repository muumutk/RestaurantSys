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
    public class OrdersController : Controller
    {
        private readonly RestaurantSysContext _context;

        public OrdersController(RestaurantSysContext context)
        {
            _context = context;
        }

        // GET: User/Orders
        public async Task<IActionResult> Index()
        {
            //取得當前登入使用者的memberID
            var currentMemberID = User.FindFirstValue(ClaimTypes.NameIdentifier);

            //確保使用者已登入
            if(string.IsNullOrEmpty(currentMemberID))
            {
                //沒有會員ID，表示未登入，導向登入頁面
                return RedirectToAction("MemberLogin", "Login");
            }

            //篩選屬於當前會員的訂單
            var orders = await _context.Order.Where(o => o.MemberID == currentMemberID).ToListAsync();

            return View(orders);

        }

        //確認訂單
        public IActionResult ConfirmOrder()
        {
            // 在這裡處理結帳邏輯，例如：
            // 1. 從 Session 或其他地方取得購物車資料。
            // 2. 建立一個新訂單，並將資料存入資料庫。
            // 3. 成功建立訂單後，將使用者導向到顯示訂單詳情的頁面。
            //    例如：return RedirectToAction("ConfirmOrder", new { id = newOrderId });

            // 暫時先回傳一個 View 進行測試
            return View("ConfirmOrder"); // 這裡可以回傳一個空白的結帳頁面
        }

        // 保留你原來的 Action，用來顯示特定訂單的詳情
        public async Task<IActionResult> ConfirmOrder(string id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var order = await _context.Order
                .Include(o => o.Member)
                .FirstOrDefaultAsync(m => m.OrderID == id);
            if (order == null)
            {
                return NotFound();
            }
            return View(order);
        }


        // GET: User/Orders/Details/5
        public async Task<IActionResult> Details(string id)
        {
            var currentMemberID = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (id == null)
            {
                return NotFound();
            }

            var order = await _context.Order
                .Include(o => o.Member)
                .FirstOrDefaultAsync(m => m.OrderID == id && m.MemberID == currentMemberID);
            if (order == null)
            {
                return NotFound();
            }

            return View(order);
        }

        // GET: User/Orders/Create
        public IActionResult Create()
        {
            ViewData["MemberID"] = new SelectList(_context.Member, "MemberID", "MemberID");
            return View();
        }

        // POST: User/Orders/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("OrderID,OrderDate,PickUpTime,Note,MemberID,EmployeefID")] Order order)
        {
            if (ModelState.IsValid)
            {
                _context.Add(order);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["MemberID"] = new SelectList(_context.Member, "MemberID", "MemberID", order.MemberID);
            return View(order);
        }

        // GET: User/Orders/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            ModelState.Remove("Password");

            if (id == null)
            {
                return NotFound();
            }

            var order = await _context.Order.FindAsync(id);
            if (order == null)
            {
                return NotFound();
            }
            ViewData["MemberID"] = new SelectList(_context.Member, "MemberID", "MemberID", order.MemberID);
            return View(order);
        }

        // POST: User/Orders/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("OrderID,OrderDate,PickUpTime,Note,MemberID,EmployeefID")] Order order)
        {
            if (id != order.OrderID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(order);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!OrderExists(order.OrderID))
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
            ViewData["MemberID"] = new SelectList(_context.Member, "MemberID", "MemberID", order.MemberID);
            return View(order);
        }

        // GET: User/Orders/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var order = await _context.Order
                .Include(o => o.Member)
                .FirstOrDefaultAsync(m => m.OrderID == id);
            if (order == null)
            {
                return NotFound();
            }

            return View(order);
        }

        // POST: User/Orders/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var order = await _context.Order.FindAsync(id);
            if (order != null)
            {
                _context.Order.Remove(order);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool OrderExists(string id)
        {
            return _context.Order.Any(e => e.OrderID == id);
        }
    }
}
