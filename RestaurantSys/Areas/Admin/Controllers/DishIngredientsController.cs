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
                .Include(d => d.DishIngredients)
                .ThenInclude(di => di.Item)
                .FirstOrDefaultAsync(d => d.DishID == id);

            if (dishToEdit == null)
            {
                return NotFound();
            }

            var allItems = await _context.Stock.ToListAsync();

            var vm = new VMDishIngredient
            {
                DishID = dishToEdit.DishID,
                DishName = dishToEdit.DishName,
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
            // 驗證現有食材清單的合法性
            if (model.Items != null)
            {
                foreach (var item in model.Items)
                {
                    // 這裡可以加入更多驗證，例如檢查數量是否為正數
                    if (item.Quantity <= 0)
                    {
                        ModelState.AddModelError("", $"{item.ItemName} 的數量必須大於0。");
                    }
                }
            }

            if (!ModelState.IsValid)
            {
                // 如果驗證失敗，需要重新載入 AllItems 讓下拉選單不為空
                model.AllItems = new SelectList(await _context.Stock.ToListAsync(), "ItemID", "ItemName");
                return View(model);
            }

            try
            {
                // 1. 取得資料庫中該餐點的所有食材
                var existingIngredients = await _context.DishIngredient
                    .Where(di => di.DishID == model.DishID)
                    .ToListAsync();

                // 2. 準備要更新、新增或刪除的清單
                var itemsToUpdate = model.Items ?? new List<VMItemDetail>();
                var itemsFromDb = new HashSet<int>(existingIngredients.Select(i => i.ItemID));
                var itemsFromForm = new HashSet<int>(itemsToUpdate.Select(i => i.ItemID));

                // 3. 處理需要刪除的食材 (在資料庫中存在，但在表單中不存在)
                var itemsToRemove = existingIngredients.Where(di => !itemsFromForm.Contains(di.ItemID)).ToList();
                _context.DishIngredient.RemoveRange(itemsToRemove);

                // 4. 處理需要更新的食材 (兩者都存在)
                foreach (var itemDetail in itemsToUpdate)
                {
                    var ingredientToUpdate = existingIngredients.FirstOrDefault(di => di.ItemID == itemDetail.ItemID);
                    if (ingredientToUpdate != null)
                    {
                        ingredientToUpdate.Quantity = itemDetail.Quantity;
                        ingredientToUpdate.Unit = itemDetail.Unit;
                        _context.Entry(ingredientToUpdate).State = EntityState.Modified;
                    }
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

        // POST: 用於新增單一食材，透過 AJAX 呼叫
        [HttpPost]
        public async Task<IActionResult> AddIngredient(int dishId, int itemId, decimal quantity, string unit)
        {
            // 檢查食材是否已存在
            var existingIngredient = await _context.DishIngredient
                .AnyAsync(di => di.DishID == dishId && di.ItemID == itemId);

            if (existingIngredient)
            {
                return Json(new { success = false, message = "此食材已存在於餐點中。" });
            }

            var newIngredient = new DishIngredient
            {
                DishID = dishId,
                ItemID = itemId,
                Quantity = quantity,
                Unit = unit,
                IsActive = true
            };

            try
            {
                _context.DishIngredient.Add(newIngredient);
                await _context.SaveChangesAsync();

                var addedItem = await _context.Stock.FindAsync(itemId);
                var newItemDetail = new VMItemDetail
                {
                    ItemID = newIngredient.ItemID,
                    ItemName = addedItem?.ItemName,
                    Quantity = newIngredient.Quantity,
                    Unit = newIngredient.Unit
                };

                return Json(new { success = true, newItem = newItemDetail });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = $"新增失敗: {ex.Message}" });
            }
        }

        // POST: 用於移除單一食材，透過 AJAX 呼叫
        [HttpPost]
        public async Task<IActionResult> RemoveIngredient(int dishId, int itemId)
        {
            var ingredientToRemove = await _context.DishIngredient
                .FirstOrDefaultAsync(di => di.DishID == dishId && di.ItemID == itemId);

            if (ingredientToRemove == null)
            {
                return Json(new { success = false, message = "找不到要移除的食材。" });
            }

            try
            {
                _context.DishIngredient.Remove(ingredientToRemove);
                await _context.SaveChangesAsync();
                return Json(new { success = true, message = "食材已成功移除。" });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = $"移除失敗: {ex.Message}" });
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
