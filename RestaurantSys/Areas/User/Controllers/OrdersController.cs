using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using RestaurantSys.Access.Data;
using RestaurantSys.Areas.User.Services;
using RestaurantSys.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace RestaurantSys.Areas.User.Controllers
{
    [Area("User")]
    public class OrdersController : Controller
    {
        private readonly RestaurantSysContext _context;
        private readonly IOrderService _orderService;

        public OrdersController(RestaurantSysContext context, IOrderService orderService)
        {
            _context = context;
            _orderService = orderService;
        }

        // GET: User/Orders
        public async Task<IActionResult> Index()
        {
            // 取得當前登入使用者的memberID
            var currentMemberID = User.FindFirstValue(ClaimTypes.NameIdentifier);

            // 確保使用者已登入
            if (string.IsNullOrEmpty(currentMemberID))
            {
                // 沒有會員ID，表示未登入，導向登入頁面
                return RedirectToAction("Login", "MemberLogin");
            }

            // 篩選屬於當前會員的訂單
            var orders = await _context.Order
                .Where(o => o.MemberID == currentMemberID)
                .Include(o => o.Member)
                .Include(o => o.OrderStatus)
                .Include(o => o.PayType)
                .Include(o => o.OrderDetails) // 為了總金額計算
                .OrderByDescending(o => o.OrderDate)
                .ToListAsync();

            return View(orders);
        }

        // 新增：處理會員取消訂單的 Action
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CancelOrder(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return NotFound();
            }

            // 取得當前使用者的 MemberID
            var currentMemberID = User.FindFirstValue(ClaimTypes.NameIdentifier);

            // 尋找訂單並確保它屬於當前使用者
            var order = await _context.Order
                .Where(o => o.OrderID == id && o.MemberID == currentMemberID)
                .FirstOrDefaultAsync();

            if (order == null)
            {
                return NotFound(); // 或者顯示更明確的錯誤訊息
            }

            // 檢查訂單是否可以取消（只有在狀態為 '01' 時才能取消）
            if (order.OrderStatusID == "01")
            {
                order.OrderStatusID = "05"; // 將狀態設為「已取消」
                _context.Update(order);
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = "訂單已成功取消。";
            }
            else
            {
                TempData["ErrorMessage"] = "此訂單狀態無法取消。";
            }

            return RedirectToAction(nameof(Index));
        }

        // GET: User/Orders/Details/5
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
                .FirstOrDefaultAsync(m => m.OrderID == id);
            if (order == null)
            {
                return NotFound();
            }

            return View(order);
        }

        // GET: User/Orders/Create
        public IActionResult Create()
        {
            // 根據目前時間判斷取餐日期，如果已過 22:00 則自動設定為隔天
            DateTime orderDate = DateTime.Now.Hour >= 22 ? DateTime.Now.AddDays(1) : DateTime.Now;

            ViewData["OrderDate"] = orderDate.ToString("yyyy-MM-dd");
            ViewData["PayTypeID"] = new SelectList(_context.PayType, "PayTypeID", "PayTypeName");
            ViewData["PickUpTimeOptions"] = GetPickUpTimeOptions(orderDate);

            return View();
        }

        // POST: User/Orders/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(IFormCollection form)
        {
            string orderDateString = form["OrderDate"];
            string pickUpTimeString = form["PickUpTime"];
            string payTypeID = form["PayTypeID"];
            string note = form["Note"];
            string cart = form["Cart"];

            string memberID = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            string orderStatusID = "01";

            DateTime combinedPickUpTime;
            if (DateTime.TryParse($"{orderDateString} {pickUpTimeString}", out combinedPickUpTime))
            {
                try
                {
                    await _orderService.AddNewOrderAsync(combinedPickUpTime, payTypeID, note, memberID, orderStatusID, cart);

                    TempData["SuccessMessage"] = "Order_Created";
                    TempData["ClearCart"] = true;

                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", "建立訂單時發生錯誤: " + ex.Message);
                }
            }
            else
            {
                ModelState.AddModelError("PickUpTime", "預計取餐時間格式不正確。");
            }

            // 如果 ModelState 無效或發生錯誤，重新載入表單所需的資料並返回 View
            DateTime orderDate = DateTime.Now.Hour >= 22 ? DateTime.Now.AddDays(1) : DateTime.Now;
            ViewData["OrderDate"] = orderDate.ToString("yyyy-MM-dd");
            ViewData["PayTypeID"] = new SelectList(_context.PayType, "PayTypeID", "PayTypeName");
            ViewData["PickUpTimeOptions"] = GetPickUpTimeOptions(orderDate);

            return View();
        }

        // GET: User/Orders/Edit/5
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
            ViewData["PayTypeID"] = new SelectList(_context.PayType, "PayTypeID", "PayTypeName", order.PayTypeID);
            return View(order);
        }

        // POST: User/Orders/Edit/5
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
            ViewData["PayTypeID"] = new SelectList(_context.PayType, "PayTypeID", "PayTypeName", order.PayTypeID);
            return View(order);
        }

        // 新增：專門用於 AJAX 請求的 Action，回傳特定日期的時間選項
        [HttpGet]
        public JsonResult GetAvailableTimeSlots(string date)
        {
            if (string.IsNullOrEmpty(date) || !DateTime.TryParse(date, out DateTime selectedDate))
            {
                return Json(new SelectList(new List<SelectListItem>()));
            }

            var timeOptions = GetPickUpTimeOptions(selectedDate);
            return Json(timeOptions);
        }

        // 新增：用於更新訂單狀態的 Action
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateOrderStatus(string id, string newStatus)
        {
            // 驗證輸入參數
            if (string.IsNullOrEmpty(id) || string.IsNullOrEmpty(newStatus))
            {
                return Json(new { success = false, message = "訂單ID或新狀態無效。" });
            }

            // 取得當前會員ID，確保只有自己的訂單能被修改
            var currentMemberID = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var order = await _context.Order
                                    .Where(o => o.OrderID == id && o.MemberID == currentMemberID)
                                    .FirstOrDefaultAsync();

            if (order == null)
            {
                return Json(new { success = false, message = "找不到該訂單或您沒有權限修改。" });
            }

            // 確保新狀態是允許的，例如 "03" (未取餐) 或 "04" (已取餐)
            if (newStatus == "03" || newStatus == "04")
            {
                order.OrderStatusID = newStatus;
                _context.Update(order);
                await _context.SaveChangesAsync();
                return Json(new { success = true, message = "訂單狀態已成功更新。" });
            }
            else
            {
                return Json(new { success = false, message = "無效的訂單狀態。" });
            }
        }

        private SelectList GetPickUpTimeOptions(DateTime selectedDate)
        {
            var pickUpTimes = new List<SelectListItem>();
            var now = DateTime.Now;

            var openTime = new TimeSpan(10, 0, 0);
            var closeTime = new TimeSpan(22, 0, 0);

            if (selectedDate.Date == now.Date)
            {
                var nextHalfHour = now.AddMinutes(30 - (now.Minute % 30));
                var startTime = nextHalfHour > now.Date.Add(openTime) ? nextHalfHour : now.Date.Add(openTime);
                var endTime = now.Date.Add(closeTime);

                for (var time = startTime; time <= endTime; time = time.AddMinutes(30))
                {
                    pickUpTimes.Add(new SelectListItem
                    {
                        Value = time.ToString("HH:mm"),
                        Text = time.ToString("HH:mm")
                    });
                }
            }
            else if (selectedDate.Date > now.Date)
            {
                var startTime = selectedDate.Date.Add(openTime);
                var endTime = selectedDate.Date.Add(closeTime);

                for (var time = startTime; time <= endTime; time = time.AddMinutes(30))
                {
                    pickUpTimes.Add(new SelectListItem
                    {
                        Value = time.ToString("HH:mm"),
                        Text = time.ToString("HH:mm")
                    });
                }
            }

            return new SelectList(pickUpTimes, "Value", "Text");
        }

        private bool OrderExists(string id)
        {
            return _context.Order.Any(e => e.OrderID == id);
        }
    }
}