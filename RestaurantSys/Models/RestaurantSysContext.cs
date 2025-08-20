using Microsoft.EntityFrameworkCore;

namespace RestaurantSys.Models
{
    public class RestaurantSysContext:DbContext
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



    }
}
