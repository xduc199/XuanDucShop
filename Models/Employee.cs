using System.ComponentModel.DataAnnotations;

namespace XuanDucShop.Models;

public class Employee
{
    public int Id { get; set; }

    [Required(ErrorMessage = "Vui lòng nhập họ tên.")]
    [StringLength(150)]
    [Display(Name = "Họ và tên")]
    public string FullName { get; set; } = string.Empty;

    [Phone(ErrorMessage = "Số điện thoại không hợp lệ.")]
    [StringLength(20)]
    [Display(Name = "Điện thoại")]
    public string Phone { get; set; } = string.Empty;

    [EmailAddress(ErrorMessage = "Email không hợp lệ.")]
    [StringLength(150)]
    [Display(Name = "Email")]
    public string Email { get; set; } = string.Empty;

    [StringLength(100)]
    [Display(Name = "Mã tài khoản")]
    public string UserId { get; set; } = string.Empty;

    [Display(Name = "Đang làm việc")]
    public bool IsActive { get; set; } = true;
}
