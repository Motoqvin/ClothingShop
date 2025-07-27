using ClothingStoreApp.Core.Models;

namespace ClothingStoreApp.Core.Services;

public interface IProductService
{
    void AddProduct(Product product);
    void ChangeProduct(int id, Product product);
    void RemoveProduct(int id);
    Product GetProductById(int id);
    List<Product> GetAllProducts();
}