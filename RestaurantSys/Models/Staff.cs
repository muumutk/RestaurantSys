namespace RestaurantSys.Models
{
    public partial class Staff
    {
        public string StaffID { get; set; } = null!;

        public string Name { get; set; } = null!;

        public string StaffTel { get; set; } = null!;   

        public string Address { get; set; } = null!;

        public DateTime Birthday { get; set; }

        public DateTime HireDate { get; set; }

        public string Password { get; set; } = null!;

        public virtual List<Order>? Orders { get; set; }
    }
}
