using System;

namespace TF.Data
{
    public class CategoryTree
    {
        public Guid Id { get; set; }
        public string Key { get; set; }
        public string Name { get; set; }

        public Guid? ParentId { get; set; }

        public AggregateProduct[] Products { get; set; }
        public CategoryTree[] Childs { get; set; }
    }
}
