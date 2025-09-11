using System.ComponentModel.DataAnnotations;

namespace RestaurantSys.Models
{
    public partial class Order
    {
        public string OrderID { get; set; } = null!;

        public DateTime OrderDate { get; set; }

        public DateTime PickUpTime { get; set; }

        public string PayTypeID { get; set; } = null!;

        public string? Note { get; set; }

        public string MemberID { get; set; } = null!;

        public string? EmployeeID { get; set; }

        public string OrderStatusID { get; set; } = null!;

        public virtual Member? Member { get; set; }
        public virtual Employee? Employee { get; set; }
        public virtual PayType? PayType { get; set; }
        public virtual OrderStatus? OrderStatus { get; set; }

        public virtual List<OrderDetail>? OrderDetails { get; set; }
    }
}
