using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BusinessWmsPriceMicroserviceTest
{
    public class Price
    {
        public Guid Id { get; set; }
        public Guid ProductId { get; set; }
        public double ProductPrice { get; set; }
    }
}
