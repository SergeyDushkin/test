using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace SystemSecurityUserServiceTest
{
    [TestClass]
    public class UserServiceTest
    {
        [TestMethod]
        public void InitTest()
        {
            var service = new TF.SystemSecurity.UserService();
            service.Init();

            Assert.IsNotNull(service);
        }

        [TestMethod]
        public void CreateAndReadAndDeleteTest()
        {
            var service = new TF.SystemSecurity.UserService();

            var record = new TF.SystemSecurity.USER()
            {
                GUID_RECORD = Guid.NewGuid(),
                KEY = "TestCategory002"

            };

            service.Create(record);

            var record2 = service.GetById(record.GUID_RECORD);
            
            Assert.AreEqual(record.GUID_RECORD, record2.GUID_RECORD);
            Assert.AreEqual(record.KEY, record2.KEY);

            record2.KEY = "TestCategory001";

            service.Update(record2);

            service.Delete(record.GUID_RECORD);
        }

    }
}
