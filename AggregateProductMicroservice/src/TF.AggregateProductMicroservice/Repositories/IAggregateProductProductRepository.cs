using System;
using System.Collections.Generic;
using TF.Data;

namespace TF.AggregateProductMicroservice
{
    public interface IAggregateProductProductRepository
    {
        IEnumerable<AggregateProduct> Get();
    }
}
