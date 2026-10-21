using Microsoft.AspNetCore.Mvc.Rendering;
using XuanDucShop.Models;

namespace XuanDucShop.ViewModels;

public class ProductFormViewModel
{
    public Product Product { get; set; } = new();
    public List<SelectListItem> Categories { get; set; } = new();
}
