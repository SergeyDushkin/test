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

        void AddChild(Guid productId, Guid childId);
        void DeleteChild(Guid productId, Guid childId);

        CategoryTree GetById(Guid id);   
    }
}
