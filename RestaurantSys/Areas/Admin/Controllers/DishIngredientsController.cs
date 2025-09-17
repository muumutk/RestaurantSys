using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using RestaurantSys.Access.Data;
using RestaurantSys.Models;
using RestaurantSys.Areas.Admin.ViewModels;

namespace RestaurantSys.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class DishIngredientsController : Controller
    {
        private readonly RestaurantSysContext _context;

        public DishIngredientsController(RestaurantSysContext context)
        {
            _context = context;
        }

        // GET: Backend/DishIngredients
        public async Task<IActionResult> Index()
        {
            var restaurantSysContext = _context.DishIngredient.Include(d => d.Dish).Include(d => d.Item);
            return View(await restaurantSysContext.ToListAsync());
        }

        //取得庫存單位
        [HttpGet]
        public async Task<IActionResult> GetItemUnit(int itemID)
        {
            var stock = await _context.Stock.FirstOrDefaultAsync(s => s.ItemID == itemID);
            if (stock == null)
            {
                return NotFound();
            }
            return Json(new { unit = stock.Unit }); // 假設 Stock 表中有 Unit 欄位
        }


        // GET: Backend/DishIngredients/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var dishIngredient = await _context.DishIngredient
                .Include(d => d.Dish)
                .Include(d => d.Item)
                .FirstOrDefaultAsync(m => m.DishID == id);
            if (dishIngredient == null)
            {
                return NotFound();
            }

            return View(dishIngredient);
        }

        // GET: Backend/DishIngredients/Create
        public IActionResult Create()
        {
            ViewData["DishID"] = new SelectList(_context.Dish, "DishID", "DishName");
            ViewData["ItemID"] = new SelectList(_context.Stock, "ItemID", "ItemName");
            return View();
        }

        // POST: Backend/DishIngredients/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(VMDishIngredient vmDishIngredient)
        {
            // 檢查 DishID 和 Items 列表
            if (ModelState.IsValid && vmDishIngredient.Items != null && vmDishIngredient.Items.Any())
            {
                var newIngredients = new List<DishIngredient>();
                var existingRecords = new HashSet<int>(_context.DishIngredient
                    .Where(di => di.DishID == vmDishIngredient.DishID)
                    .Select(di => di.ItemID));

                // 遍歷所有提交的食材
                foreach (var itemDetail in vmDishIngredient.Items)
                {
                    // 檢查是否已存在
                    if (existingRecords.Contains(itemDetail.ItemID))
                    {
                        ModelState.AddModelError("", $"食材 '{itemDetail.ItemName}' 已存在於此餐點中，已自動跳過。");
                        continue;
                    }

                    // 建立新的 DishIngredient 物件
                    var dishIngredient = new DishIngredient
                    {
                        DishID = vmDishIngredient.DishID,
                        ItemID = itemDetail.ItemID,
                        Quantity = itemDetail.Quantity,
                        Unit = itemDetail.Unit,
                        IsActive = true
                    };
                    newIngredients.Add(dishIngredient);
                }

                if (newIngredients.Any())
                {
                    _context.DishIngredient.AddRange(newIngredients);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
            }

            // 如果模型驗證失敗或沒有新增任何食材
            ViewData["DishID"] = new SelectList(_context.Dish, "DishID", "DishName", vmDishIngredient.DishID);
            ViewData["ItemID"] = new SelectList(_context.Stock, "ItemID", "ItemName");
            return View(vmDishIngredient);
        }


        // GET: Backend/DishIngredients/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var dishIngredient = await _context.DishIngredient.FindAsync(id);
            if (dishIngredient == null)
            {
                return NotFound();
            }
            ViewData["DishID"] = new SelectList(_context.Dish, "DishID", "Description", dishIngredient.DishID);
            ViewData["ItemID"] = new SelectList(_context.Stock, "ItemID", "ItemName", dishIngredient.ItemID);
            return View(dishIngredient);
        }

        // POST: Backend/DishIngredients/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("DishID,ItemID,IsActive")] DishIngredient dishIngredient)
        {
            if (id != dishIngredient.DishID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(dishIngredient);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DishIngredientExists(dishIngredient.DishID))
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
            ViewData["DishID"] = new SelectList(_context.Dish, "DishID", "Description", dishIngredient.DishID);
            ViewData["ItemID"] = new SelectList(_context.Stock, "ItemID", "ItemName", dishIngredient.ItemID);
            return View(dishIngredient);
        }

        // GET: Backend/DishIngredients/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var dishIngredient = await _context.DishIngredient
                .Include(d => d.Dish)
                .Include(d => d.Item)
                .FirstOrDefaultAsync(m => m.DishID == id);
            if (dishIngredient == null)
            {
                return NotFound();
            }

            return View(dishIngredient);
        }

        // POST: Backend/DishIngredients/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var dishIngredient = await _context.DishIngredient.FindAsync(id);
            if (dishIngredient != null)
            {
                _context.DishIngredient.Remove(dishIngredient);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool DishIngredientExists(int id)
        {
            return _context.DishIngredient.Any(e => e.DishID == id);
        }
    }
}
