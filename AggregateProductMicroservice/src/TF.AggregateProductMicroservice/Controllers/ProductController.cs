﻿using Microsoft.Data.OData;
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
    public class ProductsController : ODataController
    {
        private static ODataValidationSettings _validationSettings = new ODataValidationSettings();
        private readonly IAggregateProductProductRepository repository;
        private readonly ILogger logger;

        public ProductsController(IAggregateProductProductRepository repository, ILogger logger)
        {
            this.repository = repository;
            this.logger = logger;

            this.logger.Trace("Call ProductsController");
        }

        [Queryable]
        public async Task<IHttpActionResult> GetProducts(ODataQueryOptions<AggregateProduct> queryOptions)
        {
            this.logger.Trace("Call ProductsController GetProducts");

            try
            {
                queryOptions.Validate(_validationSettings);
            }
            catch (ODataException ex)
            {
                return BadRequest(ex.Message);
            }

            var orders = queryOptions.ApplyTo(repository.Get().AsQueryable<AggregateProduct>());

            //return Ok<PageResult>(new PageResult<AggregateProduct>(orders as IEnumerable<AggregateProduct>, Request.GetNextPageLink(), Request.GetInlineCount()));
            return Ok<IQueryable<AggregateProduct>>((IQueryable<AggregateProduct>)orders);
        }

        [Queryable]
        public async Task<IHttpActionResult> GetProduct([FromODataUri] System.Guid key, ODataQueryOptions<AggregateProduct> queryOptions)
        {
            this.logger.Trace("Call ProductsController GetProduct");

            try
            {
                queryOptions.Validate(_validationSettings);
            }
            catch (ODataException ex)
            {
                return BadRequest(ex.Message);
            }

            // return Ok<Order>(order);
            return StatusCode(HttpStatusCode.NotImplemented);
        }

        protected override void Dispose(bool disposing)
        {
            this.logger.Trace("End ProductsController");

            base.Dispose(disposing);
        }
    }
}
