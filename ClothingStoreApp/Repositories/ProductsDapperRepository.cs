using System.Data.SqlClient;
using ClothingStoreApp.Dtos;
using ClothingStoreApp.Models;
using ClothingStoreApp.Repositories.Base;
using Dapper;
using Microsoft.Data.SqlClient;

namespace ClothingStoreApp.Repositories;
public class ProductsDapperRepository : IProductsRepository
{
    private readonly string ConnStr;
    public ProductsDapperRepository(string connStr)
    {
        ConnStr = connStr;
    }

    public void Create(ProductRequestDto product)
    {
        using var conn = new SqlConnection(ConnStr);
        
        conn.Execute(@"insert into Products (Name, Description, Price) 
        values (@Name, @Description, @Price)", product);
    }

    public void Delete(int id)
    {
        using var conn = new SqlConnection(ConnStr);

        conn.Execute("delete from OrdersProducts where ProductId = @id", new { Id = id });
        conn.Execute("delete from Products where Id = @id", new { Id = id });
    }

    public List<Product> GetAll()
    {
        using var conn = new SqlConnection(ConnStr);
        var products = conn.Query<Product>("select * from Products");

        return products.ToList();
    }

    public Product? GetById(int id)
    {
        using var conn = new SqlConnection(ConnStr);
        var product = conn.QueryFirstOrDefault<Product>("select * from Products where Id = @Id", new { Id = id });

        return product;
    }

    public bool Update(int id, Product product)
    {
        using var conn = new SqlConnection(ConnStr);

        var affected = conn.Execute("update Products set Name = @Name, Description = @Description, Price = @Price where Id = @Id", new {
            Id = id,
            Name = product.Name,
            Description = product.Description,
            Price = product.Price
        });

        return affected > 0;
    }

}