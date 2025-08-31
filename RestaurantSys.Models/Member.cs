using System.ComponentModel.DataAnnotations;

namespace RestaurantSys.Models
{
    public partial class Member
    {
        [Key]
        public string MemberID { get; set; } = null!;
        
        public string Name { get; set; } = null!;
        
        public string MemberTel { get; set; } = null!;

        public string City { get; set; } = null!;
        
        public string Address { get; set; } = null!;

        public DateTime? Birthday { get; set; }

        public string title { get; set; } = null!;

        public string? MEmail { get; set; }

        public string Password { get; set; } = null!;

        public virtual List<Order>? Orders { get; set; }
    }
}
