using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace XuanDucShop.Models;

public class Product
{
    public int Id { get; set; }

    [Required(ErrorMessage = "Vui lòng nhập tên sản phẩm.")]
    [StringLength(150)]
    [Display(Name = "Tên sản phẩm")]
    public string Name { get; set; } = string.Empty;

    [Display(Name = "Danh mục")]
    [Range(1, int.MaxValue, ErrorMessage = "Vui lòng chọn danh mục.")]
    public int CategoryId { get; set; }

    [Range(typeof(decimal), "0.01", "999999999", ErrorMessage = "Giá sản phẩm phải lớn hơn 0.")]
    [Column(TypeName = "decimal(18,2)")]
    [Display(Name = "Đơn giá")]
    public decimal Price { get; set; }

    [Range(0, int.MaxValue, ErrorMessage = "Số lượng tồn kho không hợp lệ.")]
    [Display(Name = "Số lượng tồn")]
    public int StockQuantity { get; set; }

    [StringLength(255)]
    [Display(Name = "Tên tệp ảnh")]
    public string ImageFileName { get; set; } = string.Empty;

    [Display(Name = "Đang kinh doanh")]
    public bool IsActive { get; set; } = true;
}
