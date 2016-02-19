using System;
using System.Web.Http;

namespace TF.UserMicroservice
{
    public class UserController : ApiController
    {
        readonly TF.SystemSecurity.IUserService _service;

        public UserController()
        {
            _service = new TF.SystemSecurity.UserService();
        }

        public IHttpActionResult Get()
        {
            return Ok(_service.GetAll());
        }

        public IHttpActionResult Get(Guid id)
        {
            return Ok(_service.GetById(id));
        }

        public IHttpActionResult Post([FromBody]TF.SystemSecurity.USER value)
        {
            _service.Create(value);

            return Ok();
        }

        public IHttpActionResult Put(Guid id, [FromBody]TF.SystemSecurity.USER value)
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
