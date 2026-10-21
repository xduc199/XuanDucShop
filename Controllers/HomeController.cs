using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using XuanDucShop.Models;
using XuanDucShop.Models.ViewModels;
using XuanDucShop.Services;

namespace XuanDucShop.Controllers;

public class HomeController(StoreData storeData) : Controller
{
    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error() => View("Error", new ErrorViewModel
    {
        RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier
    });

    [HttpGet]
    public IActionResult Privacy() => View();

    [HttpGet]
    public IActionResult Index()
    {
        var invoices = storeData.Invoices;
        var employees = storeData.Employees;
        return View(new DashboardViewModel
        {
            CategoryCount = storeData.Categories.Count,
            ProductCount = storeData.Products.Count,
            EmployeeCount = employees.Count,
            InvoiceCount = invoices.Count,
            Revenue = invoices.Sum(invoice => invoice.TotalAmount),
            RecentInvoices = ToInvoiceRows(invoices.OrderByDescending(invoice => invoice.CreatedAt).Take(5), employees)
        });
    }

    [HttpGet]
    public IActionResult Category() => View(storeData.Categories);

    [HttpGet]
    public IActionResult Product()
    {
        var categories = storeData.Categories.ToDictionary(category => category.Id, category => category.Name);
        var products = storeData.Products.Select(product => new ProductRowViewModel
        {
            Id = product.Id,
            Name = product.Name,
            CategoryName = categories.GetValueOrDefault(product.CategoryId, "Không xác định"),
            Price = product.Price,
            StockQuantity = product.StockQuantity,
            ImageFileName = product.ImageFileName,
            IsActive = product.IsActive
        }).ToList();
        return View(products);
    }

    [HttpGet]
    public IActionResult Employee() => View(storeData.Employees);

    [HttpGet]
    public IActionResult Invoice()
    {
        var employees = storeData.Employees;
        var invoices = storeData.Invoices.OrderByDescending(invoice => invoice.CreatedAt);
        return View(ToInvoiceRows(invoices, employees));
    }

    [HttpGet]
    public IActionResult InvoiceDetail(int? invoiceId)
    {
        var products = storeData.Products.ToDictionary(product => product.Id, product => product.Name);
        var details = storeData.InvoiceDetails
            .Where(detail => !invoiceId.HasValue || detail.InvoiceId == invoiceId.Value)
            .Select(detail => new InvoiceDetailRowViewModel
            {
                Id = detail.Id,
                InvoiceId = detail.InvoiceId,
                ProductName = products.GetValueOrDefault(detail.ProductId, "Không xác định"),
                Quantity = detail.Quantity,
                UnitPrice = detail.UnitPrice
            })
            .ToList();
        return View(details);
    }

    [HttpGet]
    public IActionResult CreateCategory() => CategoryForm(new Category());

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult CreateCategory(Category category)
    {
        if (string.IsNullOrWhiteSpace(category.Name)) ModelState.AddModelError(nameof(category.Name), "Vui lòng nhập tên danh mục.");
        if (!ModelState.IsValid) return CategoryForm(category);
        if (!storeData.SaveCategory(null, category, out var error)) return InvalidForm("CategoryForm", category, error);
        TempData["SuccessMessage"] = "Đã thêm danh mục.";
        return RedirectToAction(nameof(Category));
    }

    [HttpGet]
    public IActionResult EditCategory(int id)
    {
        var category = storeData.FindCategory(id);
        return category is null ? NotFound() : CategoryForm(category);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult EditCategory(int id, Category category)
    {
        if (storeData.FindCategory(id) is null) return NotFound();
        category.Id = id;
        if (string.IsNullOrWhiteSpace(category.Name)) ModelState.AddModelError(nameof(category.Name), "Vui lòng nhập tên danh mục.");
        if (!ModelState.IsValid) return CategoryForm(category);
        if (!storeData.SaveCategory(id, category, out var error)) return InvalidForm("CategoryForm", category, error);
        TempData["SuccessMessage"] = "Đã cập nhật danh mục.";
        return RedirectToAction(nameof(Category));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult DeleteCategory(int id) => DeleteAndReturn(storeData.DeleteCategory(id, out var error), error, "Đã xóa danh mục.", nameof(Category));

    [HttpGet]
    public IActionResult CreateProduct() => ProductForm(new ProductFormViewModel());

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult CreateProduct(ProductFormViewModel model)
    {
        if (string.IsNullOrWhiteSpace(model.Name)) ModelState.AddModelError(nameof(model.Name), "Vui lòng nhập tên sản phẩm.");
        if (!ModelState.IsValid) return ProductForm(model);
        if (!storeData.SaveProduct(null, ToProduct(model), out var error))
        {
            ModelState.AddModelError(nameof(model.CategoryId), error);
            return ProductForm(model);
        }
        TempData["SuccessMessage"] = "Đã thêm sản phẩm.";
        return RedirectToAction(nameof(Product));
    }

    [HttpGet]
    public IActionResult EditProduct(int id)
    {
        var product = storeData.FindProduct(id);
        return product is null ? NotFound() : ProductForm(ToProductForm(product));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult EditProduct(int id, ProductFormViewModel model)
    {
        if (storeData.FindProduct(id) is null) return NotFound();
        model.Id = id;
        if (string.IsNullOrWhiteSpace(model.Name)) ModelState.AddModelError(nameof(model.Name), "Vui lòng nhập tên sản phẩm.");
        if (!ModelState.IsValid) return ProductForm(model);
        if (!storeData.SaveProduct(id, ToProduct(model), out var error))
        {
            ModelState.AddModelError(nameof(model.CategoryId), error);
            return ProductForm(model);
        }
        TempData["SuccessMessage"] = "Đã cập nhật sản phẩm.";
        return RedirectToAction(nameof(Product));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult DeleteProduct(int id) => DeleteAndReturn(storeData.DeleteProduct(id, out var error), error, "Đã xóa sản phẩm.", nameof(Product));

    [HttpGet]
    public IActionResult CreateEmployee() => EmployeeForm(new EmployeeFormViewModel());

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult CreateEmployee(EmployeeFormViewModel model)
    {
        if (string.IsNullOrWhiteSpace(model.FullName)) ModelState.AddModelError(nameof(model.FullName), "Vui lòng nhập họ tên nhân viên.");
        if (!ModelState.IsValid) return EmployeeForm(model);
        if (!storeData.SaveEmployee(null, ToEmployee(model), out var error)) return InvalidForm("EmployeeForm", model, error);
        TempData["SuccessMessage"] = "Đã thêm nhân viên.";
        return RedirectToAction(nameof(Employee));
    }

    [HttpGet]
    public IActionResult EditEmployee(int id)
    {
        var employee = storeData.FindEmployee(id);
        return employee is null ? NotFound() : EmployeeForm(ToEmployeeForm(employee));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult EditEmployee(int id, EmployeeFormViewModel model)
    {
        if (storeData.FindEmployee(id) is null) return NotFound();
        model.Id = id;
        if (string.IsNullOrWhiteSpace(model.FullName)) ModelState.AddModelError(nameof(model.FullName), "Vui lòng nhập họ tên nhân viên.");
        if (!ModelState.IsValid) return EmployeeForm(model);
        if (!storeData.SaveEmployee(id, ToEmployee(model), out var error)) return InvalidForm("EmployeeForm", model, error);
        TempData["SuccessMessage"] = "Đã cập nhật nhân viên.";
        return RedirectToAction(nameof(Employee));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult DeleteEmployee(int id) => DeleteAndReturn(storeData.DeleteEmployee(id, out var error), error, "Đã xóa nhân viên.", nameof(Employee));

    [HttpGet]
    public IActionResult CreateInvoice() => InvoiceForm(new InvoiceFormViewModel());

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult CreateInvoice(InvoiceFormViewModel model)
    {
        if (string.IsNullOrWhiteSpace(model.CustomerName)) ModelState.AddModelError(nameof(model.CustomerName), "Vui lòng nhập tên khách hàng.");
        if (!ModelState.IsValid) return InvoiceForm(model);
        if (!storeData.SaveInvoice(null, ToInvoice(model), out var error))
        {
            ModelState.AddModelError(nameof(model.EmployeeId), error);
            return InvoiceForm(model);
        }
        TempData["SuccessMessage"] = "Đã tạo hóa đơn. Hãy thêm sản phẩm vào hóa đơn để cập nhật tổng tiền.";
        return RedirectToAction(nameof(Invoice));
    }

    [HttpGet]
    public IActionResult EditInvoice(int id)
    {
        var invoice = storeData.FindInvoice(id);
        return invoice is null ? NotFound() : InvoiceForm(ToInvoiceForm(invoice));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult EditInvoice(int id, InvoiceFormViewModel model)
    {
        if (storeData.FindInvoice(id) is null) return NotFound();
        model.Id = id;
        if (string.IsNullOrWhiteSpace(model.CustomerName)) ModelState.AddModelError(nameof(model.CustomerName), "Vui lòng nhập tên khách hàng.");
        if (!ModelState.IsValid) return InvoiceForm(model);
        if (!storeData.SaveInvoice(id, ToInvoice(model), out var error))
        {
            ModelState.AddModelError(nameof(model.EmployeeId), error);
            return InvoiceForm(model);
        }
        TempData["SuccessMessage"] = "Đã cập nhật hóa đơn.";
        return RedirectToAction(nameof(Invoice));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult DeleteInvoice(int id) => DeleteAndReturn(storeData.DeleteInvoice(id, out var error), error, "Đã xóa hóa đơn.", nameof(Invoice));

    [HttpGet]
    public IActionResult CreateInvoiceDetail(int? invoiceId)
    {
        var model = new InvoiceDetailFormViewModel { InvoiceId = invoiceId ?? 0 };
        return InvoiceDetailForm(model);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult CreateInvoiceDetail(InvoiceDetailFormViewModel model)
    {
        if (!ModelState.IsValid) return InvoiceDetailForm(model);
        if (!storeData.SaveInvoiceDetail(null, ToInvoiceDetail(model), out var error))
        {
            ModelState.AddModelError(error.Contains("hóa đơn", StringComparison.OrdinalIgnoreCase) ? nameof(model.InvoiceId) : nameof(model.ProductId), error);
            return InvoiceDetailForm(model);
        }
        TempData["SuccessMessage"] = "Đã thêm sản phẩm vào hóa đơn và cập nhật tổng tiền.";
        return RedirectToAction(nameof(InvoiceDetail), new { invoiceId = model.InvoiceId });
    }

    [HttpGet]
    public IActionResult EditInvoiceDetail(int id)
    {
        var detail = storeData.FindInvoiceDetail(id);
        return detail is null ? NotFound() : InvoiceDetailForm(ToInvoiceDetailForm(detail));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult EditInvoiceDetail(int id, InvoiceDetailFormViewModel model)
    {
        if (storeData.FindInvoiceDetail(id) is null) return NotFound();
        model.Id = id;
        if (!ModelState.IsValid) return InvoiceDetailForm(model);
        if (!storeData.SaveInvoiceDetail(id, ToInvoiceDetail(model), out var error))
        {
            ModelState.AddModelError(error.Contains("hóa đơn", StringComparison.OrdinalIgnoreCase) ? nameof(model.InvoiceId) : nameof(model.ProductId), error);
            return InvoiceDetailForm(model);
        }
        TempData["SuccessMessage"] = "Đã cập nhật chi tiết hóa đơn và tính lại tổng tiền.";
        return RedirectToAction(nameof(InvoiceDetail), new { invoiceId = model.InvoiceId });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult DeleteInvoiceDetail(int id)
    {
        var detail = storeData.FindInvoiceDetail(id);
        if (detail is null) return DeleteAndReturn(false, "Không tìm thấy chi tiết hóa đơn.", "", nameof(InvoiceDetail));
        return DeleteAndReturn(storeData.DeleteInvoiceDetail(id, out var error), error, "Đã xóa chi tiết hóa đơn và cập nhật tổng tiền.", nameof(InvoiceDetail), new { invoiceId = detail.InvoiceId });
    }

    private IActionResult CategoryForm(Category model)
    {
        ViewData["FormTitle"] = model.Id == 0 ? "Thêm danh mục" : "Chỉnh sửa danh mục";
        return View("CategoryForm", model);
    }

    private IActionResult ProductForm(ProductFormViewModel model)
    {
        ViewData["FormTitle"] = model.Id == 0 ? "Thêm sản phẩm" : "Chỉnh sửa sản phẩm";
        model.CategoryOptions = storeData.Categories
            .OrderBy(category => category.Name)
            .Select(category => new SelectListItem(category.Name, category.Id.ToString(), category.Id == model.CategoryId))
            .ToList();
        return View("ProductForm", model);
    }

    private IActionResult EmployeeForm(EmployeeFormViewModel model)
    {
        ViewData["FormTitle"] = model.Id == 0 ? "Thêm nhân viên" : "Chỉnh sửa nhân viên";
        return View("EmployeeForm", model);
    }

    private IActionResult InvoiceForm(InvoiceFormViewModel model)
    {
        ViewData["FormTitle"] = model.Id == 0 ? "Tạo hóa đơn" : "Chỉnh sửa hóa đơn";
        model.EmployeeOptions = storeData.Employees
            .OrderBy(employee => employee.FullName)
            .Select(employee => new SelectListItem(employee.FullName, employee.Id.ToString(), employee.Id == model.EmployeeId))
            .ToList();
        return View("InvoiceForm", model);
    }

    private IActionResult InvoiceDetailForm(InvoiceDetailFormViewModel model)
    {
        ViewData["FormTitle"] = model.Id == 0 ? "Thêm sản phẩm vào hóa đơn" : "Chỉnh sửa chi tiết hóa đơn";
        model.InvoiceOptions = storeData.Invoices
            .OrderByDescending(invoice => invoice.CreatedAt)
            .Select(invoice => new SelectListItem($"#{invoice.Id} · {invoice.CustomerName}", invoice.Id.ToString(), invoice.Id == model.InvoiceId))
            .ToList();
        model.ProductOptions = storeData.Products
            .Where(product => product.IsActive || product.Id == model.ProductId)
            .OrderBy(product => product.Name)
            .Select(product => new SelectListItem($"{product.Name} · {product.Price.ToString("N0", System.Globalization.CultureInfo.GetCultureInfo("vi-VN"))} ₫", product.Id.ToString(), product.Id == model.ProductId))
            .ToList();
        return View("InvoiceDetailForm", model);
    }

    private IActionResult InvalidForm(string viewName, object model, string error)
    {
        ModelState.AddModelError(string.Empty, error);
        return View(viewName, model);
    }

    private IActionResult DeleteAndReturn(bool deleted, string error, string success, string action, object? routeValues = null)
    {
        TempData[deleted ? "SuccessMessage" : "ErrorMessage"] = deleted ? success : error;
        return RedirectToAction(action, routeValues);
    }

    private static List<InvoiceRowViewModel> ToInvoiceRows(IEnumerable<Invoice> invoices, IReadOnlyList<Employee> employees)
    {
        var employeeNames = employees.ToDictionary(employee => employee.Id, employee => employee.FullName);
        return invoices.Select(invoice => new InvoiceRowViewModel
        {
            Id = invoice.Id,
            CreatedAt = invoice.CreatedAt,
            EmployeeName = employeeNames.GetValueOrDefault(invoice.EmployeeId, "Không xác định"),
            CustomerName = invoice.CustomerName,
            CustomerPhone = invoice.CustomerPhone,
            TotalAmount = invoice.TotalAmount
        }).ToList();
    }

    private static Product ToProduct(ProductFormViewModel model) => new()
    {
        Name = model.Name,
        CategoryId = model.CategoryId,
        Price = model.Price,
        StockQuantity = model.StockQuantity,
        ImageFileName = model.ImageFileName,
        IsActive = model.IsActive
    };

    private static ProductFormViewModel ToProductForm(Product product) => new()
    {
        Id = product.Id,
        Name = product.Name,
        CategoryId = product.CategoryId,
        Price = product.Price,
        StockQuantity = product.StockQuantity,
        ImageFileName = product.ImageFileName,
        IsActive = product.IsActive
    };

    private static Employee ToEmployee(EmployeeFormViewModel model) => new()
    {
        FullName = model.FullName,
        Phone = model.Phone,
        Email = model.Email,
        UserId = model.UserId,
        IsActive = model.IsActive
    };

    private static EmployeeFormViewModel ToEmployeeForm(Employee employee) => new()
    {
        Id = employee.Id,
        FullName = employee.FullName,
        Phone = employee.Phone,
        Email = employee.Email,
        UserId = employee.UserId,
        IsActive = employee.IsActive
    };

    private static Invoice ToInvoice(InvoiceFormViewModel model) => new()
    {
        EmployeeId = model.EmployeeId,
        CustomerName = model.CustomerName,
        CustomerPhone = model.CustomerPhone
    };

    private static InvoiceFormViewModel ToInvoiceForm(Invoice invoice) => new()
    {
        Id = invoice.Id,
        EmployeeId = invoice.EmployeeId,
        CustomerName = invoice.CustomerName,
        CustomerPhone = invoice.CustomerPhone
    };

    private static InvoiceDetail ToInvoiceDetail(InvoiceDetailFormViewModel model) => new()
    {
        InvoiceId = model.InvoiceId,
        ProductId = model.ProductId,
        Quantity = model.Quantity
    };

    private static InvoiceDetailFormViewModel ToInvoiceDetailForm(InvoiceDetail detail) => new()
    {
        Id = detail.Id,
        InvoiceId = detail.InvoiceId,
        ProductId = detail.ProductId,
        Quantity = detail.Quantity
    };
}
