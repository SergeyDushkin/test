using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Net.Http;
using System.Net.Http.Headers;
using ProductModuleTests;
using System.Threading.Tasks;
using TF.ProductMicroservice;

namespace ProductRestApiTests
{
    [TestClass]
    public class ProductsApiTests
    {
        public const string serviceUri = "http://localhost:5555/api/products";

        [TestMethod]
        public async Task SimplePOSTTest()
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(serviceUri);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                HttpResponseMessage response;
                var product = RepositoryTests.GetSimpleProduct();

                response = await client.PostAsJsonAsync(serviceUri, product);
                Assert.IsTrue(response.IsSuccessStatusCode, "HTTP POST fails");
            }
        }

        [TestMethod]
        public async Task SimplePUTTest()
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(serviceUri);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                HttpResponseMessage response;
                var product = RepositoryTests.GetSimpleProduct();

                response = await client.PostAsJsonAsync(serviceUri, product);
                ProductMessage postResult = await response.Content.ReadAsAsync<ProductMessage>();

                response = await client.PutAsJsonAsync(serviceUri + '/' + postResult.Id, postResult);
                Assert.IsTrue(response.IsSuccessStatusCode, "HTTP PUT fails");
            }
        }

        [TestMethod]
        public async Task SimpleDELETETest()
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(serviceUri);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                HttpResponseMessage response;
                var product = RepositoryTests.GetSimpleProduct();

                response = await client.PostAsJsonAsync(serviceUri, product);
                ProductMessage postResult = await response.Content.ReadAsAsync<ProductMessage>();

                response = await client.DeleteAsync(serviceUri + '/' + postResult.Id);
                Assert.IsTrue(response.IsSuccessStatusCode, "HTTP DELETE fails");
            }
        }

        [TestMethod]
        public async Task SimpleGETTest()
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(serviceUri);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                HttpResponseMessage response;
                var product = RepositoryTests.GetSimpleProduct();

                response = await client.GetAsync(serviceUri);
                Assert.IsTrue(response.IsSuccessStatusCode, "HTTP GET fails");
            }
        }
    }
}
