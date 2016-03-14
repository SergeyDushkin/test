using System;
using System.Collections.Generic;

namespace TF.Data
{
    public class ProductsCategory
    {
        public Guid Id { get; set; }
        public Guid CategoryId { get; set; }
        public IEnumerable<AggregateProduct> Products { get; set; }
    }
}
