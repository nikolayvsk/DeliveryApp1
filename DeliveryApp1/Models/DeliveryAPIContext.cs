using Microsoft.EntityFrameworkCore;

namespace DeliveryApp1.Models
{
    public class DeliveryAPIContext : DbContext
    {
        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<Product> Products { get; set; }
        public virtual DbSet<Order> Orders { get; set; }
        public virtual DbSet<Manufacturer> Manufacturers { get; set; }
        public virtual DbSet<DeliveryDriver> DeliveryDrivers { get; set; }

        public DeliveryAPIContext(DbContextOptions<DeliveryAPIContext> options)
            : base(options)
        {
            Database.EnsureCreated();
        }
    }
}
