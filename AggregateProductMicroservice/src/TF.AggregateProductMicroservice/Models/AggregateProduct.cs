﻿using System;

namespace TF.AggregateProductMicroservice
{
    public class AggregateProduct
    {
        public Guid Id { get; set; }
        public string Type { get; set; }
        public string Name { get; set; }

        public Price Price { get; set; }
    }
}
