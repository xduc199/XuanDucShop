using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace XuanDucShop.Models.ViewModels;

public class InvoiceFormViewModel
{
    public int Id { get; set; }

    [Range(1, int.MaxValue, ErrorMessage = "Vui lòng chọn nhân viên lập hóa đơn.")]
    public int EmployeeId { get; set; }

    [Required(ErrorMessage = "Vui lòng nhập tên khách hàng.")]
    [StringLength(150, ErrorMessage = "Tên khách hàng không được vượt quá 150 ký tự.")]
    public string CustomerName { get; set; } = string.Empty;

    [Phone(ErrorMessage = "Số điện thoại khách hàng không hợp lệ.")]
    [StringLength(20, ErrorMessage = "Số điện thoại không được vượt quá 20 ký tự.")]
    public string? CustomerPhone { get; set; }

    [ValidateNever]
    public IReadOnlyList<SelectListItem> EmployeeOptions { get; set; } = [];
}
