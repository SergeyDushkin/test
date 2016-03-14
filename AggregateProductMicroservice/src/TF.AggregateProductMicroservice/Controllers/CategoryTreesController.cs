﻿using Microsoft.Data.OData;
using NLog;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.OData;
using System.Web.Http.OData.Query;
using TF.Data;

namespace TF.AggregateProductMicroservice.Controllers
{
    public class CategoryTreesController : ODataController
    {
        private static ODataValidationSettings _validationSettings = new ODataValidationSettings();

        private readonly ICategoryTreeRepository repository;
        private readonly IProductsCategoryRepository productsCategoryRepository;
        private readonly ILogger logger;

        public CategoryTreesController(
            ICategoryTreeRepository repository,
            IProductsCategoryRepository productsCategoryRepository, 
            ILogger logger)
        {
            this.repository = repository;
            this.productsCategoryRepository = productsCategoryRepository;
            this.logger = logger;

            this.logger.Trace("Call CategoryTreesController");
        }

        [EnableQuery]
        public async Task<IHttpActionResult> GetCategoryTrees(ODataQueryOptions<CategoryTree> queryOptions)
        {
            logger.Trace("Call CategoryTreesController GetCategoryTrees");

            try
            {
                queryOptions.Validate(_validationSettings);
            }
            catch (ODataException ex)
            {
                return BadRequest(ex.Message);
            }

            var orders = (IQueryable<CategoryTree>)queryOptions
                .ApplyTo(repository.Get().AsQueryable());

            return Ok(orders);
        }

        public async Task<IHttpActionResult> GetCategoryTree([FromODataUri] System.Guid key)
        {
            logger.Trace("Call CategoryTreesController GetCategoryTree");

            var query = repository
                .Get()
                .SingleOrDefault(r => r.Id == key);

            return Ok(query);
        }

        public async Task<IHttpActionResult> GetProducts([FromODataUri] System.Guid key)
        {
            logger.Trace("Call CategoryTreesController GetProducts");

            var query = productsCategoryRepository
                .Get()
                .SingleOrDefault(r => r.CategoryId == key)
                .Products;

            return Ok(query);
        }

        protected override void Dispose(bool disposing)
        {
            logger.Trace("End CategoryTreesController");

            base.Dispose(disposing);
        }
    }
}
