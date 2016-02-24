using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TF.AggregateProductMicroservice
{
    public class AggregateProductRepository : IAggregateProductProductRepository
    {
    }

    public class AggregateProduct
    {
        public Guid Id { get; set; }


    }
}
