using ClothingStoreApp.Dtos;
using ClothingStoreApp.Models;

namespace ClothingStoreApp.Repositories.Base;
public interface IProductsRepository
{
    Product? GetById(int id);
    List<Product> GetAll();
    void Create(ProductRequestDto product);
    bool Update(int id, Product product);
    void Delete(int id);
}