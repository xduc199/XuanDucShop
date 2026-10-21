using Microsoft.AspNetCore.Mvc.Rendering;
using XuanDucShop.Models;

namespace XuanDucShop.ViewModels;

public class InvoiceFormViewModel
{
    public Invoice Invoice { get; set; } = new();
    public List<SelectListItem> Employees { get; set; } = new();
}
