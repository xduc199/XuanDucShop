using Microsoft.AspNetCore.Mvc;
using XuanDucShop.Models;

namespace XuanDucShop.Controllers;

public class HomeController : Controller
{
    public IActionResult Index() => View();

    public IActionResult Category()
    {
        var categories = new List<Category>
        {
            new() { Id = 1, Name = "Do uong", Description = "Nuoc uong" },
            new() { Id = 2, Name = "Thuc pham", Description = "Thuc pham dong goi" }
        };
        return View(categories);
    }

    public IActionResult Product()
    {
        var products = new List<Product>
        {
            new() { Id = 1, Name = "Ca phe", CategoryId = 1, Price = 50000, StockQuantity = 20, ImageFileName = "coffee.jpg", IsActive = true },
            new() { Id = 2, Name = "Banh quy", CategoryId = 2, Price = 30000, StockQuantity = 15, ImageFileName = "cookie.jpg", IsActive = true }
        };
        return View(products);
    }

    public IActionResult Employee()
    {
        var employees = new List<Employee>
        {
            new() { Id = 1, FullName = "Nguyen Van An", Phone = "0900000001", Email = "an@example.com", UserId = "user1", IsActive = true }
        };
        return View(employees);
    }

    public IActionResult Invoice()
    {
        var invoices = new List<Invoice>
        {
            new() { Id = 1, CreatedAt = DateTime.Today, EmployeeId = 1, CustomerName = "Tran Thi B", CustomerPhone = "0900000002", TotalAmount = 100000 }
        };
        return View(invoices);
    }

    public IActionResult InvoiceDetail()
    {
        var details = new List<InvoiceDetail>
        {
            new() { Id = 1, InvoiceId = 1, ProductId = 1, Quantity = 2, UnitPrice = 50000 }
        };
        return View(details);
    }
}
