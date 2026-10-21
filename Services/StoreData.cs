using XuanDucShop.Models;

namespace XuanDucShop.Services;

public class StoreData
{
    private readonly object _gate = new();

    private readonly List<Category> _categories =
    [
        new() { Id = 1, Name = "Đồ uống", Description = "Cà phê, trà và các loại nước giải khát." },
        new() { Id = 2, Name = "Thực phẩm", Description = "Thực phẩm đóng gói và đồ ăn nhẹ." },
        new() { Id = 3, Name = "Văn phòng phẩm", Description = "Dụng cụ học tập và làm việc." }
    ];

    private readonly List<Product> _products =
    [
        new() { Id = 1, Name = "Cà phê rang xay", CategoryId = 1, Price = 85000, StockQuantity = 24, IsActive = true },
        new() { Id = 2, Name = "Trà xanh túi lọc", CategoryId = 1, Price = 45000, StockQuantity = 38, IsActive = true },
        new() { Id = 3, Name = "Bánh quy bơ", CategoryId = 2, Price = 32000, StockQuantity = 16, IsActive = true },
        new() { Id = 4, Name = "Sổ tay", CategoryId = 3, Price = 28000, StockQuantity = 0, IsActive = false }
    ];

    private readonly List<Employee> _employees =
    [
        new() { Id = 1, FullName = "Nguyễn Minh An", Phone = "0901234567", Email = "an@example.com", IsActive = true },
        new() { Id = 2, FullName = "Trần Thu Hà", Phone = "0912345678", Email = "ha@example.com", IsActive = true },
        new() { Id = 3, FullName = "Lê Quốc Bảo", Phone = "0987654321", Email = "bao@example.com", IsActive = false }
    ];

    private readonly List<Invoice> _invoices =
    [
        new() { Id = 1001, CreatedAt = new DateTime(2026, 10, 20, 9, 15, 0), EmployeeId = 1, CustomerName = "Phạm Ngọc Lan", CustomerPhone = "0934567890", TotalAmount = 202000 },
        new() { Id = 1002, CreatedAt = new DateTime(2026, 10, 20, 10, 40, 0), EmployeeId = 2, CustomerName = "Hoàng Minh Đức", CustomerPhone = "0978123456", TotalAmount = 96000 },
        new() { Id = 1003, CreatedAt = new DateTime(2026, 10, 21, 8, 30, 0), EmployeeId = 1, CustomerName = "Vũ Thanh Mai", CustomerPhone = "0908765432", TotalAmount = 45000 }
    ];

    private readonly List<InvoiceDetail> _invoiceDetails =
    [
        new() { Id = 1, InvoiceId = 1001, ProductId = 1, Quantity = 2, UnitPrice = 85000 },
        new() { Id = 2, InvoiceId = 1001, ProductId = 3, Quantity = 1, UnitPrice = 32000 },
        new() { Id = 3, InvoiceId = 1002, ProductId = 2, Quantity = 2, UnitPrice = 48000 },
        new() { Id = 4, InvoiceId = 1003, ProductId = 2, Quantity = 1, UnitPrice = 45000 }
    ];

    public IReadOnlyList<Category> Categories { get { lock (_gate) return _categories.Select(Copy).ToArray(); } }
    public IReadOnlyList<Product> Products { get { lock (_gate) return _products.Select(Copy).ToArray(); } }
    public IReadOnlyList<Employee> Employees { get { lock (_gate) return _employees.Select(Copy).ToArray(); } }
    public IReadOnlyList<Invoice> Invoices { get { lock (_gate) return _invoices.Select(Copy).ToArray(); } }
    public IReadOnlyList<InvoiceDetail> InvoiceDetails { get { lock (_gate) return _invoiceDetails.Select(Copy).ToArray(); } }

    public Category? FindCategory(int id) { lock (_gate) return _categories.FirstOrDefault(item => item.Id == id) is { } item ? Copy(item) : null; }
    public Product? FindProduct(int id) { lock (_gate) return _products.FirstOrDefault(item => item.Id == id) is { } item ? Copy(item) : null; }
    public Employee? FindEmployee(int id) { lock (_gate) return _employees.FirstOrDefault(item => item.Id == id) is { } item ? Copy(item) : null; }
    public Invoice? FindInvoice(int id) { lock (_gate) return _invoices.FirstOrDefault(item => item.Id == id) is { } item ? Copy(item) : null; }
    public InvoiceDetail? FindInvoiceDetail(int id) { lock (_gate) return _invoiceDetails.FirstOrDefault(item => item.Id == id) is { } item ? Copy(item) : null; }

    public bool SaveCategory(int? id, Category input, out string error)
    {
        lock (_gate)
        {
            var current = id.HasValue ? _categories.FirstOrDefault(item => item.Id == id.Value) : null;
            if (id.HasValue && current is null) return Missing("Không tìm thấy danh mục.", out error);
            if (string.IsNullOrWhiteSpace(input.Name)) return Invalid("Tên danh mục không được để trống.", out error);

            if (current is null)
            {
                current = new Category { Id = NextId(_categories.Select(item => item.Id)) };
                _categories.Add(current);
            }
            current.Name = input.Name.Trim();
            current.Description = input.Description?.Trim() ?? string.Empty;
            error = string.Empty;
            return true;
        }
    }

    public bool DeleteCategory(int id, out string error)
    {
        lock (_gate)
        {
            var category = _categories.FirstOrDefault(item => item.Id == id);
            if (category is null) return Missing("Không tìm thấy danh mục.", out error);
            if (_products.Any(item => item.CategoryId == id)) return Invalid("Không thể xóa danh mục đang có sản phẩm.", out error);
            _categories.Remove(category);
            error = string.Empty;
            return true;
        }
    }

    public bool SaveProduct(int? id, Product input, out string error)
    {
        lock (_gate)
        {
            var current = id.HasValue ? _products.FirstOrDefault(item => item.Id == id.Value) : null;
            if (id.HasValue && current is null) return Missing("Không tìm thấy sản phẩm.", out error);
            if (string.IsNullOrWhiteSpace(input.Name)) return Invalid("Tên sản phẩm không được để trống.", out error);
            if (input.Price < 0) return Invalid("Giá sản phẩm không được âm.", out error);
            if (input.StockQuantity < 0) return Invalid("Số lượng tồn kho không được âm.", out error);
            if (!_categories.Any(item => item.Id == input.CategoryId)) return Invalid("Danh mục đã chọn không tồn tại.", out error);

            if (current is null)
            {
                current = new Product { Id = NextId(_products.Select(item => item.Id)) };
                _products.Add(current);
            }
            current.Name = input.Name.Trim();
            current.CategoryId = input.CategoryId;
            current.Price = input.Price;
            current.StockQuantity = input.StockQuantity;
            current.ImageFileName = input.ImageFileName?.Trim() ?? string.Empty;
            current.IsActive = input.IsActive;
            error = string.Empty;
            return true;
        }
    }

    public bool DeleteProduct(int id, out string error)
    {
        lock (_gate)
        {
            var product = _products.FirstOrDefault(item => item.Id == id);
            if (product is null) return Missing("Không tìm thấy sản phẩm.", out error);
            if (_invoiceDetails.Any(item => item.ProductId == id)) return Invalid("Không thể xóa sản phẩm đã xuất hiện trong hóa đơn.", out error);
            _products.Remove(product);
            error = string.Empty;
            return true;
        }
    }

    public bool SaveEmployee(int? id, Employee input, out string error)
    {
        lock (_gate)
        {
            var current = id.HasValue ? _employees.FirstOrDefault(item => item.Id == id.Value) : null;
            if (id.HasValue && current is null) return Missing("Không tìm thấy nhân viên.", out error);
            if (string.IsNullOrWhiteSpace(input.FullName)) return Invalid("Họ tên nhân viên không được để trống.", out error);

            if (current is null)
            {
                current = new Employee { Id = NextId(_employees.Select(item => item.Id)) };
                _employees.Add(current);
            }
            current.FullName = input.FullName.Trim();
            current.Phone = input.Phone?.Trim() ?? string.Empty;
            current.Email = input.Email?.Trim() ?? string.Empty;
            current.UserId = input.UserId?.Trim() ?? string.Empty;
            current.IsActive = input.IsActive;
            error = string.Empty;
            return true;
        }
    }

    public bool DeleteEmployee(int id, out string error)
    {
        lock (_gate)
        {
            var employee = _employees.FirstOrDefault(item => item.Id == id);
            if (employee is null) return Missing("Không tìm thấy nhân viên.", out error);
            if (_invoices.Any(item => item.EmployeeId == id)) return Invalid("Không thể xóa nhân viên đã lập hóa đơn.", out error);
            _employees.Remove(employee);
            error = string.Empty;
            return true;
        }
    }

    public bool SaveInvoice(int? id, Invoice input, out string error)
    {
        lock (_gate)
        {
            var current = id.HasValue ? _invoices.FirstOrDefault(item => item.Id == id.Value) : null;
            if (id.HasValue && current is null) return Missing("Không tìm thấy hóa đơn.", out error);
            if (!_employees.Any(item => item.Id == input.EmployeeId)) return Invalid("Nhân viên lập hóa đơn không tồn tại.", out error);
            if (string.IsNullOrWhiteSpace(input.CustomerName)) return Invalid("Tên khách hàng không được để trống.", out error);

            if (current is null)
            {
                current = new Invoice { Id = NextId(_invoices.Select(item => item.Id)), CreatedAt = DateTime.Now };
                _invoices.Add(current);
            }
            current.EmployeeId = input.EmployeeId;
            current.CustomerName = input.CustomerName.Trim();
            current.CustomerPhone = input.CustomerPhone?.Trim() ?? string.Empty;
            RecalculateInvoice(current);
            error = string.Empty;
            return true;
        }
    }

    public bool DeleteInvoice(int id, out string error)
    {
        lock (_gate)
        {
            var invoice = _invoices.FirstOrDefault(item => item.Id == id);
            if (invoice is null) return Missing("Không tìm thấy hóa đơn.", out error);
            if (_invoiceDetails.Any(item => item.InvoiceId == id)) return Invalid("Không thể xóa hóa đơn còn chi tiết. Hãy xóa các dòng chi tiết trước.", out error);
            _invoices.Remove(invoice);
            error = string.Empty;
            return true;
        }
    }

    public bool SaveInvoiceDetail(int? id, InvoiceDetail input, out string error)
    {
        lock (_gate)
        {
            var current = id.HasValue ? _invoiceDetails.FirstOrDefault(item => item.Id == id.Value) : null;
            if (id.HasValue && current is null) return Missing("Không tìm thấy chi tiết hóa đơn.", out error);
            if (input.Quantity <= 0) return Invalid("Số lượng phải lớn hơn 0.", out error);
            var invoice = _invoices.FirstOrDefault(item => item.Id == input.InvoiceId);
            if (invoice is null) return Invalid("Hóa đơn đã chọn không tồn tại.", out error);
            var product = _products.FirstOrDefault(item => item.Id == input.ProductId);
            if (product is null) return Invalid("Sản phẩm đã chọn không tồn tại.", out error);

            var oldInvoiceId = current?.InvoiceId;
            if (current is null)
            {
                current = new InvoiceDetail { Id = NextId(_invoiceDetails.Select(item => item.Id)) };
                _invoiceDetails.Add(current);
            }
            current.InvoiceId = input.InvoiceId;
            current.ProductId = input.ProductId;
            current.Quantity = input.Quantity;
            current.UnitPrice = product.Price;
            if (oldInvoiceId.HasValue && _invoices.FirstOrDefault(item => item.Id == oldInvoiceId.Value) is { } oldInvoice)
                RecalculateInvoice(oldInvoice);
            RecalculateInvoice(invoice);
            error = string.Empty;
            return true;
        }
    }

    public bool DeleteInvoiceDetail(int id, out string error)
    {
        lock (_gate)
        {
            var detail = _invoiceDetails.FirstOrDefault(item => item.Id == id);
            if (detail is null) return Missing("Không tìm thấy chi tiết hóa đơn.", out error);
            var invoice = _invoices.FirstOrDefault(item => item.Id == detail.InvoiceId);
            _invoiceDetails.Remove(detail);
            if (invoice is not null) RecalculateInvoice(invoice);
            error = string.Empty;
            return true;
        }
    }

    private void RecalculateInvoice(Invoice invoice) =>
        invoice.TotalAmount = _invoiceDetails
            .Where(item => item.InvoiceId == invoice.Id)
            .Sum(item => item.Quantity * item.UnitPrice);

    private static int NextId(IEnumerable<int> ids) => ids.DefaultIfEmpty(0).Max() + 1;

    private static bool Missing(string message, out string error)
    {
        error = message;
        return false;
    }

    private static bool Invalid(string message, out string error)
    {
        error = message;
        return false;
    }

    private static Category Copy(Category item) => new() { Id = item.Id, Name = item.Name, Description = item.Description };
    private static Product Copy(Product item) => new() { Id = item.Id, Name = item.Name, CategoryId = item.CategoryId, Price = item.Price, StockQuantity = item.StockQuantity, ImageFileName = item.ImageFileName, IsActive = item.IsActive };
    private static Employee Copy(Employee item) => new() { Id = item.Id, FullName = item.FullName, Phone = item.Phone, Email = item.Email, UserId = item.UserId, IsActive = item.IsActive };
    private static Invoice Copy(Invoice item) => new() { Id = item.Id, CreatedAt = item.CreatedAt, EmployeeId = item.EmployeeId, CustomerName = item.CustomerName, CustomerPhone = item.CustomerPhone, TotalAmount = item.TotalAmount };
    private static InvoiceDetail Copy(InvoiceDetail item) => new() { Id = item.Id, InvoiceId = item.InvoiceId, ProductId = item.ProductId, Quantity = item.Quantity, UnitPrice = item.UnitPrice };
}
