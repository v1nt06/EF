using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace EF.Tests
{
    [TestClass]
    public class EFTests
    {
        /// <summary>
        /// Demo for Task 2 - Issue 1:
        /// Products list with category and supplier.
        /// </summary>
        [TestMethod]
        public void GetProductListWithCategoryAndSupplier()
        {
            var model = new Model.Model();
            var products = model.Products;
            var categories = model.Categories;
            var suppliers = model.Suppliers;

            var productInfo = products.Join(categories,
                p => p.ProductID,
                c => c.CategoryID, (product, category) => new
                {
                    product.ProductName,
                    category.CategoryName,
                    product.SupplierID
                })
                .Join(suppliers,
                p => p.SupplierID,
                s => s.SupplierID, (p, s) => new
                {
                    p.ProductName,
                    p.CategoryName,
                    s.CompanyName
                });

            foreach (var p in productInfo)
            {
                Console.WriteLine($"{p.ProductName} - {p.CategoryName} - {p.CompanyName}");
            }
        }

        [TestMethod]
        public void SelectOrdersByCategory()
        {

            Assert.IsTrue(false);
        }
    }
}
