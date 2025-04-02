using ClothingStoreApp.Core.Dtos;
using ClothingStoreApp.Core.Models;

namespace ClothingStoreApp.Core.Services;

public interface IProductService
{
    void AddProduct(ProductRequestDto product);
    void ChangeProduct(int id, Product product);
    void RemoveProduct(int id);
    Product GetProductById(int id);
    List<Product> GetAllProducts();
}