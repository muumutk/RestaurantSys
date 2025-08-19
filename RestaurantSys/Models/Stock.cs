namespace RestaurantSys.Models
{
    public class Stock
    {
        public int ItemID { get; set; }

        public string ItemName { get; set; } = null!;

        public int CurrentStock { get; set; }

        public int SafeStock { get; set; }

        public string Unit { get; set; } = null!;

        public bool IsActive { get; set; } = true;

        public string SupplierID { get; set; } = null!;

        public virtual Supplier Supplier { get; set; } = null!;

        public virtual List<DishIngredient>? DishIngredients { get; set; }
    }
}
