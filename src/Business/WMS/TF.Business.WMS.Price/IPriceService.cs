using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TF.Business.WMS
{
    public interface IPriceService
    {
        void Create(Price price);
        void Update(Price price);

        void Delete(Guid id);
        void DeleteByProduct(Guid id);

        IEnumerable<Price> GetAll();

        Price GetByProductId(Guid id);
        Price GetById(Guid id);
    }
}
