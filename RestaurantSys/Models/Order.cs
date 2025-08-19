namespace RestaurantSys.Models
{
    public class Order
    {
        public string OrderID { get; set; } = null!;

        public DateTime OrderDate { get; set; }

        public DateTime PickUpTime { get; set; }

        public string? Note { get; set; }

        public string MemberID { get; set; } = null!;

        public string StaffID { get; set; } = null!;

        public virtual Member Member { get; set; } = null!;
        public virtual Staff Staff { get; set; } = null!;

        public virtual ICollection<OrderDetail> OrderDetails { get; set; } = new List<OrderDetail>();
    }
}
