using System.Data.SqlClient;
using Dapper;
using InitProject.Models;
using InitProject.Repositories.Base;
using Microsoft.Data.SqlClient;

namespace InitProject.Repositories;
public class ProductsDapperRepository : IProductsRepository
{
    private readonly string ConnStr;
    public ProductsDapperRepository(string connStr)
    {
        ConnStr = connStr;
    }

    public void CreateProduct(Product product)
    {
        var conn = new SqlConnection(ConnStr);

        conn.Open();
        
        conn.Execute(@"insert into Products (Id, Name, Description, Price) 
        values (@Id, @Name, @Description, @Price)", product);

        conn.Close();
    }

    public void DeleteProduct(int id)
    {
        var conn = new SqlConnection(ConnStr);

        conn.Open();

        conn.Execute("delete from Products where Id = @id", id);

        conn.Close();
    }

    public Product? GetProductById(int id)
    {
        var conn = new SqlConnection(ConnStr);

        conn.Open();

        var products = conn.Query<Product>("select * from Products where Id = @Id", new { 
            Id = id 
        });

        conn.Close();
        var prodList = products.ToList();

        return prodList.FirstOrDefault();
    }

    public bool UpdateProduct(int id, Product product)
    {
        var conn = new SqlConnection(ConnStr);

        conn.Open();

        var affected = conn.Execute("update Products set Name = @Name, Description = @Description, Price = @Price where Id = @Id", product);

        conn.Close();

        return affected > 0;
    }

}