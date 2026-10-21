using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using XuanDucShop.Models;
using XuanDucShop.ViewModels;

namespace XuanDucShop.Controllers;

public class HomeController : Controller
{
    private static readonly List<Category> Categories =
    [
        new() { Id = 1, Name = "Cà phê", Description = "Cà phê rang xay và đồ uống từ cà phê" },
        new() { Id = 2, Name = "Trà và đồ uống", Description = "Trà trái cây, trà sữa và đồ uống giải khát" },
        new() { Id = 3, Name = "Bánh ngọt", Description = "Bánh ngọt dùng kèm đồ uống" }
    ];

    private static readonly List<Product> Products =
    [
        new() { Id = 1, Name = "Cà phê sữa đá", CategoryId = 1, Price = 28000, StockQuantity = 42, ImageFileName = "ca-phe-sua-da.jpg", IsActive = true },
        new() { Id = 2, Name = "Trà đào cam sả", CategoryId = 2, Price = 32000, StockQuantity = 28, ImageFileName = "tra-dao-cam-sa.jpg", IsActive = true },
        new() { Id = 3, Name = "Bánh croissant", CategoryId = 3, Price = 25000, StockQuantity = 16, ImageFileName = "croissant.jpg", IsActive = true },
        new() { Id = 4, Name = "Trà sữa truyền thống", CategoryId = 2, Price = 35000, StockQuantity = 20, ImageFileName = "tra-sua.jpg", IsActive = false }
    ];

    private static readonly List<Employee> Employees =
    [
        new() { Id = 1, FullName = "Nguyễn Minh Anh", Phone = "0905123456", Email = "minhanh@xuanducshop.vn", UserId = "NV001", IsActive = true },
        new() { Id = 2, FullName = "Trần Quốc Bảo", Phone = "0912345678", Email = "quocbao@xuanducshop.vn", UserId = "NV002", IsActive = true },
        new() { Id = 3, FullName = "Lê Thu Hà", Phone = "0987654321", Email = "thuha@xuanducshop.vn", UserId = "NV003", IsActive = false }
    ];

    private static readonly List<Invoice> Invoices =
    [
        new() { Id = 1, CreatedAt = new DateTime(2026, 10, 18, 9, 15, 0), EmployeeId = 1, CustomerName = "Phạm Ngọc Lan", CustomerPhone = "0903000001", TotalAmount = 56000 },
        new() { Id = 2, CreatedAt = new DateTime(2026, 10, 19, 14, 30, 0), EmployeeId = 2, CustomerName = "Võ Thành Đạt", CustomerPhone = "0903000002", TotalAmount = 57000 },
        new() { Id = 3, CreatedAt = new DateTime(2026, 10, 20, 16, 45, 0), EmployeeId = 1, CustomerName = "Đặng Thảo My", CustomerPhone = "0903000003", TotalAmount = 70000 }
    ];

    private static readonly List<InvoiceDetail> InvoiceDetails =
    [
        new() { Id = 1, InvoiceId = 1, ProductId = 1, Quantity = 2, UnitPrice = 28000 },
        new() { Id = 2, InvoiceId = 2, ProductId = 2, Quantity = 1, UnitPrice = 32000 },
        new() { Id = 3, InvoiceId = 2, ProductId = 3, Quantity = 1, UnitPrice = 25000 },
        new() { Id = 4, InvoiceId = 3, ProductId = 4, Quantity = 2, UnitPrice = 35000 }
    ];

    public IActionResult Index()
    {
        var model = new HomeViewModel
        {
            CategoryCount = Categories.Count,
            ProductCount = Products.Count,
            EmployeeCount = Employees.Count,
            InvoiceCount = Invoices.Count,
            Revenue = Invoices.Sum(x => x.TotalAmount)
        };

        return View(model);
    }

    public IActionResult Category() => View(Categories);

    public IActionResult Product()
    {
        ViewBag.Categories = Categories;
        return View(Products);
    }

    public IActionResult Employee() => View(Employees);

    public IActionResult Invoice()
    {
        ViewBag.Employees = Employees;
        return View(Invoices.OrderByDescending(x => x.CreatedAt).ToList());
    }

    public IActionResult InvoiceDetail()
    {
        ViewBag.Invoices = Invoices;
        ViewBag.Products = Products;
        return View(InvoiceDetails);
    }

    [HttpGet]
    public IActionResult CreateCategory() => View("CategoryForm", new Category());

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult CreateCategory(Category model)
    {
        if (!ModelState.IsValid)
            return View("CategoryForm", model);

        model.Id = Categories.Count == 0 ? 1 : Categories.Max(x => x.Id) + 1;
        Categories.Add(model);
        TempData["Success"] = "Đã thêm danh mục mới.";

        return RedirectToAction(nameof(Category));
    }

    [HttpGet]
    public IActionResult EditCategory(int id)
    {
        var category = Categories.FirstOrDefault(x => x.Id == id);
        return category == null ? NotFound() : View("CategoryForm", category);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult EditCategory(int id, Category model)
    {
        var category = Categories.FirstOrDefault(x => x.Id == id);
        if (category == null)
            return NotFound();

        if (!ModelState.IsValid)
            return View("CategoryForm", model);

        category.Name = model.Name;
        category.Description = model.Description;
        TempData["Success"] = "Đã cập nhật danh mục.";

        return RedirectToAction(nameof(Category));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult DeleteCategory(int id)
    {
        if (Products.Any(x => x.CategoryId == id))
        {
            TempData["Error"] = "Không thể xóa danh mục đang có sản phẩm.";
            return RedirectToAction(nameof(Category));
        }

        var category = Categories.FirstOrDefault(x => x.Id == id);
        if (category != null)
            Categories.Remove(category);

        TempData["Success"] = "Đã xóa danh mục.";
        return RedirectToAction(nameof(Category));
    }

    [HttpGet]
    public IActionResult CreateProduct() => View("ProductForm", BuildProductForm(new Product()));

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult CreateProduct(ProductFormViewModel model)
    {
        if (!Categories.Any(x => x.Id == model.Product.CategoryId))
            ModelState.AddModelError("Product.CategoryId", "Danh mục đã chọn không tồn tại.");

        if (!ModelState.IsValid)
            return View("ProductForm", BuildProductForm(model.Product));

        model.Product.Id = Products.Count == 0 ? 1 : Products.Max(x => x.Id) + 1;
        Products.Add(model.Product);
        TempData["Success"] = "Đã thêm sản phẩm mới.";

        return RedirectToAction(nameof(Product));
    }

    [HttpGet]
    public IActionResult EditProduct(int id)
    {
        var product = Products.FirstOrDefault(x => x.Id == id);
        return product == null ? NotFound() : View("ProductForm", BuildProductForm(product));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult EditProduct(int id, ProductFormViewModel model)
    {
        var product = Products.FirstOrDefault(x => x.Id == id);
        if (product == null)
            return NotFound();

        if (!Categories.Any(x => x.Id == model.Product.CategoryId))
            ModelState.AddModelError("Product.CategoryId", "Danh mục đã chọn không tồn tại.");

        if (!ModelState.IsValid)
            return View("ProductForm", BuildProductForm(model.Product));

        product.Name = model.Product.Name;
        product.CategoryId = model.Product.CategoryId;
        product.Price = model.Product.Price;
        product.StockQuantity = model.Product.StockQuantity;
        product.ImageFileName = model.Product.ImageFileName;
        product.IsActive = model.Product.IsActive;
        TempData["Success"] = "Đã cập nhật sản phẩm.";

        return RedirectToAction(nameof(Product));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult DeleteProduct(int id)
    {
        if (InvoiceDetails.Any(x => x.ProductId == id))
        {
            TempData["Error"] = "Không thể xóa sản phẩm đã xuất hiện trong hóa đơn.";
            return RedirectToAction(nameof(Product));
        }

        var product = Products.FirstOrDefault(x => x.Id == id);
        if (product != null)
            Products.Remove(product);

        TempData["Success"] = "Đã xóa sản phẩm.";
        return RedirectToAction(nameof(Product));
    }

    [HttpGet]
    public IActionResult CreateEmployee() => View("EmployeeForm", new Employee());

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult CreateEmployee(Employee model)
    {
        if (!ModelState.IsValid)
            return View("EmployeeForm", model);

        model.Id = Employees.Count == 0 ? 1 : Employees.Max(x => x.Id) + 1;
        Employees.Add(model);
        TempData["Success"] = "Đã thêm nhân viên mới.";

        return RedirectToAction(nameof(Employee));
    }

    [HttpGet]
    public IActionResult EditEmployee(int id)
    {
        var employee = Employees.FirstOrDefault(x => x.Id == id);
        return employee == null ? NotFound() : View("EmployeeForm", employee);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult EditEmployee(int id, Employee model)
    {
        var employee = Employees.FirstOrDefault(x => x.Id == id);
        if (employee == null)
            return NotFound();

        if (!ModelState.IsValid)
            return View("EmployeeForm", model);

        employee.FullName = model.FullName;
        employee.Phone = model.Phone;
        employee.Email = model.Email;
        employee.UserId = model.UserId;
        employee.IsActive = model.IsActive;
        TempData["Success"] = "Đã cập nhật nhân viên.";

        return RedirectToAction(nameof(Employee));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult DeleteEmployee(int id)
    {
        if (Invoices.Any(x => x.EmployeeId == id))
        {
            TempData["Error"] = "Không thể xóa nhân viên đã lập hóa đơn.";
            return RedirectToAction(nameof(Employee));
        }

        var employee = Employees.FirstOrDefault(x => x.Id == id);
        if (employee != null)
            Employees.Remove(employee);

        TempData["Success"] = "Đã xóa nhân viên.";
        return RedirectToAction(nameof(Employee));
    }

    [HttpGet]
    public IActionResult CreateInvoice() => View("InvoiceForm", BuildInvoiceForm(new Invoice { CreatedAt = DateTime.Now }));

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult CreateInvoice(InvoiceFormViewModel model)
    {
        if (!Employees.Any(x => x.Id == model.Invoice.EmployeeId))
            ModelState.AddModelError("Invoice.EmployeeId", "Nhân viên đã chọn không tồn tại.");

        if (!ModelState.IsValid)
            return View("InvoiceForm", BuildInvoiceForm(model.Invoice));

        model.Invoice.Id = Invoices.Count == 0 ? 1 : Invoices.Max(x => x.Id) + 1;
        model.Invoice.CreatedAt = model.Invoice.CreatedAt == default ? DateTime.Now : model.Invoice.CreatedAt;
        model.Invoice.TotalAmount = 0;
        Invoices.Add(model.Invoice);
        TempData["Success"] = "Đã tạo hóa đơn mới.";

        return RedirectToAction(nameof(Invoice));
    }

    [HttpGet]
    public IActionResult EditInvoice(int id)
    {
        var invoice = Invoices.FirstOrDefault(x => x.Id == id);
        return invoice == null ? NotFound() : View("InvoiceForm", BuildInvoiceForm(invoice));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult EditInvoice(int id, InvoiceFormViewModel model)
    {
        var invoice = Invoices.FirstOrDefault(x => x.Id == id);
        if (invoice == null)
            return NotFound();

        if (!Employees.Any(x => x.Id == model.Invoice.EmployeeId))
            ModelState.AddModelError("Invoice.EmployeeId", "Nhân viên đã chọn không tồn tại.");

        if (!ModelState.IsValid)
            return View("InvoiceForm", BuildInvoiceForm(model.Invoice));

        invoice.CreatedAt = model.Invoice.CreatedAt;
        invoice.EmployeeId = model.Invoice.EmployeeId;
        invoice.CustomerName = model.Invoice.CustomerName;
        invoice.CustomerPhone = model.Invoice.CustomerPhone;
        TempData["Success"] = "Đã cập nhật hóa đơn.";

        return RedirectToAction(nameof(Invoice));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult DeleteInvoice(int id)
    {
        var invoice = Invoices.FirstOrDefault(x => x.Id == id);
        if (invoice != null)
        {
            InvoiceDetails.RemoveAll(x => x.InvoiceId == id);
            Invoices.Remove(invoice);
        }

        TempData["Success"] = "Đã xóa hóa đơn và các dòng chi tiết liên quan.";
        return RedirectToAction(nameof(Invoice));
    }

    [HttpGet]
    public IActionResult CreateInvoiceDetail() => View("InvoiceDetailForm", BuildInvoiceDetailForm(new InvoiceDetail { Quantity = 1, UnitPrice = 1 }));

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult CreateInvoiceDetail(InvoiceDetailFormViewModel model)
    {
        ValidateInvoiceDetail(model.InvoiceDetail);

        if (!ModelState.IsValid)
            return View("InvoiceDetailForm", BuildInvoiceDetailForm(model.InvoiceDetail));

        model.InvoiceDetail.Id = InvoiceDetails.Count == 0 ? 1 : InvoiceDetails.Max(x => x.Id) + 1;
        InvoiceDetails.Add(model.InvoiceDetail);
        RecalculateInvoiceTotal(model.InvoiceDetail.InvoiceId);
        TempData["Success"] = "Đã thêm chi tiết hóa đơn.";

        return RedirectToAction(nameof(InvoiceDetail));
    }

    [HttpGet]
    public IActionResult EditInvoiceDetail(int id)
    {
        var detail = InvoiceDetails.FirstOrDefault(x => x.Id == id);
        return detail == null ? NotFound() : View("InvoiceDetailForm", BuildInvoiceDetailForm(detail));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult EditInvoiceDetail(int id, InvoiceDetailFormViewModel model)
    {
        var detail = InvoiceDetails.FirstOrDefault(x => x.Id == id);
        if (detail == null)
            return NotFound();

        var oldInvoiceId = detail.InvoiceId;
        ValidateInvoiceDetail(model.InvoiceDetail);

        if (!ModelState.IsValid)
            return View("InvoiceDetailForm", BuildInvoiceDetailForm(model.InvoiceDetail));

        detail.InvoiceId = model.InvoiceDetail.InvoiceId;
        detail.ProductId = model.InvoiceDetail.ProductId;
        detail.Quantity = model.InvoiceDetail.Quantity;
        detail.UnitPrice = model.InvoiceDetail.UnitPrice;
        RecalculateInvoiceTotal(oldInvoiceId);
        RecalculateInvoiceTotal(detail.InvoiceId);
        TempData["Success"] = "Đã cập nhật chi tiết hóa đơn.";

        return RedirectToAction(nameof(InvoiceDetail));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult DeleteInvoiceDetail(int id)
    {
        var detail = InvoiceDetails.FirstOrDefault(x => x.Id == id);
        if (detail != null)
        {
            var invoiceId = detail.InvoiceId;
            InvoiceDetails.Remove(detail);
            RecalculateInvoiceTotal(invoiceId);
        }

        TempData["Success"] = "Đã xóa chi tiết hóa đơn.";
        return RedirectToAction(nameof(InvoiceDetail));
    }

    private static ProductFormViewModel BuildProductForm(Product product) => new()
    {
        Product = product,
        Categories = Categories.Select(x => new SelectListItem(x.Name, x.Id.ToString())).ToList()
    };

    private static InvoiceFormViewModel BuildInvoiceForm(Invoice invoice) => new()
    {
        Invoice = invoice,
        Employees = Employees.Select(x => new SelectListItem($"{x.FullName} ({x.UserId})", x.Id.ToString())).ToList()
    };

    private static InvoiceDetailFormViewModel BuildInvoiceDetailForm(InvoiceDetail detail) => new()
    {
        InvoiceDetail = detail,
        Invoices = Invoices.Select(x => new SelectListItem($"HD{x.Id:000} - {x.CustomerName}", x.Id.ToString())).ToList(),
        Products = Products.Select(x => new SelectListItem(x.Name, x.Id.ToString())).ToList()
    };

    private void ValidateInvoiceDetail(InvoiceDetail detail)
    {
        if (!Invoices.Any(x => x.Id == detail.InvoiceId))
            ModelState.AddModelError("InvoiceDetail.InvoiceId", "Hóa đơn đã chọn không tồn tại.");

        if (!Products.Any(x => x.Id == detail.ProductId))
            ModelState.AddModelError("InvoiceDetail.ProductId", "Sản phẩm đã chọn không tồn tại.");
    }

    private static void RecalculateInvoiceTotal(int invoiceId)
    {
        var invoice = Invoices.FirstOrDefault(x => x.Id == invoiceId);
        if (invoice != null)
            invoice.TotalAmount = InvoiceDetails.Where(x => x.InvoiceId == invoiceId).Sum(x => x.Quantity * x.UnitPrice);
    }
}
