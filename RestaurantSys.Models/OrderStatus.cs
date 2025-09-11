using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestaurantSys.Models
{
    public partial class OrderStatus
    {
        public string OrderStatusID { get; set; } = null!;

        public string OrderStatusName { get; set; } = null!;

        public virtual List<Order>? Orders { get; set; }
    }
}
