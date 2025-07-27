namespace ClothingStoreApp.Core.Dtos;

public class ProductRequestDto
{
    public required string Name { get; set; }
    public string? Description { get; set; }
    public decimal Price { get; set; }
}