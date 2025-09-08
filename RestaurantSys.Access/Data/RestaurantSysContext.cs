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

        public virtual DbSet<DishCategory> DishCategory { get; set; }

        public virtual DbSet<Member> Member { get; set; }

        public virtual DbSet<Employee> Employee { get; set; }

        public virtual DbSet<Supplier> Supplier { get; set; }

        public virtual DbSet<Stock> Stock { get; set; }

        public virtual DbSet<StockBatch> StockBatch { get; set; }

        public virtual DbSet<Order> Order { get; set; }

        public virtual DbSet<OrderDetail> OrderDetail { get; set; }

        public virtual DbSet<DishIngredient> DishIngredient { get; set; }

        public virtual DbSet<StockBatchWarningLog> StockBatchWarningLog { get; set; }



        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Dish>(entity =>
            {
                entity.HasKey(e => e.DishID).HasName("PK_DishID");
                entity.Property(e => e.DishName).HasMaxLength(20);
                entity.Property(e => e.Description).HasMaxLength(50);
                entity.Property(e => e.PhotoPath).HasMaxLength(300);
                entity.Property(e => e.DishPrice).HasColumnType("decimal(18, 2)");
                entity.Property(e => e.Note).HasMaxLength(40);
            });

            modelBuilder.Entity<DishCategory>(entity =>
            {
                entity.HasKey(e => e.DishCategoryID).HasName("PK_DishCategoryID");
                entity.Property(e => e.DishCategoryName).HasMaxLength(20);
            });

            modelBuilder.Entity<Member>(entity =>
            {
                entity.HasKey(e => e.MemberID).HasName("PK_MemberID");
                entity.Property(e => e.MemberID).HasMaxLength(9);
                entity.Property(e => e.Name).HasMaxLength(40);
                entity.Property(e => e.MemberTel).HasMaxLength(20);
                entity.Property(e => e.City).HasMaxLength(10);
                entity.Property(e => e.Address).HasMaxLength(100);
                entity.Property(e => e.Birthday).HasColumnType("date");
                entity.Property(e => e.title).HasMaxLength(10);
                entity.Property(e => e.MEmail).HasMaxLength(40);
                entity.Property(e => e.Password).HasMaxLength(200);
                entity.Property(e => e.AvatarUrl).HasMaxLength(300);
            });

            modelBuilder.Entity<Employee>(entity =>
            {
                entity.HasKey(e => e.EmployeeID).HasName("PK_EmployeeID");
                entity.Property(e => e.EmployeeID).HasMaxLength(8);
                entity.Property(e => e.EName).HasMaxLength(40);
                entity.Property(e => e.EmployeeTel).HasMaxLength(20);
                entity.Property(e => e.Address).HasMaxLength(100);
                entity.Property(e => e.Birthday).HasColumnType("date");
                entity.Property(e => e.HireDate).HasColumnType("date");
                entity.Property(e => e.EEmail).HasMaxLength(40);
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
                entity.Property(e => e.ItemName).HasMaxLength(20);
                entity.Property(e => e.Unit).HasMaxLength(10);
                entity.Property(e => e.ItemPrice).HasColumnType("decimal(18,2)");
                entity.Property(e => e.IsActive).HasDefaultValue(true);
            });

            modelBuilder.Entity<StockBatch>(entity =>
            {
                entity.HasKey(e => new { e.ItemID,e.EmployeeID });
                entity.HasKey(e => e.BatchID).HasName("PK_BatchID");
                entity.Property(e => e.BatchNo).HasMaxLength(15);
                entity.Property(e => e.Quantity);
                entity.Property(e => e.ItemPrice).HasColumnType("decimal(18,2)");
                entity.Property(e => e.ArrivalDate).HasColumnType("date");
                entity.Property(e => e.ExpiryDate).HasColumnType("date");
            });

            modelBuilder.Entity<StockBatchWarningLog>(entity =>
            {
                entity.HasKey(e => e.StockBatchWarningLogID).HasName("PK_StockBatchWarningLogID");
                entity.Property(e => e.WarningSentDate).HasColumnType("datetime");
            });

            modelBuilder.Entity<Order>(entity =>
            {
                entity.HasKey(e => e.OrderID).HasName("PK_OrderID");
                entity.Property(e => e.OrderID).HasMaxLength(12);
                entity.Property(e => e.OrderDate).HasColumnType("datetime");
                entity.Property(e => e.PickUpTime).HasColumnType("datetime");
                entity.Property(e => e.Note).HasMaxLength(100);
            });

            modelBuilder.Entity<OrderDetail>(entity =>
            {
                entity.HasKey(e => new { e.OrderID, e.DishID });
                entity.Property(e => e.UnitPrice).HasColumnType("decimal(18,2)");
                entity.Property(e => e.GetTime).HasColumnType("datetime");
            });

            modelBuilder.Entity<DishIngredient>(entity =>
            {
                entity.HasKey(e => new { e.DishID, e.ItemID });
                entity.Property(e => e.IsActive).HasDefaultValue(true);
            });

        }

    }
}
