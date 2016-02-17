using System;
using System.Web.Http;

namespace TF.PriceMicroservice
{
    public class PriceController : ApiController
    {
        readonly TF.Business.WMS.IPriceService _service;

        public PriceController()
        {
            _service = new TF.Business.WMS.PriceService();
        }

        public IHttpActionResult Get()
        {
            return Ok(_service.GetAll());
        }

        public IHttpActionResult Get(Guid id)
        {
            return Ok(_service.GetById(id));
        }
        
        public IHttpActionResult Post([FromBody]TF.Business.WMS.Price value)
        {
            _service.Create(value);

            return Ok();
        }

        public IHttpActionResult Put(Guid id, [FromBody]TF.Business.WMS.Price value)
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
