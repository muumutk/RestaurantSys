namespace RestaurantSys.Models
{
    public partial class Stock
    {
        public int ItemID { get; set; }

        public string ItemName { get; set; } = null!;

        public decimal? CurrentStock { get; set; }

        public decimal? SafeStock { get; set; }

        public string Unit { get; set; } = null!;

        public decimal ItemPrice { get; set; }

        public bool IsActive { get; set; } = true;

        public int SupplierID { get; set; }

        public virtual Supplier? Supplier { get; set; }

        public virtual List<DishIngredient>? DishIngredients { get; set; }

        public virtual List<StockBatch>? StockBatches { get; set; }
    }
}
