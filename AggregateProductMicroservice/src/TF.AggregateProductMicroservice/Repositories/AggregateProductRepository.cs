using System;
using System.Collections.Generic;
using TF.Data;

namespace TF.AggregateProductMicroservice
{
    public class AggregateProductRepository : IAggregateProductProductRepository
    {
        private readonly IEnumerable<AggregateProduct> _data;

        public AggregateProductRepository()
        {
            _data = new[] { 
                new AggregateProduct()
                {
                    Id = Guid.NewGuid(),
                    Name = "WOK",
                    Type = "REGULAR",
                    Price = new Price { ProductPrice = 10}
                }, 
                new AggregateProduct()
                {
                    Id = Guid.NewGuid(),
                    Name = "WOK V2",
                    Type = "REGULAR",
                    Price = new Price { ProductPrice = 5}
                }, 
                new AggregateProduct()
                {
                    Id = Guid.NewGuid(),
                    Name = "NOODLE V2",
                    Type = "KIT"
                }, 
                new AggregateProduct()
                {
                    Id = Guid.NewGuid(),
                    Name = "NOODLE V3",
                    Type = "KIT"
                }
            };
        }

        public IEnumerable<AggregateProduct> Get()
        {
            return _data;
        }
    }
}
