using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

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
                    Id = Guid.NewGuid(),
                    Key = "FOOD",
                    Name = "FOOD",
                    ParentId = Guid.Empty
                }, 
                new CategoryTree{
                    Id = Guid.NewGuid(),
                    Key = "DRINKS",
                    Name = "DRINKS",
                    ParentId = Guid.Empty
                }, 
                new CategoryTree{
                    Id = Guid.NewGuid(),
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
