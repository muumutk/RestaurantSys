using System.ComponentModel.DataAnnotations;

namespace RestaurantSys.Areas.Admin.ViewModels
{
    // 新增一個 ViewModel 來表示單一的食材資訊
    public class VMItemDetail
    {
        public int ItemID { get; set; }
        public string? ItemName { get; set; }

        [Required(ErrorMessage = "請填寫數量")]
        [Range(1, 99999, ErrorMessage = "數量必須介於 1 到 99999 之間")]
        public int Quantity { get; set; }

        [StringLength(10, ErrorMessage = "單位欄位最多10個字")]
        public string? Unit { get; set; }
    }

    // 修改主 ViewModel
    public class VMDishIngredient
    {
        [Display(Name = "餐點")]
        [Required(ErrorMessage = "請選擇餐點")]
        public int DishID { get; set; }

        // 儲存選定的多個食材及其詳細資訊
        public List<VMItemDetail> Items { get; set; }
    }
}