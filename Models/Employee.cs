using System.ComponentModel.DataAnnotations;

namespace XuanDucShop.Models;

public class Employee
{
    public int Id { get; set; }
    [Required(ErrorMessage = "Vui lòng nhập họ tên nhân viên.")]
    [StringLength(150, ErrorMessage = "Họ tên không được vượt quá 150 ký tự.")]
    public string FullName { get; set; } = string.Empty;

    [Phone(ErrorMessage = "Số điện thoại không hợp lệ.")]
    public string? Phone { get; set; }

    [EmailAddress(ErrorMessage = "Email không hợp lệ.")]
    public string? Email { get; set; }

    [StringLength(450)]
    public string? UserId { get; set; }
    public bool IsActive { get; set; }
}
