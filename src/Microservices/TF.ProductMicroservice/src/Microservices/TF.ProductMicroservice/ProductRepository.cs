using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace TF.ProductMicroservice
{
    public class ProductRepository : IProductRepository
    {
        public IEnumerable<Product> Get()
        {
            List<Product> result = null;
            using (var productContext = new ProductContext())
            {
                result = productContext.Products.ToList();
            }
            if (result == null)
            {
                return new List<Product>();
            }
            return result;
        }

        public Product Get(Guid productId)
        {
            Product result = null;
            using (var productContext = new ProductContext())
            {
                result = productContext.Products.Include("ChildProducts").SingleOrDefault(p => p.Id == productId);
            }
            return result;
        }

        public Product Save(Product product)
        {
            if (product.Id == Guid.Empty)
            {
                return Create(product);
            }
            else
            {
                return Update(product);
            }
        }

        public void Delete(Guid productId)
        {
            Product toDel = null;
            using (var productContext = new ProductContext())
            {
                toDel = productContext.Products.Include("ChildProducts").SingleOrDefault(p => p.Id == productId);
                if (toDel != null)
                {
                    productContext.Products.Remove(toDel);
                    productContext.SaveChanges();
                }
                else
                {
                    throw new ArgumentException();
                }
            }
        }

        private Product Update(Product product)
        {
            using (var productContext = new ProductContext())
            {
                productContext.Products.Attach(product);
                productContext.Entry(product).State = EntityState.Modified;
                productContext.SaveChanges();
                return product;
            }
        }

        private Product Create(Product product)
        {
            using (var productContext = new ProductContext())
            {
                var result = productContext.Products.Add(product);
                productContext.SaveChanges();
                return productContext.Products.Add(result);
            }
        }
    }
}
