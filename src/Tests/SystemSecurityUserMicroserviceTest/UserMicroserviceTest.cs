using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Linq;

namespace BusinessCategoryTreeMicroserviceTest
{
    [TestClass]
    public class UserMicroserviceTest
    {
        private readonly string _uri;
        private readonly string _controller;

        public UserMicroserviceTest()
        {
            _uri = "http://localhost:5544/";
            _controller = "api/user/";
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
                var User = new USER
                {
                    GUID_RECORD = id,
                    KEY = "KEY" + id.ToString()
 
                };

                response = await client.PostAsJsonAsync(_controller, User);

                Assert.IsTrue(response.IsSuccessStatusCode, "HTTP POST fail");

                // HTTP GET
                response = await client.GetAsync(_controller + id.ToString());

                Assert.IsTrue(response.IsSuccessStatusCode, "HTTP GET fail");

                var result = await response.Content.ReadAsAsync<USER>();

                Assert.AreEqual(User.GUID_RECORD, result.GUID_RECORD);
                Assert.AreEqual(User.KEY, result.KEY);


                // HTTP PUT
                User.KEY = User.KEY + "Upd";

                response = await client.PutAsJsonAsync(_controller + id.ToString(), User);

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
                var User = new USER
                {
                    GUID_RECORD = id,
                    KEY = "KEY" + id.ToString()

                };

                response = await client.PostAsJsonAsync(_controller, User);

                Assert.IsTrue(response.IsSuccessStatusCode, "HTTP POST fail");
            }
        }

    }

     class USER
    {
        public Guid GUID_RECORD { get; set; }
        public string KEY { get; set; }
        public DateTime LAST_LOGIN { get; set; }
        public Int16 LOGIN_ATTEMPT_COUNT { get; set; }
        public Guid? BATCH_GUID { get; set; }
        public Boolean HIDDEN { get; set; }
        public Boolean DELETED { get; set; }

        public string PROVIDER { get; set; }
        public string KEY_IDENTITY { get; set; }
    }
}
