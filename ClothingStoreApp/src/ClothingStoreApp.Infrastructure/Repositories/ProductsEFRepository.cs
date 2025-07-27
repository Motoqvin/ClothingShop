using ClothingStoreApp.Core.Exceptions;
using ClothingStoreApp.Core.Models;
using ClothingStoreApp.Core.Repositories;
using ClothingStoreApp.Infrastructure.Data;

namespace ClothingStoreApp.Infrastructure.Repositories;

public class ProductsEFRepository : IProductsRepository
{
    private readonly StoreDbContext dbContext;
    public ProductsEFRepository(StoreDbContext context)
    {
        dbContext = context;
    }
    public void Create(Product product)
    {
        dbContext.Products.Add(product);
        dbContext.SaveChanges();
    }

    public void Delete(int id)
    {
        _ = dbContext.Products.Remove(new Product { Id = id });
        dbContext.SaveChanges();
    }

    public List<Product> GetAll()
    {
        return [.. dbContext.Products];
    }

    public Product? GetById(int id)
    {
        return dbContext.Products.FirstOrDefault(p => p.Id == id);
    }

    public bool Update(int id, Product product)
    {
        var existingProduct = dbContext.Products.FirstOrDefault(p => p.Id == id) ?? throw new NotFoundException(message: "Product not found!");
        existingProduct.Name = product.Name;
        existingProduct.Description = product.Description;
        existingProduct.Price = product.Price;

        dbContext.SaveChanges();
        return true;
    }
}