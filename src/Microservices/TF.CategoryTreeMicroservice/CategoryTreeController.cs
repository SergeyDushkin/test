using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;

namespace TF.CategoryTreeMicroservice
{
    [RoutePrefix("api/categorytree")]
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

        [Route("{id}/childs"), HttpGet]
        public IHttpActionResult GetByParentId(Guid id)
        {
            return Ok(_service.GetByParentId(id));
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

    /*
    [RoutePrefix("categorytree/{id}")]
    public class ChildCategoryTreeController : ApiController
    {
        readonly TF.Business.ICategoryTreeService _service;

        public ChildCategoryTreeController()
        {
            _service = new TF.Business.CategoryTreeService();
        }

        [HttpGet]
        [Route("childs")]
        public IHttpActionResult GetByParentId(Guid id)
        {
            return Ok(_service.GetByParentId(id));
        }
    }*/
}
