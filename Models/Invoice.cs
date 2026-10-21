using System.ComponentModel.DataAnnotations;

namespace XuanDucShop.Models;

public class Invoice
{
    public int Id { get; set; }
    public DateTime CreatedAt { get; set; }

    [Range(1, int.MaxValue, ErrorMessage = "Nhân viên lập hóa đơn không hợp lệ.")]
    public int EmployeeId { get; set; }

    [Required(ErrorMessage = "Vui lòng nhập tên khách hàng.")]
    [StringLength(150, ErrorMessage = "Tên khách hàng không được vượt quá 150 ký tự.")]
    public string CustomerName { get; set; } = "";

    [Phone(ErrorMessage = "Số điện thoại khách hàng không hợp lệ.")]
    public string? CustomerPhone { get; set; }

    [Range(typeof(decimal), "0", "999999999999", ErrorMessage = "Tổng tiền không hợp lệ.")]
    public decimal TotalAmount { get; set; }
}
