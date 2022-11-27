using System.ComponentModel;

namespace WebApp.Models;

public class Shop
{
    public Guid Id { get; set; }
    [DisplayName("Номер телефона")]
    public string PhoneNumber { get; set; }
    [DisplayName("Адрес")]
    public string Adress { get; set; }
}