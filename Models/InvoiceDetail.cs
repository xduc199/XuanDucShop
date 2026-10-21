using System.ComponentModel.DataAnnotations;

namespace XuanDucShop.Models;

public class InvoiceDetail
{
    public int Id { get; set; }

    [Range(1, int.MaxValue, ErrorMessage = "Mã hóa đơn không hợp lệ.")]
    public int InvoiceId { get; set; }

    [Range(1, int.MaxValue, ErrorMessage = "Mã sản phẩm không hợp lệ.")]
    public int ProductId { get; set; }

    [Range(1, int.MaxValue, ErrorMessage = "Số lượng phải lớn hơn 0.")]
    public int Quantity { get; set; }

    [Range(typeof(decimal), "0", "999999999", ErrorMessage = "Đơn giá phải lớn hơn hoặc bằng 0.")]
    public decimal UnitPrice { get; set; }
}
