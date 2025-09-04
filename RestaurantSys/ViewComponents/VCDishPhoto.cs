using Microsoft.AspNetCore.Mvc;

namespace RestaurantSys.ViewComponents
{
    public class VCDishPhoto : ViewComponent
    {
        public IViewComponentResult Invoke(string photoPath, string dishName)
        {
            var model = new DishPhotoViewModel
            {
                PhotoPath = photoPath,
                DishName = dishName
            };

            return View(model);
        }
    }

    public class DishPhotoViewModel
    {
        public string PhotoPath { get; set; }
        public string DishName { get; set; }
    }

}
