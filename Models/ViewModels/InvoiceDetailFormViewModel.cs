using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace XuanDucShop.Models.ViewModels;

public class InvoiceDetailFormViewModel
{
    public int Id { get; set; }

    [Range(1, int.MaxValue, ErrorMessage = "Vui lòng chọn hóa đơn.")]
    public int InvoiceId { get; set; }

    [Range(1, int.MaxValue, ErrorMessage = "Vui lòng chọn sản phẩm.")]
    public int ProductId { get; set; }

    [Range(1, int.MaxValue, ErrorMessage = "Số lượng phải lớn hơn 0.")]
    public int Quantity { get; set; } = 1;

    [ValidateNever]
    public IReadOnlyList<SelectListItem> InvoiceOptions { get; set; } = [];

    [ValidateNever]
    public IReadOnlyList<SelectListItem> ProductOptions { get; set; } = [];
}
