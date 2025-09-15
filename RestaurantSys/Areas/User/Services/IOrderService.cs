namespace RestaurantSys.Areas.User.Services
{
    public interface IOrderService
    {
        Task<int> AddNewOrderAsync(
            DateTime pickUpTime,
            string payTypeID,
            string note,
            string memberID,
            string orderStatusID,
            string cart);
    }
}
