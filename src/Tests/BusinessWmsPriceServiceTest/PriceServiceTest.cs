using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BusinessWmsPriceServiceTest
{
    [TestClass]
    public class PriceServiceTest
    {

        [TestMethod]
        public void InitTest()
        {
            var service = new TF.Business.WMS.PriceService();
            service.Init();

            Assert.IsNotNull(service);
        }

        [TestMethod]
        public void CreateAndReadAndDeleteTest()
        {
            var service = new TF.Business.WMS.PriceService();

            var record = new TF.Business.WMS.Price()
            {
                Id = Guid.NewGuid(),
                ProductId = Guid.NewGuid(),
                ProductPrice = 10
            };

            service.Create(record);

            var record2 = service.GetById(record.Id);

            Assert.AreEqual(record.Id, record2.Id);
            Assert.AreEqual(record.ProductId, record2.ProductId);
            Assert.AreEqual(record.ProductPrice, record2.ProductPrice);

            service.Delete(record.Id);
        }

        [TestMethod]
        public void CreateAndUpdateAndDeleteTest()
        {
            var service = new TF.Business.WMS.PriceService();

            var record = new TF.Business.WMS.Price()
            {
                Id = Guid.NewGuid(),
                ProductId = Guid.NewGuid(),
                ProductPrice = 10
            };

            service.Create(record);

            record.ProductPrice = 20;

            service.Update(record);

            var record2 = service.GetById(record.Id);

            Assert.AreEqual(record.Id, record2.Id);
            Assert.AreEqual(record.ProductId, record2.ProductId);
            Assert.AreEqual(record.ProductPrice, record2.ProductPrice);

            service.Delete(record.Id);
        }
    }
}
