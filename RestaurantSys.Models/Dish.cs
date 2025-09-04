namespace RestaurantSys.Models
{
    public partial class Dish
    {
        public int DishID { get; set; }

        public int DishCategoryID { get; set; }

        public string DishName { get; set; } = null!;

        public string Description { get; set; } = null!;

        public string? PhotoPath { get; set; }

        public decimal DishPrice { get; set; }

        public string? Note { get; set; }

        public bool IsActive { get; set; } = true;

        public virtual DishCategory? DishCategory { get; set; }

        public virtual List<DishIngredient>? DishIngredients { get; set; }
        public virtual List<OrderDetail>? OrderDetails { get; set; }
    }
}
