using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Linq;
using System.Diagnostics;
using System.Collections.Generic;

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
        public void PostTest()
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

                response = client.PostAsJsonAsync(_controller, category).Result;

                System.Diagnostics.Debug.Print("PostTest " + DateTime.Now.ToString("MM/dd/yyyy hh:mm:ss.fff tt") + " " + response.IsSuccessStatusCode);

                Assert.IsTrue(response.IsSuccessStatusCode, "HTTP POST fail");
            }
        }

        [TestMethod]
        public async Task PerfomanceParallelCreateTest()
        {
            System.Diagnostics.Debug.Print("Start " + DateTime.Now.ToShortTimeString());

            Stopwatch stopwatch = new Stopwatch();
            var watch = new List<TimeSpan>();

            Parallel.For(0, 1000, (i) =>
            {
                stopwatch.Start();

                PostTest();

                stopwatch.Stop();
                watch.Add(stopwatch.Elapsed);
                stopwatch.Reset();
            });


            var count = watch.Count();
            var avg = watch.Average(r => r.Milliseconds);
            var min = watch.Min(r => r.Milliseconds);
            var max = watch.Max(r => r.Milliseconds);

            System.Diagnostics.Debug.Print("avg: {0}, min: {1}, max: {2}, count: {3}", avg, min, max, count);
            System.Diagnostics.Debug.Print("End " + DateTime.Now.ToShortTimeString());
        }

        [TestMethod]
        public async Task PerfomanceCreateTest()
        {
            System.Diagnostics.Debug.Print("Start " + DateTime.Now.ToShortTimeString());

            Stopwatch stopwatch = new Stopwatch();
            var watch = new List<TimeSpan>();

            System.Linq.Enumerable.Range(0, 1000).ToList().ForEach(r => {
                stopwatch.Start();
                PostTest();

                stopwatch.Stop();
                watch.Add(stopwatch.Elapsed);
                stopwatch.Reset();
            });

            var avg = watch.Average(r => r.Milliseconds);
            var min = watch.Min(r => r.Milliseconds);
            var max = watch.Max(r => r.Milliseconds);

            System.Diagnostics.Debug.Print("avg: {0}, min: {1}, max: {2} ", avg, min, max);
            System.Diagnostics.Debug.Print("End " + DateTime.Now.ToShortTimeString());
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
