namespace RestaurantSys.Models
{
    public class Supplier
    {
        public int SupplierID { get; set; }

        public string SupplierName { get; set; } = null!;

        public string ContactPerson { get; set; } = null!;
        
        public string SupplierTel { get; set; } = null!;

        public string Address { get; set; } = null!;

        public virtual ICollection<Stock> Stocks { get; set; } = new List<Stock>();
    }
}
