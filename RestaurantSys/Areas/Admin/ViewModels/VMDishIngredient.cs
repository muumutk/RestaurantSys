using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace RestaurantSys.Areas.Admin.ViewModels
{
    // 新增一個 ViewModel 來表示單一的食材資訊
    public class VMItemDetail
    {
        public int ItemID { get; set; }
        public string? ItemName { get; set; }

        [Required(ErrorMessage = "請填寫數量")]
        [Range(0.01, 99999.99, ErrorMessage = "數量必須介於 0.01 到 99999.99 之間")]
        public decimal Quantity { get; set; }

        [StringLength(10, ErrorMessage = "單位欄位最多10個字")]
        public string? Unit { get; set; }
    }

    // 修改主 ViewModel
    public class VMDishIngredient
    {
        [Display(Name = "餐點")]
        [Required(ErrorMessage = "請選擇餐點")]
        public int DishID { get; set; }

        [Display(Name = "餐點名稱")]
        public string? DishName { get; set; }

        // 儲存選定的多個食材及其詳細資訊
        public List<VMItemDetail> Items { get; set; }

        /// <summary>
        /// 提供給新增食材下拉選單的選項列表
        /// </summary>
        public SelectList? AllItems { get; set; }
    }
}