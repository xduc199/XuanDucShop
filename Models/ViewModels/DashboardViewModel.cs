namespace XuanDucShop.Models.ViewModels;

public class DashboardViewModel
{
    public int CategoryCount { get; init; }
    public int ProductCount { get; init; }
    public int EmployeeCount { get; init; }
    public int InvoiceCount { get; init; }
    public decimal Revenue { get; init; }
    public IReadOnlyList<InvoiceRowViewModel> RecentInvoices { get; init; } = [];
}
