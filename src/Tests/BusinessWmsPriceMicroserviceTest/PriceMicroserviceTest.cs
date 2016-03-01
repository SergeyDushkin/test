using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Net.Http;
using System.Threading.Tasks;
using System.Net.Http.Headers;

namespace BusinessWmsPriceMicroserviceTest
{
    [TestClass]
    public class PriceMicroserviceTest
    {

        private readonly string _uri;
        private readonly string _controller;

        public PriceMicroserviceTest()
        {
            _uri = "http://localhost:5533/";
            _controller = "api/price/";
        }

        [TestMethod]
        public async Task CrudTest()
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(_uri);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                HttpResponseMessage response;

                // HTTP POST
                var id = Guid.NewGuid();
                var value = new Price
                {
                    Id = id,
                    ProductId = Guid.NewGuid(),
                    ProductPrice = 10
                };

                response = await client.PostAsJsonAsync(_controller, value);

                Assert.IsTrue(response.IsSuccessStatusCode, "HTTP POST fail");

                // HTTP GET
                response = await client.GetAsync(_controller + id.ToString());

                Assert.IsTrue(response.IsSuccessStatusCode, "HTTP GET fail");

                var result = await response.Content.ReadAsAsync<Price>();

                Assert.AreEqual(value.Id, result.Id);
                Assert.AreEqual(value.ProductId, result.ProductId);
                Assert.AreEqual(value.ProductPrice, result.ProductPrice);

                // HTTP PUT
                value.ProductPrice = 20;

                response = await client.PutAsJsonAsync(_controller + id.ToString(), value);

                Assert.IsTrue(response.IsSuccessStatusCode, "HTTP PUT fail");

                // HTTP DELETE
                response = await client.DeleteAsync(_controller + id.ToString());

                Assert.IsTrue(response.IsSuccessStatusCode, "HTTP DELETE fail");
            }
        }

    }
}
