namespace RestaurantSys.Models
{
    public partial class Order
    {
        public string OrderID { get; set; } = null!;

        public DateTime OrderDate { get; set; }

        public DateTime PickUpTime { get; set; }

        public string? Note { get; set; }

        public string MemberID { get; set; } = null!;

        public string EmployeeID { get; set; } = null!;

        public virtual Member Member { get; set; } = null!;
        public virtual Employee Employee { get; set; } = null!;

        public virtual List<OrderDetail>? OrderDetails { get; set; }
    }
}
