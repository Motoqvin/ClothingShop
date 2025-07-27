using ClothingStoreApp.Core.Models;

namespace ClothingStoreApp.Core.Dtos;
public class OrderRequestDto
{
    public List<Product>? Products { get; set; }
}