namespace RestaurantSys.Models
{
    public class Dish
    {
        public int DishID { get; set; }

        public string DishName { get; set; } = null!;

        public string Description { get; set; } = null!;

        public string? PhotoPath { get; set; }

        public decimal Price { get; set; }

        public string? Note { get; set; }

        public virtual ICollection<DishIngredient> DishIngredients { get; set; } = new List<DishIngredient>();
        public virtual ICollection<OrderDetail> OrderDetails { get; set; } = new List<OrderDetail>();
    }
}
