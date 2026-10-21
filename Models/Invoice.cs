namespace XuanDucShop.Models;

public class Invoice
{
    public int Id { get; set; }
    public DateTime CreatedAt { get; set; }
    public int EmployeeId { get; set; }
    public string CustomerName { get; set; } = "";
    public string CustomerPhone { get; set; } = "";
    public decimal TotalAmount { get; set; }
}
