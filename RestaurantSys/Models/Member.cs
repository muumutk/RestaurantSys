namespace RestaurantSys.Models
{
    public class Member
    {
        public string MemberID { get; set; } = null!;
        
        public string Name { get; set; } = null!;
        
        public string City { get; set; } = null!;
        
        public string Address { get; set; } = null!;

        public DateTime Birthday { get; set; }

        public string title { get; set; } = null!;

        public string Password { get; set; } = null!;

        public virtual List<MemberTel>? MemberTels { get; set; }
        public virtual List<Order>? Orders { get; set; }
    }
}
