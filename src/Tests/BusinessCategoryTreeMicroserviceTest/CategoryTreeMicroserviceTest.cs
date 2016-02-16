using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Linq;

namespace BusinessCategoryTreeMicroserviceTest
{
    [TestClass]
    public class CategoryTreeMicroserviceTest
    {
        private readonly string _uri;
        private readonly string _controller;

        public CategoryTreeMicroserviceTest()
        {
            _uri = "http://localhost:5544/";
            _controller = "api/categorytree/";
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
                var category = new CategoryTree
                {
                    Id = id,
                    Key = "Key" + id.ToString(),
                    Name = "Name" + id.ToString()
                };

                response = await client.PostAsJsonAsync(_controller, category);

                Assert.IsTrue(response.IsSuccessStatusCode, "HTTP POST fail");

                // HTTP GET
                response = await client.GetAsync(_controller + id.ToString());

                Assert.IsTrue(response.IsSuccessStatusCode, "HTTP GET fail");

                var result = await response.Content.ReadAsAsync<CategoryTree>();

                Assert.AreEqual(category.Id, result.Id);
                Assert.AreEqual(category.Key, result.Key);
                Assert.AreEqual(category.Name, result.Name);
                Assert.AreEqual(category.ParentId, result.ParentId);

                // HTTP PUT
                category.Name = category.Name + "Upd";

                response = await client.PutAsJsonAsync(_controller + id.ToString(), category);

                Assert.IsTrue(response.IsSuccessStatusCode, "HTTP PUT fail");

                // HTTP DELETE
                response = await client.DeleteAsync(_controller + id.ToString());

                Assert.IsTrue(response.IsSuccessStatusCode, "HTTP DELETE fail");
            }
        }

        [TestMethod]
        public async Task PostTest()
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(_uri);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                HttpResponseMessage response;

                // HTTP POST
                var id = Guid.NewGuid();
                var category = new CategoryTree
                {
                    Id = id,
                    Key = "Key" + id.ToString(),
                    Name = "Name" + id.ToString()
                };

                response = await client.PostAsJsonAsync(_controller, category);

                Assert.IsTrue(response.IsSuccessStatusCode, "HTTP POST fail");
            }
        }

        [TestMethod]
        public async Task PerfomanceCrudTest()
        {
            /// Нормально не работает
            System.Linq.Enumerable.Range(0, 10000).ToList().ForEach(async r =>
            {
                await PostTest();
            });

            /*
            Parallel.For(0, 10000, async (i) =>
            {
                await PostTest();
            });*/
        }

    }

    class CategoryTree
    {
        public Guid Id { get; set; }
        public string Key { get; set; }
        public string Name { get; set; }

        public Guid? ParentId { get; set; }
    }
}
