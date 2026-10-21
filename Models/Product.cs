namespace XuanDucShop.Models;

public class Product
{
    public int Id { get; set; }
    public string Name { get; set; } = "";
    public int CategoryId { get; set; }
    public decimal Price { get; set; }
    public int StockQuantity { get; set; }
    public string ImageFileName { get; set; } = "";
    public bool IsActive { get; set; }
}
