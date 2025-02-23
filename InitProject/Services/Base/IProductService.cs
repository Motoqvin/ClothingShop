using InitProject.Models;

namespace InitProject.Services.Base;

public interface IProductService
{
    void AddProduct(Product product);
    void ChangeProduct(int id, Product product);
    void RemoveProduct(int id);
    Product? GetProductById(int id);
}