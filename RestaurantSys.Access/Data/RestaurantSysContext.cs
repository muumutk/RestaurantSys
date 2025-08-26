using Microsoft.EntityFrameworkCore;
using RestaurantSys.Models;

namespace RestaurantSys.Access.Data
{
    public partial class RestaurantSysContext:DbContext
    {
        public RestaurantSysContext(DbContextOptions<RestaurantSysContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Dish> Dish { get; set; }

        public virtual DbSet<Member> Member { get; set; }

        public virtual DbSet<MemberTel> MemberTel { get; set; }

        public virtual DbSet<Employee> Employee { get; set; }

        public virtual DbSet<Supplier> Supplier { get; set; }

        public virtual DbSet<Stock> Stock { get; set; }

        public virtual DbSet<Order> Order { get; set; }

        public virtual DbSet<OrderDetail> OrderDetail { get; set; }

        public virtual DbSet<DishIngredient> DishIngredient { get; set; }



        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Dish>(entity =>
            {
                entity.HasKey(e => e.DishID).HasName("PK_DishID");


                entity.Property(e => e.DishName).HasMaxLength(20);
                entity.Property(e => e.Description).HasMaxLength(50);
                entity.Property(e => e.PhotoPath).HasMaxLength(50);
                entity.Property(e => e.DishPrice).HasColumnType("decimal(18, 2)");
                entity.Property(e => e.Note).HasMaxLength(40);
            });

            modelBuilder.Entity<Member>(entity =>
            {
                entity.HasKey(e => e.MemberID).HasName("PK_MemberID");
                entity.Property(e => e.MemberID).HasMaxLength(9);
                entity.Property(e => e.Name).HasMaxLength(40);
                entity.Property(e => e.City).HasMaxLength(10);
                entity.Property(e => e.Address).HasMaxLength(100);
                entity.Property(e => e.Birthday).HasColumnType("date");
                entity.Property(e => e.title).HasMaxLength(10);
                entity.Property(e => e.Password).HasMaxLength(200);
            });

            modelBuilder.Entity<MemberTel>(entity =>
            {
                entity.HasKey(e => e.SN).HasName("PK_SN");
                entity.Property(e => e.MemTel).HasMaxLength(20);
                entity.Property(e => e.MemberID).HasMaxLength(9);
            });

            modelBuilder.Entity<Employee>(entity =>
            {
                entity.HasKey(e => e.EmployeeID).HasName("PK_StaffID");
                entity.Property(e => e.EmployeeID).HasMaxLength(8);
                entity.Property(e => e.Name).HasMaxLength(40);
                entity.Property(e => e.EmployeeTel).HasMaxLength(20);
                entity.Property(e => e.Address).HasMaxLength(100);
                entity.Property(e => e.Birthday).HasColumnType("date");
                entity.Property(e => e.HireDate).HasColumnType("date");
                entity.Property(e => e.Password).HasMaxLength(200);
            });

            modelBuilder.Entity<Supplier>(entity =>
            {
                entity.HasKey(e => e.SupplierID).HasName("PK_SupplierID");
                entity.Property(e => e.SupplierName).HasMaxLength(20);
                entity.Property(e => e.ContactPerson).HasMaxLength(30);
                entity.Property(e => e.SupplierTel).HasMaxLength(20);
                entity.Property(e => e.Address).HasMaxLength(100);
            });

            modelBuilder.Entity<Stock>(entity =>
            {
                entity.HasKey(e => e.ItemID).HasName("PK_StockID");
                entity.Property(e => e.ItemName).HasMaxLength(10);
                entity.Property(e => e.Unit).HasMaxLength(10);
                entity.Property(e => e.ItemPrice).HasColumnType("decimal(18,2)");
                entity.Property(e => e.IsActive).HasDefaultValue(true);
            });

            modelBuilder.Entity<Order>(entity =>
            {
                entity.HasKey(e => e.OrderID).HasName("PK_OrderID");
                entity.Property(e => e.OrderDate).HasColumnType("date");
                entity.Property(e => e.PickUpTime).HasColumnType("date");
                entity.Property(e => e.Note).HasMaxLength(100);
            });

            modelBuilder.Entity<OrderDetail>(entity =>
            {
                entity.HasKey(e => new { e.OrderID, e.DishID });
                entity.Property(e => e.Price).HasColumnType("decimal(18,2)");
                entity.Property(e => e.GetTime).HasColumnType("date");
            });

            modelBuilder.Entity<DishIngredient>(entity =>
            {
                entity.HasKey(e => new { e.DishID, e.ItemID });
                entity.Property(e => e.IsActive).HasDefaultValue(true);
            });

        }

    }
}
