using System;
using System.Collections.Generic;
using TF.Data;

namespace TF.AggregateProductMicroservice
{
    public class ProductsCategoryRepository : IProductsCategoryRepository
    {
        private readonly IEnumerable<ProductsCategory> _data;

        public ProductsCategoryRepository()
        {
            _data = new[] { 
                new ProductsCategory{
                    Id = new Guid("1bd21bed-1869-484f-b19f-f158ef5b6833"),
                    CategoryId = new Guid("be2ebf01-4846-498f-92c5-33e8a3882a4d"),
                    Products = new [] {
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
                        }
                    }
                },  
                new ProductsCategory{
                    Id = new Guid("8729d78b-87ea-4c1d-ba60-c330b8025233"),
                    CategoryId = new Guid("248f532f-4766-427e-97e9-5dc3a8249fed"),
                    Products = new [] {
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
                        }
                    }
                }
            };
        }

        public IEnumerable<ProductsCategory> Get()
        {
            return _data;
        }
    }
}
