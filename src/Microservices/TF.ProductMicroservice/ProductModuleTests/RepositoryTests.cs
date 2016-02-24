using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using TF.ProductMicroservice;

namespace ProductModuleTests
{
    [TestClass]
    public class RepositoryTests
    {
        private static IProductRepository repository
        {
            get
            {
                return new ProductRepository();
            }
        }

        private const int SIMPLE_PRODUCTS_COUNT = 10;
        private const int COMPLEX_PRODUCT_CHILDS = 10;

        [TestMethod]
        public void SimpleCreateTest()
        {
            foreach (var product in GetSimpleProductsCollection(SIMPLE_PRODUCTS_COUNT))
            {
                var saveResult = repository.Save(product);
                TestProductRecursive(product, saveResult);
                TestProductFromDB(product, saveResult.Id);
            }
        }

        [TestMethod]
        public void CreateComplexObjectTest()
        {
            var complexProduct = GetComplexProduct(COMPLEX_PRODUCT_CHILDS);
            var saveResult = repository.Save(complexProduct);
            TestProductRecursive(complexProduct, saveResult);
            TestProductFromDB(complexProduct, saveResult.Id);
        }

        [TestMethod]
        public void UpdateTest()
        {
            var product = GetSimpleProduct();
            var saveResult = repository.Save(product);

            saveResult.Name = string.Format("UpdatedTestProduct_{0}", Guid.NewGuid());
            saveResult.Type = string.Format("UpdatedType_{0}", Guid.NewGuid());
            
            var updateResult = repository.Save(saveResult);
            TestProductRecursive(saveResult, updateResult);
            TestProductFromDB(saveResult, saveResult.Id);
        }

        [TestMethod]
        public void DeleteTest()
        {
            var product = GetComplexProduct(1);
            var saveResult = repository.Save(product);

            repository.Delete(saveResult.Id);
            var getResult = repository.Get(saveResult.Id);
            Assert.IsNull(getResult);
        }


        [TestMethod]
        public void CascadeDeleteTest()
        {
            var product = GetComplexProduct(COMPLEX_PRODUCT_CHILDS);
            var saveResult = repository.Save(product);
            Product getResult = null;

            foreach (var child in saveResult.ChildProducts)
            {
                repository.Delete(child.Id);
                getResult = repository.Get(child.Id);
                Assert.IsNull(getResult);
            }

            repository.Delete(saveResult.Id);
            getResult = repository.Get(saveResult.Id);
            Assert.IsNull(getResult);
        }

        private static void TestProduct(Product expected, Product actual)
        {
            Assert.AreEqual(expected.Name, actual.Name);
            Assert.AreEqual(expected.Type, actual.Type);
            if (expected.Parent != null)
            {
                Assert.AreEqual(expected.Parent.Id, actual.Parent.Id);
            }
            Assert.AreEqual(expected.ChildProducts.Count, actual.ChildProducts.Count);
            Assert.IsNotNull(actual.Id);
        }

        public static void TestProductRecursive(Product expected, Product actual)
        {
            TestProduct(expected, actual);
            if (actual.ChildProducts.Count != 0)
            {
                var expectedChildProducts = (List<Product>)expected.ChildProducts;
                var actualChildProducts = (List<Product>)actual.ChildProducts;
                for (int childIdx = 0; childIdx < expectedChildProducts.Count; childIdx++)
                {
                    TestProductRecursive(expectedChildProducts[childIdx], actualChildProducts[childIdx]);
                }
            }
        }

        private static List<Product> SortByName(IEnumerable<Product> Products)
        {
            return Products.OrderBy(p => p.Name).ToList();
        }

        public static Product GetSimpleProduct()
        {
            return new Product() {
                Name = string.Format("SimpleTestProduct_{0}", Guid.NewGuid()),
                Type =string.Format("TestType_{0}", Guid.NewGuid())
            };
        }

        private static List<Product> GetSimpleProductsCollection(int count)
        {
            var products = new List<Product>();
            for (var productIdx = 0; productIdx < count; productIdx++)
            {
                products.Add(GetSimpleProduct());
            }
            return products;
        }

        public static Product GetComplexProduct(int childCount)
        {
            var product = new Product() {
                Name = string.Format("ComplexTestProduct_{0}", Guid.NewGuid()),
                Type = string.Format("ComplexType_{0}", Guid.NewGuid())
            };

            product.ChildProducts = GetSimpleProductsCollection(childCount);
            return product;
        }

        private void TestProductFromDB(Product product, Guid Id)
        {
            var getResult = repository.Get(Id);
            TestProductRecursive(product, getResult);
        }
    }
}