using Microsoft.EntityFrameworkCore;

namespace ASPNETCOREAPI.Entities
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions options): base (options) { }

        public DbSet<Product> Products { get; set;}
    }
}
