using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using RestaurantSys.Access.Data;
using RestaurantSys.Models;

namespace RestaurantSys.Controllers
{
    public class MenuController : Controller
    {
        private readonly RestaurantSysContext _context;

        public MenuController(RestaurantSysContext context)
        {
            _context = context;
        }

        // GET: User/Menu
        public async Task<IActionResult> Index()
        {
            var activeDishes = await _context.Dish
                                 .Where(d => d.IsActive)
                                 .OrderBy(d => d.DishID) // 依 ID 排序，讓順序固定
                                 .ToListAsync();
            return View(activeDishes);
        }

        // GET: User/Menu/Details/5
        //public async Task<IActionResult> Details(int? id)
        //{
        //    if (id == null)
        //    {
        //        return NotFound();
        //    }

        //    var dish = await _context.Dish
        //        .FirstOrDefaultAsync(m => m.DishID == id);
        //    if (dish == null)
        //    {
        //        return NotFound();
        //    }

        //    return View(dish);
        //}



        private bool DishExists(int id)
        {
            return _context.Dish.Any(e => e.DishID == id);
        }
    }
}
