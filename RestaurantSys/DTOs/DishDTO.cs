using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RestaurantSys.DTOs
{
    public class DishDTO
    {

        [Display(Name = "餐點編號")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        [HiddenInput]
        public int DishID { get; set; }

        [Display(Name = "餐點名稱")]
        [StringLength(20, ErrorMessage = "餐點名稱不能超過20個字")]
        [Required(ErrorMessage = "餐點名稱為必填欄位")]
        public string DishName { get; set; } = null!;

        [Display(Name = "餐點描述")]
        [StringLength(50, ErrorMessage = "餐點描述不能超過50個字")]
        [Required(ErrorMessage = "餐點描述不可為空")]
        public string Description { get; set; } = null!;

        [Display(Name = "照片")]
        [StringLength(300, ErrorMessage = "照片路徑最多300字元")]
        public string? PhotoPath { get; set; }

        [Display(Name = "價格")]
        [Required(ErrorMessage = "請設定價格")]
        public decimal DishPrice { get; set; }

        [Display(Name = "備註")]
        [StringLength(40, ErrorMessage = "備註最多40個字")]
        public string? Note { get; set; }

        [Display(Name = "是否啟用")]
        public bool IsActive { get; set; } = true;

        [Display(Name = "餐點類別")]
        public int DishCategoryID { get; set; }
    }
}
