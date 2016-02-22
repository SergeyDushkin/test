using System;
using System.Collections.Generic;
using System.Web.Http;

namespace TF.ProductMicroservice
{
    public class ProductsController : ApiController
    {
        IProductRepository _repository;

        public ProductsController(IProductRepository repository)
        {
            _repository = repository;
        }

        public IHttpActionResult Get() //все списал у Сереги. Нужно больше проверок...
        {
            IEnumerable<Product> products = _repository.Get();
            return Ok(Product.CreateMessages(products));
        }

        public IHttpActionResult Get(Guid id)
        {
            var product = _repository.Get(id);
            if (product == null)
            {
                return NotFound();
            }
            return Ok(product.GetMessage());
        }

        public IHttpActionResult Post([FromBody]Product product)
        {
            return Ok(_repository.Save(product).GetMessage());
        }

        public IHttpActionResult Put(Guid id, [FromBody]Product product)
        {
           return  Ok(_repository.Save(product).GetMessage());
        }

        public IHttpActionResult Delete(Guid id)
        {
            try
            {
                _repository.Delete(id);
            }
            catch (ArgumentException)
            {
                return NotFound();
            }
            return Ok();
        } 
    }
}
