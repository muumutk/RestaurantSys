using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestaurantSys.Models
{
    public partial class DishCategory
    {
        public int DishCategoryID { get; set; }

        public string DishCategoryName { get; set; } = null!;

        public virtual List<Dish>? Dishes { get; set; }

    }
}
