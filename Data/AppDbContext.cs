using Microsoft.EntityFrameworkCore;
using MyBookOrder.Models;

namespace MyBookOrder.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }
        public DbSet<Book> Book { get; set; }
        public DbSet<Order> Order { get; set; }
    }
}
