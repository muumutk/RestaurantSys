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
        public async Task<IActionResult> Create([Bind("DishID,ItemIDs,IsActive")] VMDishIngredient vmDishIngredient)
        {
            if (ModelState.IsValid)
            {
                foreach(var itemID in vmDishIngredient.ItemIDs)
                {
                    // 檢查是否已存在相同的 DishID 和 ItemID 組合
                    var exists = _context.DishIngredient.Any(di => di.DishID == vmDishIngredient.DishID && di.ItemID == itemID);
                    if (exists)
                    {
                        // 如果存在，跳過這個成分
                        continue;
                    }
                    // 為每個選定的成分建立一個 DishIngredient 物件
                    var dishIngredient = new DishIngredient
                    {
                        DishID = vmDishIngredient.DishID,
                        ItemID = itemID,
                        IsActive = true
                    };
                    _context.DishIngredient.Add(dishIngredient);
                }

                // 一次性將所有紀錄儲存到資料庫
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));

            }
            ViewData["DishID"] = new SelectList(_context.Dish, "DishID", "Description", vmDishIngredient.DishID);
            ViewData["ItemID"] = new SelectList(_context.Stock, "ItemID", "ItemName", vmDishIngredient.ItemIDs);
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
