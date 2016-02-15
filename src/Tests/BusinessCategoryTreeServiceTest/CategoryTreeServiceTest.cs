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
            service.Init();

            Assert.IsNotNull(service);
        }

        [TestMethod]
        public void CreateAndReadAndDeleteTest()
        {
            var service = new TF.Business.CategoryTreeService();

            var record = new TF.Business.CategoryTree()
            {
                Id = Guid.NewGuid(),
                Key = "TestCategory001",
                Name = "TestCategory001",
            };

            service.Create(record);

            var record2 = service.GetById(record.Id);

            Assert.AreEqual(record.Id, record2.Id);
            Assert.AreEqual(record.Key, record2.Key);
            Assert.AreEqual(record.Name, record2.Name);
            Assert.AreEqual(record.ParentId, record2.ParentId);

            service.Delete(record.Id);
        }

        [TestMethod]
        public void CreateAndUpdateAndDeleteTest()
        {
            var service = new TF.Business.CategoryTreeService();

            var record = new TF.Business.CategoryTree()
            {
                Id = Guid.NewGuid(),
                Key = "TestCategory001",
                Name = "TestCategory001",
            };

            service.Create(record);

            record.Key = "TestCategory002";
            record.Name = "TestCategory002";

            service.Update(record);
            
            var record2 = service.GetById(record.Id);

            Assert.AreEqual(record.Id, record2.Id);
            Assert.AreEqual(record.Key, record2.Key);
            Assert.AreEqual(record.Name, record2.Name);
            Assert.AreEqual(record.ParentId, record2.ParentId);

            service.Delete(record.Id);
        }
    }
}
