using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using RestaurantSys.Access.Data;
using RestaurantSys.Models;

namespace RestaurantSys.Areas.Backend.Controllers
{
    [Area("Backend")]
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
            var restaurantSysContext = _context.StockBatch.Include(s => s.Employee);
            return View(await restaurantSysContext.ToListAsync());
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
        public IActionResult Create()
        {
            ViewData["EmployeeID"] = new SelectList(_context.Employee, "EmployeeID", "EmployeeID");
            return View();
        }

        // POST: Backend/StockBatches/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("BatchID,BatchNo,EmployeeID,ItemID,Quantity,ItemPrice,ArrivalDate,ExpiryDate")] StockBatch stockBatch)
        {
            if (ModelState.IsValid)
            {
                _context.Add(stockBatch);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["EmployeeID"] = new SelectList(_context.Employee, "EmployeeID", "EmployeeID", stockBatch.EmployeeID);
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

        // GET: Backend/StockBatches/Delete/5
        public async Task<IActionResult> Delete(int? id)
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

        // POST: Backend/StockBatches/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var stockBatch = await _context.StockBatch.FindAsync(id);
            if (stockBatch != null)
            {
                _context.StockBatch.Remove(stockBatch);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool StockBatchExists(int id)
        {
            return _context.StockBatch.Any(e => e.BatchID == id);
        }
    }
}
