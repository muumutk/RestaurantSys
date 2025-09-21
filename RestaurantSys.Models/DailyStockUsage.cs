using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestaurantSys.Models
{
    public partial class DailyStockUsage
    {
        public int UsageID { get; set; } // PK
        public int DishID { get; set; } // 外鍵：關聯餐點
        public int ItemID { get; set; } // 外鍵：關聯食材
        public decimal QuantityUsed { get; set; } // 當日累計用量
        public DateTime UsageDate { get; set; } // 用量登記日期

        // 導覽屬性
        public virtual Dish Dish { get; set; }
        public virtual Stock Item { get; set; }
    }
}
