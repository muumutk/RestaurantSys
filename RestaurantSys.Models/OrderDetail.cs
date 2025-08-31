namespace RestaurantSys.Models
{
    public partial class OrderDetail
    {
        public string OrderID { get; set; } = null!;

        public int DishID { get; set; }

        public int Quantity { get; set; }

        public decimal UnitPrice { get; set; }

        public DateTime? GetTime { get; set; }

        public virtual Order Order { get; set; } = null!;
        public virtual Dish Dish { get; set; } = null!;
    }
}
