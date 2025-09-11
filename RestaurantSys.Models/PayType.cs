using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestaurantSys.Models
{
    public partial class PayType
    {
        public string PayTypeID { get; set; } = null!;

        public string PayTypeName { get; set; } = null!;

        public virtual List<Order>? Orders { get; set; }

    }
}
