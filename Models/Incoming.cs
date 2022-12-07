namespace WebApp.Models;

public class Incoming
{
    public Guid Id { get; set; }
    public Guid ProdId { get; set; }
    public DateTime Date { get; set; }
    public int Amount { get; set; }
}