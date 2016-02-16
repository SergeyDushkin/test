using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;

namespace TF.CategoryTreeMicroservice
{
    public class CategoryTreeController : ApiController
    {
        readonly TF.Business.ICategoryTreeService _service;

        public CategoryTreeController()
        {
            _service = new TF.Business.CategoryTreeService();
        }

        public IHttpActionResult Get()
        {
            return Ok(_service.GetAll());
        }

        public IHttpActionResult Get(Guid id)
        {
            return Ok(_service.GetById(id));
        }

        public IHttpActionResult Post([FromBody]TF.Business.CategoryTree value)
        {
            _service.Create(value);

            return Ok();
        }

        public IHttpActionResult Put(Guid id, [FromBody]TF.Business.CategoryTree value)
        {
            _service.Update(value);

            return Ok();
        }

        public IHttpActionResult Delete(Guid id)
        {
            _service.Delete(id);

            return Ok();
        }
    }
}
