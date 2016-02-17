using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TF.Business
{
    public interface ICategoryTreeService
    {
        void Create(CategoryTree category);
        void Update(CategoryTree category);
        void Delete(Guid id);

        IEnumerable<CategoryTree> GetAll();
        IEnumerable<CategoryTree> GetByParentId(Guid id);   
        CategoryTree GetById(Guid id);
    }
}
