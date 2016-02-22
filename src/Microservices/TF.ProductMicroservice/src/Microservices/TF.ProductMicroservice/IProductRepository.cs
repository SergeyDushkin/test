using System;
using System.Collections.Generic;

namespace TF.ProductMicroservice
{
    public interface IProductRepository
    {
        Product Get(Guid productId);
        Product Save(Product product);
        IEnumerable<Product> Get();
        void Delete(Guid id);
    }
}