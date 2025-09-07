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
    public class DishCategoriesController : Controller
    {
        private readonly RestaurantSysContext _context;

        public DishCategoriesController(RestaurantSysContext context)
        {
            _context = context;
        }

        // GET: Admin/DishCategories
        public async Task<IActionResult> Index()
        {
            return View(await _context.DishCategory.ToListAsync());
        }

        // GET: Admin/DishCategories/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var dishCategory = await _context.DishCategory
                .FirstOrDefaultAsync(m => m.DishCategoryID == id);
            if (dishCategory == null)
            {
                return NotFound();
            }

            return View(dishCategory);
        }

        // GET: Admin/DishCategories/Create
        public IActionResult Create()
        {
            ViewData["DishCategoryID"] = new SelectList(_context.DishCategory, "DishCategoryID", "DishCategoryName");
            return View();
        }

        // POST: Admin/DishCategories/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("DishCategoryID,DishCategoryName")] DishCategory dishCategory)
        {
            if (ModelState.IsValid)
            {
                _context.Add(dishCategory);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(dishCategory);
        }

        // GET: Admin/DishCategories/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var dishCategory = await _context.DishCategory.FindAsync(id);
            if (dishCategory == null)
            {
                return NotFound();
            }
            return View(dishCategory);
        }

        // POST: Admin/DishCategories/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("DishCategoryID,DishCategoryName")] DishCategory dishCategory)
        {
            if (id != dishCategory.DishCategoryID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(dishCategory);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DishCategoryExists(dishCategory.DishCategoryID))
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
            return View(dishCategory);
        }

        // GET: Admin/DishCategories/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var dishCategory = await _context.DishCategory
                .FirstOrDefaultAsync(m => m.DishCategoryID == id);
            if (dishCategory == null)
            {
                return NotFound();
            }

            return View(dishCategory);
        }

        // POST: Admin/DishCategories/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var dishCategory = await _context.DishCategory.FindAsync(id);
            if (dishCategory != null)
            {
                _context.DishCategory.Remove(dishCategory);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool DishCategoryExists(int id)
        {
            return _context.DishCategory.Any(e => e.DishCategoryID == id);
        }
    }
}
