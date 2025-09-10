using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using RestaurantSys.Access.Data;
using RestaurantSys.DTOs;
using RestaurantSys.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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
        // Controllers/MenuController.cs
        // 在 MenuController.cs
        public async Task<IActionResult> Index(int? id)
        {
            var dishesQuery = _context.Dish.Where(d => d.IsActive).AsQueryable();

            if (id.HasValue)
            {
                dishesQuery = dishesQuery.Where(d => d.DishCategoryID == id.Value);
            }

            // 將資料庫模型投影到 DishDTO
            var dishes = await dishesQuery
                .OrderBy(d => d.DishID)
                .Select(d => new DishDTO
                {
                    DishID = d.DishID,
                    DishName = d.DishName,
                    Description = d.Description,
                    PhotoPath = d.PhotoPath,
                    DishPrice = d.DishPrice,
                    Note = d.Note
                })
                .ToListAsync();

            return View(dishes);
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
