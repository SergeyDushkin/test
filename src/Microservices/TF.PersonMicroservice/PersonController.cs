using System;
using System.Web.Http;

namespace TF.PersonMicroservice
{
    public class PersonController : ApiController
    {
        readonly TF.Business.IPersonService _service;

        public PersonController()
        {
            _service = new TF.Business.PersonService();
        }

        public IHttpActionResult Get()
        {
            return Ok(_service.GetAll());
        }

        public IHttpActionResult Get(Guid id)
        {
            return Ok(_service.GetById(id));
        }

        public IHttpActionResult Post([FromBody]TF.Business.PERSON value)
        {
            _service.Create(value);

            return Ok();
        }

        public IHttpActionResult Put(Guid id, [FromBody]TF.Business.PERSON value)
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
