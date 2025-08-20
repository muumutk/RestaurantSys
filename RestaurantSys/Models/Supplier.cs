namespace RestaurantSys.Models
{
    public partial class Supplier
    {
        public int SupplierID { get; set; }

        public string SupplierName { get; set; } = null!;

        public string ContactPerson { get; set; } = null!;
        
        public string SupplierTel { get; set; } = null!;

        public string Address { get; set; } = null!;

        public virtual List<Stock>? Stocks { get; set; }
    }
}
