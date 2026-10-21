using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace XuanDucShop.Models;

public class InvoiceDetail
{
    public int Id { get; set; }

    [Range(1, int.MaxValue, ErrorMessage = "Vui lòng chọn hóa đơn.")]
    [Display(Name = "Hóa đơn")]
    public int InvoiceId { get; set; }

    [Range(1, int.MaxValue, ErrorMessage = "Vui lòng chọn sản phẩm.")]
    [Display(Name = "Sản phẩm")]
    public int ProductId { get; set; }

    [Range(1, int.MaxValue, ErrorMessage = "Số lượng phải lớn hơn 0.")]
    [Display(Name = "Số lượng")]
    public int Quantity { get; set; }

    [Range(typeof(decimal), "0.01", "999999999", ErrorMessage = "Đơn giá phải lớn hơn 0.")]
    [Column(TypeName = "decimal(18,2)")]
    [Display(Name = "Đơn giá")]
    public decimal UnitPrice { get; set; }
}
