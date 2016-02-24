using System.Data.Entity;

namespace TF.ProductMicroservice
{
    class ProductContext : DbContext
    {
        public DbSet<Product> Products { get; set; }
    }
}
