﻿using System;
using System.Collections.Generic;
using TF.Data;

namespace TF.AggregateProductMicroservice
{
    public interface ICategoryTreeRepository
    {
        IEnumerable<CategoryTree> Get();
    }
}
