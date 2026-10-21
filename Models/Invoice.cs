using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace XuanDucShop.Models;

public class Invoice
{
    public int Id { get; set; }

    [Display(Name = "Ngày tạo")]
    public DateTime CreatedAt { get; set; } = DateTime.Now;

    [Range(1, int.MaxValue, ErrorMessage = "Vui lòng chọn nhân viên.")]
    [Display(Name = "Nhân viên")]
    public int EmployeeId { get; set; }

    [Required(ErrorMessage = "Vui lòng nhập tên khách hàng.")]
    [StringLength(150)]
    [Display(Name = "Tên khách hàng")]
    public string CustomerName { get; set; } = string.Empty;

    [Phone(ErrorMessage = "Số điện thoại không hợp lệ.")]
    [StringLength(20)]
    [Display(Name = "Điện thoại khách hàng")]
    public string CustomerPhone { get; set; } = string.Empty;

    [Range(typeof(decimal), "0", "999999999999", ErrorMessage = "Tổng tiền không hợp lệ.")]
    [Column(TypeName = "decimal(18,2)")]
    [Display(Name = "Tổng tiền")]
    public decimal TotalAmount { get; set; }
}
