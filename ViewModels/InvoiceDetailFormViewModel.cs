using Microsoft.AspNetCore.Mvc.Rendering;
using XuanDucShop.Models;

namespace XuanDucShop.ViewModels;

public class InvoiceDetailFormViewModel
{
    public InvoiceDetail InvoiceDetail { get; set; } = new();
    public List<SelectListItem> Invoices { get; set; } = new();
    public List<SelectListItem> Products { get; set; } = new();
}
