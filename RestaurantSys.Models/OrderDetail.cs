namespace RestaurantSys.Models
{
    public partial class OrderDetail
    {
        public string OrderID { get; set; } = null!;

        public int DishID { get; set; }

        public decimal Price { get; set; }

        public DateTime GetTime { get; set; }

        public virtual Order Order { get; set; } = null!;
        public virtual Dish Dish { get; set; } = null!;
    }
}
