using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using RestaurantSys.Access.Data;
using RestaurantSys.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims; // 由於不再需要 User.FindFirstValue，此行可選擇性移除
using System.Threading.Tasks;

namespace RestaurantSys.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(AuthenticationSchemes = "EmployeeLogin")]
    public class OrdersController : Controller
    {
        private readonly RestaurantSysContext _context;

        public OrdersController(RestaurantSysContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            // 查詢訂單，並急切載入所有相關的導覽屬性
            var orders = await _context.Order
                .Include(o => o.Member)
                .Include(o => o.OrderStatus)
                .Include(o => o.PayType)
                .Include(o => o.OrderDetails)
                .OrderByDescending(o => o.OrderDate)
                .ToListAsync();

            // 遍歷所有訂單，並將 OrderDetails 中的 GetTime 轉換為本地時間
            foreach (var order in orders)
            {
                if (order.OrderDetails != null)
                {
                    foreach (var detail in order.OrderDetails)
                    {
                        if (detail.GetTime.HasValue)
                        {
                            // 將 UTC 時間轉換為伺服器的本地時間
                            detail.GetTime = detail.GetTime.Value.ToLocalTime();
                        }
                    }
                }
            }

            return View(orders);
        }

        // 新增：處理前端輪詢請求的 Action
        // 這個方法會檢查是否有任何狀態為「已送出訂單」（OrderStatusID == "01"）且尚未被管理的訂單。
        [HttpGet]
        public async Task<IActionResult> CheckForNewOrders()
        {
            // 在這裡篩選你想要通知的訂單狀態
            var newOrdersCount = await _context.Order
                .CountAsync(o => o.OrderStatusID == "01");

            // 返回 JSON 格式的結果
            // 如果數量大於 0，表示有新訂單
            return Json(new { hasNewOrders = newOrdersCount > 0 });
        }


        // GET: Admin/Orders/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var order = await _context.Order
                .Include(o => o.Member)
                .Include(o => o.OrderStatus)
                .Include(o => o.PayType)
                .Include(o => o.OrderDetails)
                    .ThenInclude(od => od.Dish)
                .FirstOrDefaultAsync(m => m.OrderID == id);

            if (order == null)
            {
                return NotFound();
            }

            // 只將來自前端的 UTC 時間 (GetTime) 轉換為本地時間
            if (order.OrderDetails != null)
            {
                foreach (var detail in order.OrderDetails)
                {
                    if (detail.GetTime.HasValue)
                    {
                        detail.GetTime = detail.GetTime.Value.ToLocalTime();
                    }
                }
            }

            return View(order);
        }


        // 新增：處理訂單狀態變更的 Action
        [HttpPost]
        public async Task<IActionResult> ChangeStatus([FromForm] string id, [FromForm] string statusId, [FromForm] DateTime? getTime)
        {
            if (string.IsNullOrEmpty(id) || string.IsNullOrEmpty(statusId))
            {
                return Json(new { success = false, message = "傳入的資料無效。" });
            }

            // 使用 Include 方法確保同時載入 OrderDetails
            var order = await _context.Order
                .Include(o => o.OrderDetails)
                .FirstOrDefaultAsync(o => o.OrderID == id);

            if (order == null)
            {
                return Json(new { success = false, message = "找不到此訂單。" });
            }

            // 更新訂單狀態
            order.OrderStatusID = statusId;

            // 判斷是否為「已取餐」狀態，並寫入取餐時間
            if (statusId == "04" && getTime.HasValue)
            {
                foreach (var detail in order.OrderDetails)
                {
                    detail.GetTime = getTime.Value;
                }
            }

            try
            {
                await _context.SaveChangesAsync();
                return Json(new { success = true });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "狀態更新失敗: " + ex.Message + " (Inner Exception: " + ex.InnerException?.Message + ")" });
            }
        }



        // GET: Admin/Orders/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
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
            ViewData["OrderStatusID"] = new SelectList(_context.OrderStatus, "OrderStatusID", "OrderStatusID", order.OrderStatusID);
            ViewData["PayTypeID"] = new SelectList(_context.PayType, "PayTypeID", "PayTypeID", order.PayTypeID);
            return View(order);
        }

        // POST: Admin/Orders/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("OrderID,OrderDate,PickUpTime,PayTypeID,Note,MemberID,OrderStatusID")] Order order)
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
            ViewData["OrderStatusID"] = new SelectList(_context.OrderStatus, "OrderStatusID", "OrderStatusID", order.OrderStatusID);
            ViewData["PayTypeID"] = new SelectList(_context.PayType, "PayTypeID", "PayTypeID", order.PayTypeID);
            return View(order);
        }


        private bool OrderExists(string id)
        {
            return _context.Order.Any(e => e.OrderID == id);
        }
    }
}