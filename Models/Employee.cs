namespace XuanDucShop.Models;

public class Employee
{
    public int Id { get; set; }
    public string FullName { get; set; } = "";
    public string Phone { get; set; } = "";
    public string Email { get; set; } = "";
    public string UserId { get; set; } = "";
    public bool IsActive { get; set; }
}
