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

        public virtual ICollection<MemberTel> MemberTels { get; set; } = new List<MemberTel>();
        public virtual ICollection<Order> Orders { get; set; } = new List<Order>();
    }
}
