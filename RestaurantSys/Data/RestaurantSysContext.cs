using Microsoft.EntityFrameworkCore;
using RestaurantSys.Models;

namespace RestaurantSys.Data
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

        public virtual DbSet<Staff> Staff { get; set; }

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
                entity.Property(e => e.Name).HasMaxLength(40);
                entity.Property(e => e.City).HasMaxLength(10);
                entity.Property(e => e.Address).HasMaxLength(100);
                entity.Property(e => e.Birthday).HasColumnType("date");
                entity.Property(e => e.title).HasMaxLength(10);
            });

            modelBuilder.Entity<MemberTel>(entity =>
            {
                entity.HasKey(e => e.SN).HasName("PK_SN");
                entity.Property(e => e.MemTel).HasMaxLength(20);
                entity.Property(e => e.MemberID).HasMaxLength(9);
            });


        }

    }
}
