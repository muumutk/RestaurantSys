namespace RestaurantSys.Models
{
    public partial class DishIngredient
    {
        public int DishID { get; set; }

        public int ItemID { get; set; }

        public decimal Quantity { get; set; }

        public string? Unit { get; set; }

        public bool IsActive { get; set; } = true;

        public virtual Dish? Dish { get; set; }
        public virtual Stock? Item { get; set; }

    }
}
