using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BusinessPersonServiceTest
{
    [TestClass]
    public class PersonServiceTest
    {
        [TestMethod]
        public void InitTest()
        {
            var service = new TF.Business.PersonService();
            service.Init();

            Assert.IsNotNull(service);
        }

        [TestMethod]
        public void CreateAndReadAndDeleteTestPerson()
        {
            var service = new TF.Business.PersonService();

            var record = new TF.Business.PERSON()
            {
                GUID_RECORD = Guid.NewGuid(),
                FIRSTNAME = "Alexey",
                LASTNAME = "Serov",
                BIRTHDATE = DateTime.Now

            };

            service.Create(record);

            var record2 = service.GetById(record.GUID_RECORD);

            Assert.AreEqual(record.GUID_RECORD, record2.GUID_RECORD);
            Assert.AreEqual(record.FIRSTNAME, record2.FIRSTNAME);
            Assert.AreEqual(record.LASTNAME, record2.LASTNAME);

            record2.LASTNAME = "Zudov";
            service.Update(record2);

            service.Delete(record.GUID_RECORD);
        }
    }
}
