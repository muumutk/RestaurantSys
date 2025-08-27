using Microsoft.AspNetCore.Mvc;

namespace RestaurantSys.Controllers
{
    public class LoginController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
