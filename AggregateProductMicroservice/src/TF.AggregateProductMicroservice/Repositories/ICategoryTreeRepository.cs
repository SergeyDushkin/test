using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TF.Data;

namespace TF.AggregateProductMicroservice
{
    public interface ICategoryTreeRepository
    {
        Task<IEnumerable<CategoryTree>> All();
        Task<CategoryTree> Get(Guid id);
        Task<IEnumerable<CategoryTree>> GetByParentId(Guid id);
    }
}
