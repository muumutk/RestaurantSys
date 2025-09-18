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

        // 新增屬性存放餐點名稱
        [Display(Name = "餐點名稱")]
        public string? DishName { get; set; }

        // 儲存選定的多個食材及其詳細資訊
        public List<VMItemDetail> Items { get; set; }

        // 以下為新增的屬性，用於處理新增食材的表單

        /// <summary>
        /// 提供給新增食材下拉選單的選項列表
        /// </summary>
        public SelectList? AllItems { get; set; }

        /// <summary>
        /// 儲存使用者選擇的新增食材ID
        /// </summary>
        [Display(Name = "選擇食材")]
        [Required(ErrorMessage = "請選擇要新增的食材")]
        public int? NewItemID { get; set; }

        /// <summary>
        /// 儲存使用者輸入的新增食材數量
        /// </summary>
        [Display(Name = "數量")]
        [Required(ErrorMessage = "請填寫新食材的數量")]
        [Range(0.01, 99999.99, ErrorMessage = "數量必須介於 0.01 到 99999.99 之間")]
        public decimal? NewQuantity { get; set; }

        /// <summary>
        /// 儲存使用者輸入的新增食材單位
        /// </summary>
        [Display(Name = "單位")]
        [StringLength(10, ErrorMessage = "單位欄位最多10個字")]
        public string? NewUnit { get; set; }
    }
}