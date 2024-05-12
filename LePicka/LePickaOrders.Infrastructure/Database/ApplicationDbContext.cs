using LePickaOrders.Domain.Orders;
using LePickaOrders.Domain.Products;
using LePickaOrders.Domain.Users;
using Microsoft.EntityFrameworkCore;

namespace LePickaOrders.Infrastructure.Database
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Order> Orders { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
        }
    }
}
