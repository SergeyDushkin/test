using System;
using System.Collections.Generic;
using TF.Data;

namespace TF.AggregateProductMicroservice
{
    public interface IProductsCategoryRepository
    {
        IEnumerable<ProductsCategory> Get();
    }
}
