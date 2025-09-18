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

        // 修改 Index 方法，確保 View 使用正確的 ViewModel 型別
        public async Task<IActionResult> Index()
        {
            // 取得所有餐點，並載入相關的食材和食材名稱
            var dishes = await _context.Dish
                .Include(d => d.DishIngredients)
                .ThenInclude(di => di.Item)
                .ToListAsync();

            // 將每道菜的食材整理成 ViewModel
            var dishIngredients = dishes.Select(d => new VMDishIngredient
            {
                DishID = d.DishID,
                // 在這裡將餐點的名稱賦值給 ViewModel
                // 假設您的 Dish 實體中儲存名稱的屬性是 DishName 或 Description
                DishName = d.DishName, // 請依據您的資料模型修改
                Items = (d.DishIngredients ?? new List<DishIngredient>()).Select(di => new VMItemDetail
                {
                    ItemID = di.ItemID,
                    ItemName = di.Item?.ItemName ?? "",
                    Quantity = di.Quantity,
                    Unit = di.Unit
                }).ToList()
            }).ToList();

            return View("Index", dishIngredients);
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
        public async Task<IActionResult> Create(VMDishIngredient model)
        {
            if (ModelState.IsValid)
            {
                if (model.Items == null || model.Items.Count == 0)
                {
                    ModelState.AddModelError("", "請至少新增一個食材。");
                }
                else
                {
                    var newIngredients = new List<DishIngredient>();
                    var existingItemIDs = new HashSet<int>(_context.DishIngredient
                        .Where(di => di.DishID == model.DishID)
                        .Select(di => di.ItemID));

                    foreach (var itemDetail in model.Items)
                    {
                        if (existingItemIDs.Contains(itemDetail.ItemID))
                        {
                            ModelState.AddModelError("", $"食材 '{itemDetail.ItemName}' 已存在於此餐點中，已自動跳過。");
                            continue;
                        }

                        var dishIngredient = new DishIngredient
                        {
                            DishID = model.DishID,
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
            }
            
            ViewBag.DishID = new SelectList(await _context.Dish.ToListAsync(), "DishID", "DishName", model.DishID);
            ViewBag.ItemID = new SelectList(await _context.Stock.ToListAsync(), "ItemID", "ItemName");
            return View(model);
        }


        // GET: Backend/DishIngredients/Edit?dishId=1&itemId=2
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var dishToEdit = await _context.Dish
                .Include(d => d.DishIngredients)       // 載入 DishIngredient 列表
                .ThenInclude(di => di.Item)            // 繼續載入每個 DishIngredient 關聯的 Item (食材)
                .FirstOrDefaultAsync(d => d.DishID == id);

            if (dishToEdit == null)
            {
                return NotFound();
            }

            // 載入所有可供選擇的食材，用於新增功能
            var allItems = await _context.Stock.ToListAsync();

            // 將資料整理成 ViewModel
            var vm = new VMDishIngredient
            {
                DishID = dishToEdit.DishID,
                Items = dishToEdit.DishIngredients.Select(di => new VMItemDetail
                {
                    ItemID = di.ItemID,
                    ItemName = di.Item?.ItemName ?? "Unknown",
                    Quantity = di.Quantity,
                    Unit = di.Unit
                }).ToList(),
                AllItems = new SelectList(allItems, "ItemID", "ItemName")
            };

            return View(vm);
        }

        // POST: Backend/DishIngredients/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(VMDishIngredient model)
        {
            if (!ModelState.IsValid)
            {
                // 如果驗證失敗，需要重新載入 AllItems 讓下拉選單不為空
                model.AllItems = new SelectList(await _context.Stock.ToListAsync(), "ItemID", "ItemName");
                return View(model);
            }

            // 檢查是否有同名的新增食材，以避免重複
            if (model.NewItemID.HasValue)
            {
                var existingItem = model.Items.FirstOrDefault(i => i.ItemID == model.NewItemID.Value);
                if (existingItem != null)
                {
                    ModelState.AddModelError("NewItemID", "此食材已存在於餐點中，請直接編輯其數量。");
                    model.AllItems = new SelectList(await _context.Stock.ToListAsync(), "ItemID", "ItemName");
                    return View(model);
                }
            }

            try
            {
                // 1. 更新現有食材
                var existingIngredients = await _context.DishIngredient
                    .AsNoTracking()
                    .Where(di => di.DishID == model.DishID)
                    .ToListAsync();

                foreach (var item in model.Items)
                {
                    var originalItem = existingIngredients.FirstOrDefault(ei => ei.ItemID == item.ItemID);
                    if (originalItem != null)
                    {
                        originalItem.Quantity = item.Quantity;
                        originalItem.Unit = item.Unit;
                        _context.Entry(originalItem).State = EntityState.Modified;
                    }
                }

                // 2. 新增新的食材
                if (model.NewItemID.HasValue && model.NewQuantity.HasValue && !string.IsNullOrEmpty(model.NewUnit))
                {
                    var newIngredient = new DishIngredient
                    {
                        DishID = model.DishID,
                        ItemID = model.NewItemID.Value,
                        Quantity = model.NewQuantity.Value,
                        Unit = model.NewUnit,
                        IsActive = true // 假設新增的食材預設為啟用
                    };
                    _context.DishIngredient.Add(newIngredient);
                }

                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            catch (DbUpdateConcurrencyException)
            {
                ModelState.AddModelError(string.Empty, "資料已被其他使用者修改，請重新載入頁面後再試一次。");
                model.AllItems = new SelectList(await _context.Stock.ToListAsync(), "ItemID", "ItemName");
                return View(model);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, $"發生錯誤：{ex.Message}");
                model.AllItems = new SelectList(await _context.Stock.ToListAsync(), "ItemID", "ItemName");
                return View(model);
            }
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

        private bool DishIngredientExists(int dishId, int itemId)
        {
            return _context.DishIngredient.Any(e => e.DishID == dishId && e.ItemID == itemId);
        }
    }
}
