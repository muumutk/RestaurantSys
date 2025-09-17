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

        [Display(Name = "數量")]
        [Required(ErrorMessage = "請填寫數量")]
        [StringLength(5, ErrorMessage = "數量欄位最多5個")]
        public int Quantity { get; set; }

        [Display(Name = "單位")]
        [StringLength(10, ErrorMessage = "單位欄位最多10個字")]
        public string? Unit { get; set; }

        [Display(Name = "是否啟用")]
        public bool IsActive { get; set; } = true;
    }
}
