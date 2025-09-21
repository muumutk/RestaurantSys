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
    public class StocksController : Controller
    {
        private readonly RestaurantSysContext _context;

        public StocksController(RestaurantSysContext context)
        {
            _context = context;
        }

        // GET: Backend/Stocks
        public async Task<IActionResult> Index()
        {
            // 1. 查詢所有庫存資料，並包含供應商資訊
            var restaurantSysContext = _context.Stock.Include(s => s.Supplier);

            // 2. 查詢今日用量總計，並將結果轉換成字典，以便快速查詢
            var today = DateTime.Today;
            var dailyUsage = await _context.DailyStockUsage
                .Where(dsu => dsu.UsageDate == today)
                .GroupBy(dsu => dsu.ItemID)
                .ToDictionaryAsync(
                    g => g.Key,
                    g => g.Sum(d => d.QuantityUsed)
                );

            // 3. 將今日用量資料存入 ViewBag
            ViewBag.DailyUsage = dailyUsage;

            // 4. 將原有的庫存資料傳遞給 View
            return View(await restaurantSysContext.ToListAsync());
        }

        // GET: Backend/Stocks/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var stock = await _context.Stock
                .Include(s => s.Supplier)
                .FirstOrDefaultAsync(m => m.ItemID == id);
            if (stock == null)
            {
                return NotFound();
            }

            return View(stock);
        }

        // GET: Backend/Stocks/Create
        public IActionResult Create()
        {
            ViewData["SupplierID"] = new SelectList(_context.Supplier, "SupplierID", "SupplierName");
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> GetSupplierDetails(int id)
        {
            var supplier = await _context.Supplier.FirstOrDefaultAsync(s => s.SupplierID == id);

            if (supplier == null)
            {
                return NotFound();
            }

            return Json(new
            {
                supplierID = supplier.SupplierID,
                supplierName = supplier.SupplierName,
                contactPerson = supplier.ContactPerson,
                supplierTel = supplier.SupplierTel,
                address = supplier.Address
            });
        }

        // POST: Backend/Stocks/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ItemName,CurrentStock,SafeStock,Unit,ItemPrice,IsActive,SupplierID")] Stock stock)
        {
            if (ModelState.IsValid)
            {
                _context.Add(stock);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["SupplierID"] = new SelectList(_context.Supplier, "SupplierID", "SupplierName", stock.SupplierID);
            return View(stock);
        }

        // GET: Backend/Stocks/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var stock = await _context.Stock.FindAsync(id);
            if (stock == null)
            {
                return NotFound();
            }
            ViewData["SupplierID"] = new SelectList(_context.Supplier, "SupplierID", "SupplierName", stock.SupplierID);
            return View(stock);
        }

        // POST: Backend/Stocks/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ItemID,ItemName,CurrentStock,SafeStock,Unit,ItemPrice,IsActive,SupplierID")] Stock stock)
        {
            if (id != stock.ItemID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(stock);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!StockExists(stock.ItemID))
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
            ViewData["SupplierID"] = new SelectList(_context.Supplier, "SupplierID", "SupplierName", stock.SupplierID);
            return View(stock);
        }

        // GET: Backend/Stocks/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var stock = await _context.Stock
                .Include(s => s.Supplier)
                .FirstOrDefaultAsync(m => m.ItemID == id);
            if (stock == null)
            {
                return NotFound();
            }

            return View(stock);
        }

        // POST: Backend/Stocks/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var stock = await _context.Stock.FindAsync(id);
            if (stock != null)
            {
                _context.Stock.Remove(stock);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool StockExists(int id)
        {
            return _context.Stock.Any(e => e.ItemID == id);
        }
    }
}
