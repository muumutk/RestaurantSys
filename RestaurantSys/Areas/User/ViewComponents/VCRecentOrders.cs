using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RestaurantSys.Access.Data;
using RestaurantSys.Models;
using System.Linq;

namespace RestaurantSys.Areas.User.ViewComponents
{
    public class VCRecentOrders : ViewComponent
    {
        private readonly RestaurantSysContext _context;

        public VCRecentOrders(RestaurantSysContext context)
        {
            _context = context;
        }

        public async Task<IViewComponentResult> InvokeAsync(string memberId)
        {
            var recentOrders = await _context.Order
                .Where(o => o.MemberID == memberId)
                .OrderByDescending(o => o.OrderDate)
                .Take(3)
                .Include(o => o.OrderStatus)
                .Include(o => o.OrderDetails)
                .ToListAsync();

            // 將資料轉換為動態物件，以符合 View 的需求
            var orderViewModels = recentOrders.Select(o => new
            {
                OrderID = o.OrderID,
                OrderDate = o.OrderDate.ToLocalTime(), // 轉換時間
                Status = o.OrderStatus?.OrderStatusName, // 使用安全導覽運算子
                TotalPrice = o.OrderDetails?.Sum(od => od.UnitPrice * od.Quantity) ?? 0 // 計算總價並處理 null
            }).ToList();

            return View(orderViewModels);
        }
    }
}