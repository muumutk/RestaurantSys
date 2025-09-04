using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using RestaurantSys.Access.Data;
using RestaurantSys.Models;
using RestaurantSys.ViewComponents;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RestaurantSys.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class DishesController : Controller
    {
        private readonly RestaurantSysContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public DishesController(RestaurantSysContext context, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
        }

        // GET: Backend/Dishes
        public async Task<IActionResult> Index()
        {
            var activeDishes = await _context.Dish.Where(d => d.IsActive).ToListAsync();
            return View(activeDishes);
        }

        public async Task<IActionResult> InactiveDishes()
        {
            var inactiveDishes = await _context.Dish.Where(d => d.IsActive == false).ToListAsync();
            return View(inactiveDishes);
        }


        // GET: Backend/Dishes/Details/5
        public async Task<IActionResult> Details(int? id , string returnUrl)
        {
            if (id == null)
            {
                return NotFound();
            }

            var dish = await _context.Dish
                .FirstOrDefaultAsync(m => m.DishID == id);
            if (dish == null)
            {
                return NotFound();
            }
            ViewData["ReturnUrl"] = returnUrl;
            return View(dish);
        }

        // GET: Backend/Dishes/Create
        public IActionResult Create()
        {
            ViewData["DishCategoryID"] = new SelectList(_context.DishCategory, "DishCategoryID", "DishCategoryName");
            return View();
        }

        // POST: Backend/Dishes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("DishID,DishName,DishCategoryID,Description,PhotoPath,DishPrice,Note,IsActive")] Dish dish, IFormFile DishPhoto)
        {
            if (ModelState.IsValid)
            {
                if (DishPhoto != null && DishPhoto.Length > 0)
                {
                    var uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "DishPhotos");
                    if (!Directory.Exists(uploadsFolder))
                    {
                        Directory.CreateDirectory(uploadsFolder);
                    }
                    var uniqueFileName = Guid.NewGuid().ToString() + Path.GetExtension(DishPhoto.FileName);
                    var filePath = Path.Combine(uploadsFolder, uniqueFileName);

                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        await DishPhoto.CopyToAsync(fileStream);
                    }

                    dish.PhotoPath = "/DishPhotos/" + uniqueFileName;
                }

                _context.Add(dish);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["DishCategoryID"] = new SelectList(_context.DishCategory, "DishCategoryID", "DishCategoryName", dish.DishCategoryID);
            return View(dish);
        }

        // GET: Admin/Dishes/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var dish = await _context.Dish
                                 .Include(d => d.DishCategory)
                                 .FirstOrDefaultAsync(m => m.DishID == id);
            if (dish == null)
            {
                return NotFound();
            }
            ViewData["DishCategoryID"] = new SelectList(_context.DishCategory, "DishCategoryID", "DishCategoryName", dish.DishCategory);
            return View(dish);
        }

        // POST: Admin/Dishes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("DishID,DishName,DishCategoryID,Description,PhotoPath,DishPrice,Note,IsActive")] Dish dish, IFormFile DishPhoto)
        {
            ModelState.Remove("DishPhoto");
            if (id != dish.DishID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                var dishToUpdate = await _context.Dish.FindAsync(id);
                if (dishToUpdate == null)
                {
                    return NotFound();
                }

                if (DishPhoto != null && DishPhoto.Length > 0)
                {
                    if (!string.IsNullOrEmpty(dishToUpdate.PhotoPath))
                    {
                        var oldFilePath = Path.Combine(_webHostEnvironment.WebRootPath, dishToUpdate.PhotoPath.TrimStart('~', '/'));
                        if (System.IO.File.Exists(oldFilePath))
                        {
                            System.IO.File.Delete(oldFilePath);
                        }
                    }

                    var uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "DishPhotos");
                    if (!Directory.Exists(uploadsFolder))
                    {
                        Directory.CreateDirectory(uploadsFolder);
                    }
                    var uniqueFileName = Guid.NewGuid().ToString() + Path.GetExtension(DishPhoto.FileName);
                    var filePath = Path.Combine(uploadsFolder, uniqueFileName);

                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        await DishPhoto.CopyToAsync(fileStream);
                    }

                    dishToUpdate.PhotoPath = "/DishPhotos/" + uniqueFileName;
                }

                // 更新其他欄位（包含 IsActive）
                dishToUpdate.DishName = dish.DishName;
                dishToUpdate.Description = dish.Description;
                dishToUpdate.DishPrice = dish.DishPrice;
                dishToUpdate.Note = dish.Note;
                dishToUpdate.IsActive = dish.IsActive;
                dishToUpdate.DishCategoryID = dish.DishCategoryID;

                try
                {
                    _context.Update(dishToUpdate);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DishExists(dishToUpdate.DishID))
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
            ViewData["DishCategoryID"] = new SelectList(_context.DishCategory, "DishCategoryID", "DishCategoryName", dish.DishCategoryID);
            return View(dish);
        }

        // GET: Backend/Dishes/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var dish = await _context.Dish
                .FirstOrDefaultAsync(m => m.DishID == id);
            if (dish == null)
            {
                return NotFound();
            }

            return View(dish);
        }

        // POST: Backend/Dishes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var dish = await _context.Dish.FindAsync(id);
            if (dish != null)
            {
                _context.Dish.Remove(dish);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool DishExists(int id)
        {
            return _context.Dish.Any(e => e.DishID == id);
        }
    }
}
