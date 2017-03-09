using System;
using System.Collections.Generic;
using System.Linq;
using EF.Model;
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
            using (var db = new Model.Model())
            {
                var products = db.Products;

                var productInfo = products.Select(p => new
                {
                    p.ProductName,
                    p.Category.CategoryName,
                    p.Supplier.CompanyName
                });

                foreach (var p in productInfo)
                {
                    Console.WriteLine($"{p.ProductName} - {p.CategoryName} - {p.CompanyName}");
                }
            }
        }

        /// <summary>
        /// Demo for Task 2 - Issue 2:
        /// Selecting order details by category.
        /// </summary>
        [TestMethod]
        public void GetOrdersByCategory()
        {
            using (var db = new Model.Model())
            {
                var beveragesCategory = db.Categories.First(c => c.CategoryName == "Beverages");
                var orderDetails = db.OrderDetails;
                // Не понял почему у меня тут возникало исключение без перевода в List.
                // Можешь мне это объяснить?
                var orderInfos = orderDetails
                    .ToList()
                    .Where(d => d.Product.Category == beveragesCategory)
                    .Select(i => new
                    {
                        i.OrderId,
                        i.Product.ProductName,
                        i.UnitPrice,
                        i.Quantity,
                        i.Discount,
                        i.Order.Customer.CompanyName
                    });

                foreach (var orderInfo in orderInfos)
                {
                    Console.WriteLine(orderInfo);
                }
            }
        }

        /// <summary>
        /// Demo for Task 2 - Issue 3.
        /// Select Employees with regions.
        /// </summary>
        [TestMethod]
        public void GetEmployeesWithRegions()
        {
            using (var db = new Model.Model())
            {
                var employees = db.Employees;

                var employeesWithRegions = employees.Select(e => new
                {
                    e.FirstName,
                    e.LastName,
                    Regions = e.Territories.Select(t => t.Region).Distinct()
                });

                foreach (var employee in employeesWithRegions)
                {
                    Console.WriteLine($"{employee.FirstName} {employee.LastName}");
                    foreach (var region in employee.Regions)
                    {
                        Console.WriteLine($"    {region.RegionDescription}");
                    }
                }
            }
        }

        /// <summary>
        /// Demo for Task 2 - Issue 4.
        /// Get count of employees by region.
        /// </summary>
        [TestMethod]
        public void GetCountOfEmployeesByRegion()
        {
            using (var db = new Model.Model())
            {
                var regions = db.Regions;

                var regionsWithCountOfEmployees = regions.Select(r => new
                {
                    RegionName = r.RegionDescription,
                    EmployeesCount = r.Territories.Select(t => t.Employees).Count()

                });

                foreach (var region in regionsWithCountOfEmployees)
                {
                    Console.WriteLine($"{region.RegionName} - {region.EmployeesCount}");
                }
            }
        }

        /// <summary>
        /// Demo for Task 2- Issue 5.
        /// </summary>
        [TestMethod]
        public void GetSuppliersStatsOfEmployees()
        {
            using (var db = new Model.Model())
            {
                var orders = db.Orders;
                // И здесь тоже не понял почему без ToList() не работает.
                var supplierStats = orders.ToList()
                    .GroupBy(o => o.Employee)
                    .Select(g => new
                    {
                        g.Key.FirstName,
                        g.Key.LastName,
                        Companies = g.Select(e => e.Shipper.CompanyName).Distinct()
                    });

                foreach (var element in supplierStats)
                {
                    Console.WriteLine($"{element.FirstName} {element.LastName}");
                    foreach (var company in element.Companies)
                    {
                        Console.WriteLine($"    {company}");
                    }
                }
            }
        }

        /// <summary>
        /// Demo for Task 3 - Issue 1.
        /// </summary>
        [TestMethod]
        public void AddNewEmployeeWithTerritories()
        {
            using (var db = new Model.Model())
            {
                var employee = db.Employees.Create();
                var territories = db.Territories;
                employee.LastName = "Ivanov";
                employee.FirstName = "Ivan";
                employee.Territories.Add(territories.First(t => t.Id == "01581"));
                employee.Territories.Add(territories.First(t => t.Id == "01730"));
                employee.Territories.Add(territories.First(t => t.Id == "01833"));
                db.Employees.Add(employee);
                db.SaveChanges();

                var newEmployee = db.Employees.First(e => e.Id == employee.Id);
                Assert.AreEqual(employee, newEmployee);
            }
        }

        /// <summary>
        /// Demo for Task 3 - Issue 2.
        /// </summary>
        [TestMethod]
        public void ChangeCategoryOfProduct()
        {
            using (var db = new Model.Model())
            {
                var chai = db.Products.First(p => p.Id == 1);
                var seafoodCategory = db.Categories.First(c => c.Id == 8);
                var beveragesCategory = db.Categories.First(c => c.Id == 1);

                try
                {
                    chai.Category = seafoodCategory;
                    db.SaveChanges();
                    Assert.AreEqual(seafoodCategory, db.Products.First(p => p.Id == 1).Category);
                }
                finally
                {
                    chai.Category = beveragesCategory;
                    db.SaveChanges();
                }
            }
        }

        /// <summary>
        /// Demo for Task 3 - Issue 3.
        /// </summary>
        [TestMethod]
        public void AddNewProducts()
        {
            using (var db = new Model.Model())
            {
                // Product without new supplier and category.
                var product1 = db.Products.Create();
                product1.ProductName = "Milk";
                product1.Supplier = db.Suppliers.First();
                product1.Category = db.Categories.First();

                // Product with new supplier and category.
                var product2 = db.Products.Create();
                product2.ProductName = "Chicken eggs";
                product2.Supplier = db.Suppliers.Create();
                product2.Supplier.CompanyName = "Test company";
                product2.Category = db.Categories.Create();
                product2.Category.CategoryName = "Animal origin";

                var products = new List<Product> { product1, product2 };

                db.Products.AddRange(products);
                db.SaveChanges();
            }
        }

        /// <summary>
        /// Demo for Task 3 - Issue 4.
        /// </summary>
        [TestMethod]
        public void ReplaceProductInNotShippedOrder()
        {
            // Не понял как выполнить это задание, похоже тут кривая структура БД для таких операций.
        }

        [ClassCleanup]
        public static void Cleanup()
        {
            using (var db = new Model.Model())
            {
                db.Products.RemoveRange(db.Products.Where(p => p.ProductName == "Milk"
                                                               || p.ProductName == "Chicken eggs"));
                db.Categories.RemoveRange(db.Categories.Where(c => c.CategoryName == "Animal origin"));
                db.Suppliers.RemoveRange(db.Suppliers.Where(s => s.CompanyName == "Test company"));
                db.SaveChanges();
            }
        }
    }
}
