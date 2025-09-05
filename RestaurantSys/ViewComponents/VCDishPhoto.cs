using Microsoft.AspNetCore.Mvc;

namespace RestaurantSys.ViewComponents
{
    public class VCDishPhoto : ViewComponent
    {
        public IViewComponentResult Invoke(string photoPath, string dishName)
        {

            return View(new { PhotoPath = photoPath, DishName = dishName });
        }
    }
}