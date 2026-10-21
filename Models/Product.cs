using System.ComponentModel.DataAnnotations;

namespace XuanDucShop.Models;

public class Product
{
    public int Id { get; set; }
    [Required(ErrorMessage = "Vui lòng nhập tên sản phẩm.")]
    [StringLength(150, ErrorMessage = "Tên sản phẩm không được vượt quá 150 ký tự.")]
    public string Name { get; set; } = string.Empty;

    [Range(1, int.MaxValue, ErrorMessage = "Vui lòng chọn danh mục.")]
    public int CategoryId { get; set; }

    [Range(typeof(decimal), "0", "999999999", ErrorMessage = "Giá sản phẩm phải lớn hơn hoặc bằng 0.")]
    public decimal Price { get; set; }

    [Range(0, int.MaxValue, ErrorMessage = "Số lượng tồn kho không hợp lệ.")]
    public int StockQuantity { get; set; }

    [StringLength(255, ErrorMessage = "Tên file ảnh không hợp lệ.")]
    public string? ImageFileName { get; set; }
    public bool IsActive { get; set; }
}
