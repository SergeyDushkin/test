using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTestProject
{
    [TestClass]
    public class OrderControllerUnitTest
    {
        [TestMethod]
        public void TestGetMethod()
        {
            var controller = new TF.OrderController();
            var value = controller.Get();
        }
    }
}
