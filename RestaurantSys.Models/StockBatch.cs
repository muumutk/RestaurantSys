using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestaurantSys.Models
{
    public partial class StockBatch
    {
        public int BatchID { get; set; }

        public string BatchNo { get; set; } = null!;

        public string EmployeeID { get; set; } = null!;

        public int ItemID { get; set; }

        public int Quantity { get; set; }

        public decimal ItemPrice { get; set; }

        public DateTime ArrivalDate { get; set; }

        public DateTime ExpiryDate { get; set; }

        [ForeignKey("ItemID")]
        public virtual Stock? Stock { get; set; }

        public virtual Employee? Employee { get; set; }

    }

}
