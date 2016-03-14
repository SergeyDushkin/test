using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using System.Data.Services.Client;
using System.Linq;

namespace TF.MobileClient
{
    [TestClass]
    public class ClientUnitTest
    {
        [TestMethod]
        public void GetMainPageItemsTest()
        {

        }

        [TestMethod]
        public void GetCatalogItemsTest()
        {
            var container = new TF.Container(new Uri("http://localhost:5588/odata/"));

            var data = container.CategoryTrees.ToList();

            container.CategoryTrees.Single(r => r.Id == Guid.Parse("be2ebf01-4846-498f-92c5-33e8a3882a4d")).Products;

            Assert.IsNotNull(data);
        }

        [TestMethod]
        public void GetItemTest()
        {
            var container = new TF.Container(new Uri("http://localhost:5588/odata/"));

            var data = container
                .Products
                .Where(r => r.Name == "WOK")
                .ToList();

            var alldata = container
                .CreateQuery<TF.AggregateProduct>("Products")
                .Execute();

            Assert.IsNotNull(data);
            Assert.IsNotNull(alldata);
        }
    }
}
