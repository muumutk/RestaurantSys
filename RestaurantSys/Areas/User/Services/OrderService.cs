using Microsoft.EntityFrameworkCore;
using Microsoft.Data.SqlClient; // 一定要加這個
using RestaurantSys.Access.Data;

namespace RestaurantSys.Areas.User.Services
{
    public class OrderService : IOrderService
    {
        private readonly RestaurantSysContext _context;

        public OrderService(RestaurantSysContext context)
        {
            _context = context;
        }

        public async Task<int> AddNewOrderAsync(
            DateTime pickUpTime,
            string payTypeID,
            string note,
            string memberID,
            string orderStatusID,
            string cart)
        {
            var result = await _context.Database.ExecuteSqlRawAsync(
                "EXEC AddNewOrder @PickUpTime, @PayTypeID, @Note, @MemberID, @OrderStatusID, @Cart",
                new SqlParameter("@PickUpTime", pickUpTime),
                new SqlParameter("@PayTypeID", payTypeID),
                new SqlParameter("@Note",note),
                new SqlParameter("@MemberID", memberID),
                new SqlParameter("@OrderStatusID", orderStatusID),
                new SqlParameter("@Cart", cart)
            );

            return result;
        }
    }
}
