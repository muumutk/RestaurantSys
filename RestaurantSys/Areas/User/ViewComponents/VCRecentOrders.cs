using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RestaurantSys.Access.Data;
using RestaurantSys.Models;

namespace RestaurantSys.Areas.User.ViewComponents
{
    public class VCRecentOrders: ViewComponent
    {
        private readonly RestaurantSysContext _context;

        public VCRecentOrders(RestaurantSysContext context)
        {
            _context = context;
        }


        public async Task<IViewComponentResult> InvokeAsync(string memberId)
        {
            var recentOrders = await _context.Order
                // 根據傳入的 memberId 篩選訂單
                .Where(o => o.MemberID == memberId)
                .OrderByDescending(o => o.OrderDate) // 假設訂單模型有一個 OrderDate 屬性
                .Take(3)
                .Include(o => o.OrderDetails)
                .ToListAsync();

            // 建立一個匿名物件的清單
            var orderViewModels = recentOrders.Select(o => new
            {
                OrderID = o.OrderID,
                OrderDate = o.OrderDate,
                Status = o.OrderStatus,
                TotalPrice = o.OrderDetails.Sum(od => od.UnitPrice * od.Quantity)
            }).ToList();

            return View(orderViewModels);
        }

    }
}
