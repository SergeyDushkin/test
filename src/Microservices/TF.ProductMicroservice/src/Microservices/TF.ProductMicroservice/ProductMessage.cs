using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace TF.ProductMicroservice
{
    public class ProductMessage
    {
        public ProductMessage()
        {
            this.Links = new List<ProductLink>();
        }

        public ProductMessage(Product product):this()
        {
            this.Id = product.Id;
            this.Type = product.Type;
            this.Name = product.Name;
            this.Links.Add(new ProductLink() { Id = this.Id, Name = this.Name, Type = "self" }); // вообще это здесь не нужно скорее всего, оно должно быть на эндпоинте. Но пусть будет пока
            if (product.Parent!=null)
            {
                this.Links.Add(new ProductLink() { Id = product.Parent.Id, Name = product.Parent.Name, Type = "parent" });
            }

            foreach (var child in product.ChildProducts)
            {
                this.Links.Add(new ProductLink() { Type = child.Type, Name = child.Name, Id = child.Id });
            }
        }

        [JsonProperty(PropertyName = "id")]
        public Guid Id { get; set; }

        [JsonProperty(PropertyName = "type")]
        public string Type { get; set; }

        [JsonProperty(PropertyName = "name")]
        public string Name { get; set; }

        [JsonProperty(PropertyName = "links")]
        public List<ProductLink> Links { get; set; }
    }
}
