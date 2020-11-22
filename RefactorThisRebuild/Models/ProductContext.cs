
using Microsoft.EntityFrameworkCore;

//using System.Data.Entity;

namespace RefactorThisRebuild.Models
{
    public class ProductContext : DbContext
    {
        public ProductContext(DbContextOptions<ProductContext> options) : base(options)
        {
        }

        public DbSet<Product> Products {get; set;}

        public DbSet<ProductOption> ProductOptions { get; set; }


    }
}

