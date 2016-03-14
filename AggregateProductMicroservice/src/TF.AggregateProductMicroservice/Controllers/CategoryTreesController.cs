using Microsoft.Data.OData;
using NLog;
using System.Collections;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

using System.Net.Http;
using System.Web.Http;
using System.Web.Http.OData;
using System.Web.Http.OData.Query;
using System.Collections.Generic;

namespace TF.AggregateProductMicroservice.Controllers
{
    public class CategoryTreesController : ODataController
    {
        private static ODataValidationSettings _validationSettings = new ODataValidationSettings();
       
        private readonly ICategoryTreeRepository repository;
        private readonly ILogger logger;
        private readonly System.Diagnostics.StackTrace stackTrace;

        public CategoryTreesController(ICategoryTreeRepository repository, ILogger logger)
        {
            this.repository = repository;
            this.logger = logger;

            this.stackTrace = new System.Diagnostics.StackTrace();

            this.logger.Trace("Call CategoryTreesController");
        }

        [Queryable]
        public async Task<IHttpActionResult> GetCategoryTrees(ODataQueryOptions<CategoryTree> queryOptions)
        {
            this.logger.Trace("Call CategoryTreesController GetCategoryTrees");

            try
            {
                queryOptions.Validate(_validationSettings);
            }
            catch (ODataException ex)
            {
                return BadRequest(ex.Message);
            }

            var orders = (IQueryable<CategoryTree>)queryOptions
                .ApplyTo(repository.Get().AsQueryable<CategoryTree>());

            return Ok<IQueryable<CategoryTree>>(orders);
        }

        [Queryable]
        public async Task<IHttpActionResult> GetCategoryTree([FromODataUri] System.Guid key, ODataQueryOptions<AggregateProduct> queryOptions)
        {
            this.logger.Trace("Call CategoryTreesController GetCategoryTree");

            try
            {
                queryOptions.Validate(_validationSettings);
            }
            catch (ODataException ex)
            {
                return BadRequest(ex.Message);
            }

            return StatusCode(HttpStatusCode.NotImplemented);
        }

        protected override void Dispose(bool disposing)
        {
            this.logger.Trace("End CategoryTreesController");

            base.Dispose(disposing);
        }
    }
}
