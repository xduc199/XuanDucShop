namespace XuanDucShop.Models.ViewModels;

public class InvoiceRowViewModel
{
    public int Id { get; init; }
    public DateTime CreatedAt { get; init; }
    public string EmployeeName { get; init; } = string.Empty;
    public string CustomerName { get; init; } = string.Empty;
    public string? CustomerPhone { get; init; }
    public decimal TotalAmount { get; init; }
}
