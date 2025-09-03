using System.ComponentModel.DataAnnotations;

namespace RestaurantSys.ViewModels
{
    public class VMMemberLogin
    {
        [Display(Name ="電子郵件或手機號碼：")]
        public string EmailOrPhone { get; set; } = null!;

        [Display(Name ="密碼：")]
        [DataType(DataType.Password)]
        public string Password { get; set; } = null!;
    }
}
