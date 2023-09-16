using Microsoft.EntityFrameworkCore;
using Onyx.Products.Models;

namespace Onyx.Products.Data;

public class StoreContext : DbContext
{
    public StoreContext(DbContextOptions options) : base(options) { }

    public DbSet<Product> Products { get; set; } = null!;
}