using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BusinessCategoryTreeServiceTest
{
    [TestClass]
    public class CategoryTreeServiceTest
    {
        [TestMethod]
        public void InitTest()
        {
            var service = new TF.Business.CategoryTreeService();

            Assert.IsNotNull(service);
        }
    }
}
