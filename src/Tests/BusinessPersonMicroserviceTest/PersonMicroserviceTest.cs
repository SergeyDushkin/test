using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Linq;

namespace BusinessPersonMicroserviceTest
{
    [TestClass]
    public class PersonMicroserviceTest
    {
        private readonly string _uri;
        private readonly string _controller;

        public PersonMicroserviceTest()
        {
            _uri = "http://localhost:5544/";
            _controller = "api/person/";
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
                var Person = new PERSON
                {
                    GUID_RECORD = id,
                    FIRSTNAME = "AS " + id.ToString(),
                    LASTNAME = "SE " + id.ToString() ,
                    BIRTHDATE = DateTime.Now

                };

                response = await client.PostAsJsonAsync(_controller, Person);

                Assert.IsTrue(response.IsSuccessStatusCode, "HTTP POST fail");

                // HTTP GET
                response = await client.GetAsync(_controller + id.ToString());

                Assert.IsTrue(response.IsSuccessStatusCode, "HTTP GET fail");

                var result = await response.Content.ReadAsAsync<PERSON>();

                Assert.AreEqual(Person.GUID_RECORD, result.GUID_RECORD);
                Assert.AreEqual(Person.FIRSTNAME, result.FIRSTNAME);
                Assert.AreEqual(Person.LASTNAME, result.LASTNAME);

                // HTTP PUT
                Person.FIRSTNAME = Person.FIRSTNAME + "GGGG";

                response = await client.PutAsJsonAsync(_controller + id.ToString(), Person);

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
                var Person = new PERSON
                {
                    GUID_RECORD = id,
                    FIRSTNAME = "AS " + id.ToString(),
                    LASTNAME = "SE " + id.ToString(),
                    BIRTHDATE = DateTime.Now

                };

                response = await client.PostAsJsonAsync(_controller, Person);

                Assert.IsTrue(response.IsSuccessStatusCode, "HTTP POST fail");
            }
        }

    }

    public class PERSON
    {
        public Guid GUID_RECORD { get; set; }
        public string FIRSTNAME { get; set; }
        public string LASTNAME { get; set; }
        public string MIDNAME { get; set; }
        public DateTime? BIRTHDATE { get; set; }
        public Guid? USER_GUID { get; set; }

        public Guid? BATCH_GUID { get; set; }
        public Boolean HIDDEN { get; set; }
        public Boolean DELETED { get; set; }
    }
}
