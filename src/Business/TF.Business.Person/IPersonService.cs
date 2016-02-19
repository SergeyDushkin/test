using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TF.Business
{
    public interface IPersonService
    {
        void Create(PERSON category);
        void Update(PERSON category);
        void Delete(Guid id);

        IEnumerable<PERSON> GetAll();
        PERSON GetById(Guid id);   
    }
}
