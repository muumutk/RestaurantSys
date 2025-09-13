namespace RestaurantSys.Areas.User.Services
{
    public interface IOrderService
    {
        Task<int> AddNewOrderAsync(
            DateTime pickUpTime,
            string payTypeID,
            string memberID,
            string orderStatusID,
            string cart);
    }
}
