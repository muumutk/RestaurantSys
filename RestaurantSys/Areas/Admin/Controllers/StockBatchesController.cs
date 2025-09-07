using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using RestaurantSys.Access.Data;
using RestaurantSys.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RestaurantSys.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(AuthenticationSchemes = "EmployeeLogin")]
    public class StockBatchesController : Controller
    {
        private readonly RestaurantSysContext _context;

        public StockBatchesController(RestaurantSysContext context)
        {
            _context = context;
        }


        // GET: Backend/StockBatches
        public async Task<IActionResult> Index()
        {
            // 取得目前日期與時間
            var today = DateTime.Now;

            // 查詢資料庫，篩選出所有到期日大於或等於今日的庫存批次。
            var restaurantSysContext = _context.StockBatch
                                               .Include(s => s.Employee)
                                               .Where(s => s.ExpiryDate >= today);

            // 將篩選後的資料傳遞給 View
            return View(await restaurantSysContext.ToListAsync());
        }


        // GET: Admin/StockBatches/GetUnitByItemID
        [HttpGet]
        public async Task<IActionResult> GetUnitByItemID(string itemID)
        {
            if (string.IsNullOrEmpty(itemID))
            {
                return NotFound();
            }

            //嘗試將字串轉換為整數
            if (!int.TryParse(itemID, out int parsedItemID))
            {
                // 如果轉換失敗，表示傳入的 ID 不是有效的數字
                return BadRequest("Invalid Item ID.");
            }

            var item = await _context.Stock
                //使用轉換後的整數進行查詢
                .Where(s => s.ItemID == parsedItemID)
                .Select(s => new { Unit = s.Unit })
                .FirstOrDefaultAsync();

            if (item == null)
            {
                return NotFound();
            }

            return Json(item.Unit);
        }


        // GET: Admin/StockBatches/Expired
        public async Task<IActionResult> Expired()
        {
            // 取得目前的日期與時間。
            var today = DateTime.Now;

            // 查詢資料庫，篩選出所有到期日早於今日的庫存批次。
            // 同時使用 Include() 載入相關的 Employee 物件。
            var expiredBatches = await _context.StockBatch
                                               .Include(s => s.Employee)
                                               .Where(s => s.ExpiryDate < today)
                                               .ToListAsync();

            // 將篩選後的資料傳遞給 View。
            return View(expiredBatches);
        }


        // GET: Backend/StockBatches/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var stockBatch = await _context.StockBatch
                .Include(s => s.Employee)
                .FirstOrDefaultAsync(m => m.BatchID == id);
            if (stockBatch == null)
            {
                return NotFound();
            }

            return View(stockBatch);
        }

        // GET: Backend/StockBatches/Create
        public async Task<IActionResult> Create()
        {
            ViewData["EmployeeID"] = new SelectList(await _context.Employee.ToListAsync(), "EmployeeID", "EName");

            var stockItems = await _context.Stock.ToListAsync();

            var itemsWithCombinedText = stockItems.Select(s => new
            {
                s.ItemID,
                ItemName = $"{s.ItemID} - {s.ItemName}"
            }).ToList();

            ViewData["ItemID"] = new SelectList(itemsWithCombinedText, "ItemID", "ItemName");
            return View();
        }

        // POST: Backend/StockBatches/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("EmployeeID,ItemID,Quantity,ItemPrice,ArrivalDate,ExpiryDate")] StockBatch stockBatch)
        {
            ModelState.Remove("BatchNo");
            if (ModelState.IsValid)
            {
                stockBatch.BatchNo = DateTime.Now.ToString("yyyyMMdd");

                _context.Add(stockBatch);
                await _context.SaveChangesAsync();

                var stockItem = await _context.Stock.FindAsync(stockBatch.ItemID);
                if (stockItem != null)
                {
                    stockItem.CurrentStock += stockBatch.Quantity;
                    _context.Update(stockItem);
                    await _context.SaveChangesAsync();
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["EmployeeID"] = new SelectList(_context.Employee, "EmployeeID", "EName", stockBatch.EmployeeID);
            ViewData["ItemID"] = new SelectList(_context.Stock, "ItemID", "ItemName" , stockBatch.ItemID);
            return View(stockBatch);
        }

        // GET: Backend/StockBatches/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var stockBatch = await _context.StockBatch.FindAsync(id);
            if (stockBatch == null)
            {
                return NotFound();
            }
            ViewData["EmployeeID"] = new SelectList(_context.Employee, "EmployeeID", "EmployeeID", stockBatch.EmployeeID);
            return View(stockBatch);
        }

        // POST: Backend/StockBatches/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("BatchID,BatchNo,EmployeeID,ItemID,Quantity,ItemPrice,ArrivalDate,ExpiryDate")] StockBatch stockBatch)
        {
            if (id != stockBatch.BatchID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(stockBatch);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!StockBatchExists(stockBatch.BatchID))
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
            ViewData["EmployeeID"] = new SelectList(_context.Employee, "EmployeeID", "EmployeeID", stockBatch.EmployeeID);
            return View(stockBatch);
        }


        private bool StockBatchExists(int id)
        {
            return _context.StockBatch.Any(e => e.BatchID == id);
        }
    }
}
