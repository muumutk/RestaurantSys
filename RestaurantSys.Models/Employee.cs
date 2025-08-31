namespace RestaurantSys.Models
{
    public partial class Employee
    {
        public string EmployeeID { get; set; } = null!;

        public string EName { get; set; } = null!;

        public string EmployeeTel { get; set; } = null!;   

        public string Address { get; set; } = null!;

        public DateTime Birthday { get; set; }

        public DateTime HireDate { get; set; }

        public bool IsEmployed { get; set; } = true;

        public string? EEmail { get; set; }

        public string Password { get; set; } = null!;

        public virtual List<Order>? Orders { get; set; }

        public virtual List<StockBatch>? StockBatches { get; set; }
    }
}
