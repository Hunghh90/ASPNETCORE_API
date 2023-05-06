using Microsoft.EntityFrameworkCore;


namespace ASPNETCORE.Entities
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions options): base(options)
        {
        }

        public DbSet<Product> Products { get; set; }
    }
}
