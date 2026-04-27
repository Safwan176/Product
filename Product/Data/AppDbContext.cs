using Microsoft.EntityFrameworkCore;

namespace Product.Data
{
    public class AppDbContext: DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Products.Models.Products> Products { get; set; }
        public DbSet<Auth.Models.Auth> Auths { get; set; }
    }
}
