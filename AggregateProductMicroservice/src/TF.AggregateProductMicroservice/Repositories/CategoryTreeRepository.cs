using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using TF.Data;

namespace TF.AggregateProductMicroservice
{
    public class CategoryTreeRepository : ICategoryTreeRepository
    {
        private readonly string _uri;
        private readonly string _controller;

        public CategoryTreeRepository(string uri)
        {
            _uri = uri;
            _controller = "api/categorytree/";
        }

        public async Task<IEnumerable<CategoryTree>> All()
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(_uri);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                HttpResponseMessage response = await client.GetAsync(_controller);

                return await response.Content.ReadAsAsync<IEnumerable<CategoryTree>>();
            }
        }

        public async Task<CategoryTree> Get(Guid id)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(_uri);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                HttpResponseMessage response = await client.GetAsync(_controller + id.ToString());

                return await response.Content.ReadAsAsync<CategoryTree>();
            }
        }

        public async Task<IEnumerable<CategoryTree>> GetByParentId(Guid id)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(_uri);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                HttpResponseMessage response = await client.GetAsync(_controller + id.ToString() + "/" + "childs");

                return await response.Content.ReadAsAsync<IEnumerable<CategoryTree>>();
            }
        }
    }
}
