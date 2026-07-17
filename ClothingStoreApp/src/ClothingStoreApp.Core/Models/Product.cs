namespace ClothingStoreApp.Core.Models;

public class Product
{
    public int Id { get; set; }
    public string? Name { get; set; }
    public string? Description { get; set; }
    public decimal Price { get; set; }
    public string? ImageUrl { get; set; }
    public List<OrdersProducts> OrdersProducts { get; set; } = new List<OrdersProducts>();
}