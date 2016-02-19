using System;
using System.Collections.Generic;

namespace TF.SystemSecurity
{
    public interface IUserService
    {
        void Create(USER User);
        void Update(USER User);
        void Delete(Guid id);

        IEnumerable<USER> GetAll();
        USER GetById(Guid id);   
    }
}
