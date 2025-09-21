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
    public class DailyStockUsagesController : Controller
    {
        private readonly RestaurantSysContext _context;

        public DailyStockUsagesController(RestaurantSysContext context)
        {
            _context = context;
        }

        // GET: Admin/DailyStockUsages
        // 顯示當日用量總覽清單
        public async Task<IActionResult> Index()
        {
            var today = DateTime.Today;

            var dailyUsageSummary = await _context.DailyStockUsage
                .Where(dsu => dsu.UsageDate == today)
                .GroupBy(dsu => dsu.ItemID)
                .Select(g => new DailyUsageSummaryViewModel // 使用 ViewModel
                {
                    ItemID = g.Key,
                    QuantityUsed = g.Sum(d => d.QuantityUsed),
                    ItemName = _context.Stock.FirstOrDefault(s => s.ItemID == g.Key).ItemName // 這裡直接從資料庫查詢 ItemName
                })
                .ToListAsync();

            if (!dailyUsageSummary.Any())
            {
                ViewData["Message"] = "本日尚無任何用量資料。";
            }

            return View(dailyUsageSummary);
        }

        // GET: Admin/DailyStockUsages/Edit
        // 進入詳細編輯頁面
        public async Task<IActionResult> Edit()
        {
            var today = DateTime.Today;

            var dailyUsage = await _context.DailyStockUsage
                                           .Where(dsu => dsu.UsageDate == today)
                                           .Include(dsu => dsu.Dish)
                                           .Include(dsu => dsu.Item)
                                           .ToListAsync();

            var allStocks = await _context.Stock.ToListAsync();
            ViewData["StockID"] = new SelectList(allStocks, "ItemID", "ItemName");

            return View(dailyUsage);
        }

        // POST: Admin/DailyStockUsages/Update
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(int usageId, decimal newQuantity)
        {
            var dailyUsage = await _context.DailyStockUsage.FindAsync(usageId);
            if (dailyUsage == null) return NotFound();

            dailyUsage.QuantityUsed = newQuantity;

            await _context.SaveChangesAsync();
            TempData["SuccessMessage"] = "用量已成功更新。";

            return RedirectToAction(nameof(Edit));
        }

        // POST: Admin/DailyStockUsages/Add
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Add(int stockId, decimal quantity)
        {
            var today = DateTime.Today;
            // 請將 999 替換為你在 Dish 資料表中，專門用來表示「手動用量」的 DishID
            var defaultDishIdForManualUsage = 999;

            var dailyUsage = new DailyStockUsage
            {
                ItemID = stockId,
                QuantityUsed = quantity,
                UsageDate = today,
                DishID = defaultDishIdForManualUsage
            };

            _context.DailyStockUsage.Add(dailyUsage);
            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] = "新的用量已成功添加。";
            return RedirectToAction(nameof(Edit));
        }

        // POST: Admin/DailyStockUsages/Finalize
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Finalize()
        {
            var today = DateTime.Today;
            var dailyUsage = await _context.DailyStockUsage
                                            .Where(dsu => dsu.UsageDate == today)
                                            .ToListAsync();

            if (!dailyUsage.Any())
            {
                TempData["ErrorMessage"] = "本日沒有任何用量需要結算。";
                return RedirectToAction(nameof(Index));
            }

            using var transaction = await _context.Database.BeginTransactionAsync();

            try
            {
                foreach (var usage in dailyUsage)
                {
                    var stock = await _context.Stock.FindAsync(usage.ItemID);
                    if (stock != null)
                    {
                        stock.CurrentStock -= usage.QuantityUsed;
                        _context.Update(stock);
                    }
                }
                await _context.SaveChangesAsync();

                _context.DailyStockUsage.RemoveRange(dailyUsage);
                await _context.SaveChangesAsync();

                await transaction.CommitAsync();

                TempData["SuccessMessage"] = "本日用量已成功結算，庫存已更新。";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                TempData["ErrorMessage"] = $"結算失敗：{ex.Message}";
                return RedirectToAction(nameof(Index));
            }
        }

        // POST: Admin/DailyStockUsages/Delete
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var dailyStockUsage = await _context.DailyStockUsage.FindAsync(id);
            if (dailyStockUsage != null)
            {
                _context.DailyStockUsage.Remove(dailyStockUsage);
            }
            await _context.SaveChangesAsync();
            TempData["SuccessMessage"] = "用量已成功刪除。";
            return RedirectToAction(nameof(Edit));
        }
    }

    // 將 ViewModel 移到這裡，讓 View 能夠正確地引用它
    public class DailyUsageSummaryViewModel
    {
        public int ItemID { get; set; }
        public string ItemName { get; set; }
        public decimal QuantityUsed { get; set; }
    }
}