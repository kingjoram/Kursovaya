using System.ComponentModel;

namespace WebApp.Models;

public class Product
{
    public Guid Id { get; set; }
    [DisplayName("Наименование товара")]
    public string Name { get; set; }
    [DisplayName("Цена")]
    public decimal Price { get; set; }
}