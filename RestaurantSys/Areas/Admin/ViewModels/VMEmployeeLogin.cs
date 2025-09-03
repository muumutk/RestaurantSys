using System.ComponentModel.DataAnnotations;

namespace RestaurantSys.Areas.Admin.ViewModels
{
    public class VMEmployeeLogin
    {
        [Display(Name ="員工編號：")]
        public string EmployeeID { get; set; } = null!;

        [Display(Name ="密碼：")]
        [DataType(DataType.Password)]
        public string Password { get; set; } = null!;
        }
    }
