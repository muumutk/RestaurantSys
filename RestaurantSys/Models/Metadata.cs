using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace RestaurantSys.Models
{
    public class DishData
    {
        [Display(Name ="餐點編號")]
        [Key]
        [HiddenInput]
        public int DishID { get; set; }

        [Display(Name = "餐點名稱")]
        [StringLength(20,ErrorMessage = "餐點名稱不能超過20個字元")]
        [Required(ErrorMessage = "餐點名稱為必填欄位")]
        public string DishName { get; set; } = null!;

        [Display(Name = "餐點描述")]
        [StringLength(50, ErrorMessage = "餐點描述不能超過50個字元")]
        public string Description { get; set; } = null!;

        [Display(Name = "照片")]
        [StringLength(50)]
        public string? PhotoPath { get; set; }

        [Display(Name = "價格")]
        public decimal Price { get; set; }

        [Display(Name = "備註")]
        [StringLength(40, ErrorMessage = "備註最多40個字元")]
        public string? Note { get; set; }
    }
}
