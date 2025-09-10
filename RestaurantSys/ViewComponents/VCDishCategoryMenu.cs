using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RestaurantSys.Access.Data;
using RestaurantSys.DTOs;

namespace RestaurantSys.ViewComponents
{
    public class VCDishCategoryMenu : ViewComponent
    {
        private readonly RestaurantSysContext _context;

        public VCDishCategoryMenu(RestaurantSysContext context)
        {
            _context = context;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            // 從資料庫取得資料，並使用 LINQ 投影到 DTO
            var categories = await _context.DishCategory
                .Select(c => new CategoryMenuDTO
                {
                    DishCategoryID = c.DishCategoryID,
                    DishCategoryName = c.DishCategoryName
                })
                .ToListAsync();

            return View(categories);
        }

    }
}
