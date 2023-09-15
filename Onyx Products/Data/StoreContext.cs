using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using System.Reflection.Metadata;
using Onyx.Products.Models;

namespace Onyx.Products.Data;

public class StoreContext : DbContext
{
    public StoreContext(DbContextOptions options) : base(options) { }

    public DbSet<Product> Products { get; set; } = null!;

    //public string DbPath { get; }

    //public StoreContext()
    //{
    //    DbPath = Path.Join(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "store.db");
    //}

    //protected override void OnModelCreating(ModelBuilder modelBuilder)
    //{
    //    modelBuilder.Entity<Product>().ToTable("Product");
    //}
}