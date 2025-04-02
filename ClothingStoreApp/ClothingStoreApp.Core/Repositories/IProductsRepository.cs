using ClothingStoreApp.Core.Dtos;
using ClothingStoreApp.Core.Models;

namespace ClothingStoreApp.Core.Repositories;
public interface IProductsRepository
{
    Product? GetById(int id);
    List<Product> GetAll();
    void Create(ProductRequestDto product);
    bool Update(int id, Product product);
    void Delete(int id);
}