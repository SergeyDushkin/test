﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TF.AggregateProductMicroservice
{
    public interface IAggregateProductProductRepository
    {
        IEnumerable<AggregateProduct> Get();
    }
}