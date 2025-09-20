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

        public OrdersController(RestaurantSysContext context,IOrderService orderService)
        {
            _context = context;
            _orderService = orderService;
        }

        // GET: User/Orders
        public async Task<IActionResult> Index()
        {
            //取得當前登入使用者的memberID
            var currentMemberID = User.FindFirstValue(ClaimTypes.NameIdentifier);

            //確保使用者已登入
            if (string.IsNullOrEmpty(currentMemberID))
            {
                //沒有會員ID，表示未登入，導向登入頁面
                return RedirectToAction("MemberLogin", "Login");
            }

            //篩選屬於當前會員的訂單
            var orders = await _context.Order
                .Where(o => o.MemberID == currentMemberID)
                .Include(o => o.Employee)
                .Include(o => o.Member)
                .Include(o => o.OrderStatus)
                .Include(o => o.PayType)
                .OrderByDescending(o => o.OrderDate)
                .ToListAsync();


            return View(orders);
        }

        // GET: User/Orders/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var order = await _context.Order
                .Include(o => o.Employee)
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
            string OrderDate = DateTime.Now.ToString("yyyy-MM-dd");

            ViewData["OrderDate"] = OrderDate;
            ViewData["PayTypeID"] = new SelectList(_context.PayType, "PayTypeID", "PayTypeName");

            ViewData["PickUpTimeOptions"] = GetPickUpTimeOptions();

            return View();

        }

        // POST: User/Orders/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(
            // 使用 IFormCollection 來手動取得所有表單欄位，
            // 以徹底避免模型繫結的問題。
            IFormCollection form)
        {
            // 從表單中手動取得所有必要的欄位值
            string orderDateString = form["OrderDate"];
            string pickUpTimeString = form["PickUpTime"];
            string payTypeID = form["PayTypeID"];
            string note = form["Note"];
            string cart = form["Cart"];

            // 取得 MemberID 和 OrderStatusID
            string memberID = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            string orderStatusID = "01";

            // 嘗試將日期和時間字串合併成一個完整的 DateTime 物件
            DateTime combinedPickUpTime;
            if (DateTime.TryParse($"{orderDateString} {pickUpTimeString}", out combinedPickUpTime))
            {
                try
                {
                    // 呼叫服務層方法，傳入所有必要的參數
                    await _orderService.AddNewOrderAsync(combinedPickUpTime, payTypeID, note, memberID, orderStatusID, cart);

                    TempData["SuccessMessage"] = "Order_Created";
                    // 在 TempData 中設定旗標
                    TempData["ClearCart"] = true;

                    // 成功後重新導向到訂單列表頁面
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    // 如果服務執行時發生錯誤，將錯誤訊息加入 ModelState
                    ModelState.AddModelError("", "建立訂單時發生錯誤: " + ex.Message);
                }
            }
            else
            {
                // 如果日期時間轉換失敗，將錯誤訊息加入 ModelState
                ModelState.AddModelError("PickUpTime", "預計取餐時間格式不正確。");
            }

            // 如果 ModelState 無效或發生錯誤，重新載入表單所需的資料並返回 View
            ViewData["OrderDate"] = DateTime.Now.ToString("yyyy-MM-dd");
            ViewData["PayTypeID"] = new SelectList(_context.PayType, "PayTypeID", "PayTypeName");
            ViewData["PickUpTimeOptions"] = GetPickUpTimeOptions();

            // 返回 View，但不傳入任何模型，因為使用 IFormCollection 處理了
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
            ViewData["EmployeeID"] = new SelectList(_context.Employee, "EmployeeID", "EmployeeID", order.EmployeeID);
            ViewData["MemberID"] = new SelectList(_context.Member, "MemberID", "MemberID", order.MemberID);
            ViewData["OrderStatusID"] = new SelectList(_context.OrderStatus, "OrderStatusID", "OrderStatusID", order.OrderStatusID);
            ViewData["PayTypeID"] = new SelectList(_context.PayType, "PayTypeID", "PayTypeName", order.PayTypeID);
            return View(order);
        }

        // POST: User/Orders/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("OrderID,OrderDate,PickUpTime,PayTypeID,Note,MemberID,EmployeeID,OrderStatusID")] Order order)
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
            ViewData["EmployeeID"] = new SelectList(_context.Employee, "EmployeeID", "EmployeeID", order.EmployeeID);
            ViewData["MemberID"] = new SelectList(_context.Member, "MemberID", "MemberID", order.MemberID);
            ViewData["OrderStatusID"] = new SelectList(_context.OrderStatus, "OrderStatusID", "OrderStatusID", order.OrderStatusID);
            ViewData["PayTypeID"] = new SelectList(_context.PayType, "PayTypeID", "PayTypeName", order.PayTypeID);
            return View(order);
        }



        //取餐選擇時間
        private SelectList GetPickUpTimeOptions()
        {
            var pickUpTimes = new List<SelectListItem>();

            // 從早上 10:00 開始
            var startTime = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 10, 0, 0);
            // 到晚上 10:00 結束
            var endTime = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 22, 0, 0);

            for (var time = startTime; time <= endTime; time = time.AddMinutes(30))
            {
                string timeString = time.ToString("HH:mm");
                pickUpTimes.Add(new SelectListItem
                {
                    Value = timeString,
                    Text = timeString
                });
            }

            return new SelectList(pickUpTimes, "Value", "Text");
        }


        private bool OrderExists(string id)
        {
            return _context.Order.Any(e => e.OrderID == id);
        }
    }
}
