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

            TimeZoneInfo taipeiTimeZone;
            try
            {
                taipeiTimeZone = TimeZoneInfo.FindSystemTimeZoneById("Taipei Standard Time");
            }
            catch (TimeZoneNotFoundException)
            {
                taipeiTimeZone = TimeZoneInfo.FindSystemTimeZoneById("Asia/Taipei");
            }


            // 將資料轉換為動態物件，以符合 View 的需求
            var orderViewModels = recentOrders.Select(o => new
            {
                OrderID = o.OrderID,
                // ✨ 修正開始：假設資料庫時間是本地時間，並直接轉換到目標時區 ✨
                OrderDate = TimeZoneInfo.ConvertTime(o.OrderDate, TimeZoneInfo.Local, taipeiTimeZone),
                Status = o.OrderStatus?.OrderStatusName,
                TotalPrice = o.OrderDetails?.Sum(od => od.UnitPrice * od.Quantity) ?? 0
            }).ToList();


            return View(orderViewModels);
        }
    }
}