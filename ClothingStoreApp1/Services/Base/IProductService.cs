using ClothingStoreApp.Dtos;
using ClothingStoreApp.Models;

namespace ClothingStoreApp.Services.Base;

public interface IProductService
{
    void AddProduct(ProductRequestDto product);
    void ChangeProduct(int id, Product product);
    void RemoveProduct(int id);
    Product GetProductById(int id);
    List<Product> GetAllProducts();
}