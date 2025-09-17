using System.ComponentModel.DataAnnotations;

namespace RestaurantSys.Areas.Admin.ViewModels
{
    public class VMDishIngredient
    {
        [Display(Name ="餐點")]
        public int DishID { get; set; }

        //選擇多個成分
        [Display(Name ="物品編號")]
        public List<int> ItemIDs { get; set; }

        public bool IsActive { get; set; } = true;
    }
}
