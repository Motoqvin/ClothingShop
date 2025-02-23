using System.Text.Json;
using InitProject.Exceptions;
using InitProject.Models;
using InitProject.Repositories.Base;

namespace InitProject.Repositories;
public class ProductsJsonRepository : IProductsRepository
{
    public readonly string path;

    public ProductsJsonRepository(string path)
    {
        this.path = path;
    }

    public void CreateProduct(Product product)
    {
        var productsJson = File.ReadAllText(path);

        if(string.IsNullOrWhiteSpace(productsJson)){
            productsJson = "[]";
        }
        var productList = JsonSerializer.Deserialize<List<Product>>(productsJson);

        if (productList == null){
            productList = new List<Product>();
        }
        productList.Add(product);

        var newProductsJson = JsonSerializer.Serialize(productList);

        File.WriteAllText(path, newProductsJson);
    }

    public void DeleteProduct(int id)
    {
        var productsJson = File.ReadAllText(path);
        var productList = JsonSerializer.Deserialize<List<Product>>(productsJson);

        var product = productList?.FirstOrDefault(p => p.Id == id);

        productList?.Remove(product ?? new Product());

        var newProductsJson = JsonSerializer.Serialize(productList);

        File.WriteAllText(path, newProductsJson);
    }

    public Product? GetProductById(int id)
    {
        var productsJson = File.ReadAllText(path);
        var productList = JsonSerializer.Deserialize<List<Product>>(productsJson);

        return productList?.FirstOrDefault(p => p.Id == id);
    }

    public bool UpdateProduct(int id, Product product)
    {
        var productsJson = File.ReadAllText(path);
        var productList = JsonSerializer.Deserialize<List<Product>>(productsJson);

        var productToChange = productList?.FirstOrDefault(p => p.Id == id);

        if (productToChange == null){
            return false;
        }

        productToChange.Name = product.Name;
        productToChange.Description = product.Description;
        productToChange.Price = product.Price;

        var newProductsJson = JsonSerializer.Serialize(productList);
        File.WriteAllText(path, newProductsJson);
        return true;
    }
}