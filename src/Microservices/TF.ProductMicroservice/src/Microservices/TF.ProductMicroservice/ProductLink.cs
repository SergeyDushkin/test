using System;

namespace TF.ProductMicroservice
{
    public class ProductLink
    {
        private static readonly string apiURI = "http://localhost:5555/api/products/"; // взять откуда-то..

        public string Name { get; set; }
        public string Type { get; set; }
        public Guid Id { get; set; }
        public string Link
        {
            get
            {
                return apiURI + this.Id.ToString();
            }
            set { }
        }
    }
}
