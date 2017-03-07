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

        /// <summary>
        /// Demo for Task 2 - Issue 2:
        /// Selecting order details by category.
        /// </summary>
        [TestMethod]
        public void GetOrdersByCategory()
        {
            var model = new Model.Model();
            var beveragesCategory = model.Categories.First(c => c.CategoryName == "Beverages");
            var orderDetails = model.OrderDetails;
            // Не понял почему у меня тут возникало исключение без перевода в List.
            // Можешь мне это объяснить?
            var orderInfos = orderDetails
                .ToList()
                .Where(d => d.Product.Category == beveragesCategory)
                .Select(i => new
                {
                    i.OrderID,
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

        /// <summary>
        /// Demo for Task 2 - Issue 3.
        /// Select Employees with regions.
        /// </summary>
        [TestMethod]
        public void GetEmployeesWithRegions()
        {
            var model = new Model.Model();
            var employees = model.Employees;

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

        /// <summary>
        /// Demo for Task 2 - Issue 4.
        /// Get count of employees by region.
        /// </summary>
        [TestMethod]
        public void GetCountOfEmployeesByRegion()
        {
            var model = new Model.Model();
            var regions = model.Regions;

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

        /// <summary>
        /// Demo for Task 2- Issue 5.
        /// </summary>
        [TestMethod]
        public void GetSuppliersStatsOfEmployees()
        {
            var model = new Model.Model();
            var orders = model.Orders;
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
}
