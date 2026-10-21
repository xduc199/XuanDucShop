namespace XuanDucShop.Models.ViewModels;

public class InvoiceDetailRowViewModel
{
    public int Id { get; init; }
    public int InvoiceId { get; init; }
    public string ProductName { get; init; } = string.Empty;
    public int Quantity { get; init; }
    public decimal UnitPrice { get; init; }
    public decimal LineTotal => Quantity * UnitPrice;
}
