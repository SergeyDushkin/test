using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TF.Data;

namespace TF.AggregateProductMicroservice
{
    public class CategoryTreeRepository : ICategoryTreeRepository
    {
        private readonly IEnumerable<CategoryTree> _data;

        public CategoryTreeRepository()
        {
            _data = new[] { 
                new CategoryTree{
                    Id = Guid.Empty,
                    Key = "MENU",
                    Name = "MENU"
                }, 
                new CategoryTree{
                    Id = new Guid("be2ebf01-4846-498f-92c5-33e8a3882a4d"),
                    Key = "FOOD",
                    Name = "FOOD",
                    ParentId = Guid.Empty
                }, 
                new CategoryTree{
                    Id = new Guid("940f2a24-7461-41a3-bb3b-7c8b1f6f235f"),
                    Key = "DRINKS",
                    Name = "DRINKS",
                    ParentId = Guid.Empty
                }, 
                new CategoryTree{
                    Id = new Guid("248f532f-4766-427e-97e9-5dc3a8249fed"),
                    Key = "SNACKS",
                    Name = "SNACKS",
                    ParentId = Guid.Empty
                }
            };
        }

        public IEnumerable<CategoryTree> Get()
        {
            return _data;
        }
    }
}
