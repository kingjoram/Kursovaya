using System.ComponentModel;

namespace WebApp.Models;

public class Incoming
{
    public Guid Id { get; set; }
    public Guid ProdId { get; set; }
    public DateTime Date { get; set; }
    [DisplayName("Количество поступивших товаров")]
    public int Amount { get; set; }
}