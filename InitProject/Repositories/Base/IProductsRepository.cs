using InitProject.Models;

namespace InitProject.Repositories.Base;
public interface IProductsRepository
{
    Product? GetProductById(int id);
    void CreateProduct(Product product);
    bool UpdateProduct(int id, Product product);
    void DeleteProduct(int id);
}