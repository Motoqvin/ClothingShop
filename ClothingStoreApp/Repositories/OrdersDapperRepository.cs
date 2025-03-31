using ClothingStoreApp.Models;
using ClothingStoreApp.Repositories.Base;
using Dapper;
using Microsoft.Data.SqlClient;

namespace ClothingStoreApp.Repositories;

public class OrdersDapperRepository : IOrdersRepository
{
    private readonly string ConnStr;
    public OrdersDapperRepository(string connStr)
    {
        ConnStr = connStr;
    }
    public void Create(Order order)
    {
        using var conn = new SqlConnection(ConnStr);

        var id = conn.ExecuteScalar<int>(@"insert into Orders (Date, TotalPrice) 
        output inserted.Id
        values (@Date, @TotalPrice)", new { order.Date, order.TotalPrice });

        order.Id = id;

        if (order.Products != null)
        {
            foreach (var product in order.Products)
            {
                int productId = product.Id;

                if (productId == 0)
                {
                    string insertProductSql = @"
                        insert into Products (Name, Description, Price)
                        output inserted.Id
                        values (@Name, @Description, @Price);";

                    productId = conn.ExecuteScalar<int>(insertProductSql, new 
                    { 
                        product.Name, 
                        product.Description, 
                        product.Price 
                    });

                    product.Id = productId;
                }

                conn.Execute(@"insert into OrdersProducts (OrderId, ProductId) 
                            values (@OrderId, @ProductId);", 
                            new { OrderId = id, ProductId = productId });
            }
        }
    }

    public bool Delete(int id)
    {
        using var conn = new SqlConnection(ConnStr);
        conn.Execute("delete from OrdersProducts where OrderId = @Id;", new { Id = id });

        int affected = conn.Execute("delete from Orders where Id = @Id;", new { Id = id });

        return affected > 0;
    }

    public List<Order> GetAll()
    {
        using var conn = new SqlConnection(ConnStr);
        var orders = conn.Query<Order>("select * from Orders");

        return orders.ToList();
    }

    public Order? GetById(int id)
    {
        using var conn = new SqlConnection(ConnStr);
        var order = conn.QueryFirstOrDefault<Order>("select * from Orders where Id = @id", new {id});
        if (order == null)
        {
            return null;
        }

        var productsList = conn.Query<Product>(@"select p.* from OrdersProducts op
         join Products p on op.ProductId = p.Id
         where op.OrderId = @id", new {id});
        order.Products = productsList.ToList();

        return order;
    }

    public bool Update(int id, Order order)
    {
        using var conn = new SqlConnection(ConnStr);

        conn.Execute("delete from OrdersProducts where OrderId = @Id;", new { Id = id });

        var affected = conn.Execute("update Orders set TotalPrice = @TotalPrice where Id = @Id", new { order.TotalPrice, Id = id });

        if (order.Products != null && order.Products.Any())
        {
            foreach (var product in order.Products)
            {
                if (product.Id == 0)
                {
                    product.Id = conn.ExecuteScalar<int>(@"insert into Products (Name, Description, Price)
                        output inserted.Id
                        values (@Name, @Description, @Price)", new { product.Name, product.Description, product.Price });
                }
                conn.Execute("insert into OrdersProducts (OrderId, ProductId) values (@OrderId, @ProductId);", 
                    new { OrderId = order.Id, ProductId = product.Id });
            }
        }

        return affected > 0;
    }
}