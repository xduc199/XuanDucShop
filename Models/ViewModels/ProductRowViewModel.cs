namespace XuanDucShop.Models.ViewModels;

public class ProductRowViewModel
{
    public int Id { get; init; }
    public string Name { get; init; } = string.Empty;
    public string CategoryName { get; init; } = string.Empty;
    public decimal Price { get; init; }
    public int StockQuantity { get; init; }
    public string? ImageFileName { get; init; }
    public bool IsActive { get; init; }
}
