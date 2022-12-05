namespace WebApp.Models;

public class Basket
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public Guid? ShopId { get; set; }
    public DateTime? Date { get; set; }
    public List<BasketItem> Items { get; set; } = new List<BasketItem>();
}

public class BasketItem
{
    public Guid Id { get; set; }
    public Guid BasketId { get; set; }
    public Basket Basket { get; set; }
    public Guid ProdId { get; set; }
    public int Amount { get; set; }
    public decimal Price { get; set; }
}