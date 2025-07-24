using ClothingStoreApp.Core.Exceptions;
using ClothingStoreApp.Core.Models;
using ClothingStoreApp.Core.Repositories;
using ClothingStoreApp.Infrastructure.Data;

namespace ClothingStoreApp.Infrastructure.Repositories;
public class OrdersEFRepository : IOrdersRepository
{
    private readonly StoreDbContext dbContext;

    public OrdersEFRepository(StoreDbContext context)
    {
        dbContext = context;
    }

    public void Create(Order order)
    {
        dbContext.Orders.Add(order);
        dbContext.SaveChanges();
    }

    public bool Delete(int id)
    {
        var order = dbContext.Orders.FirstOrDefault(o => o.Id == id);
        if (order != null)
        {
            dbContext.Orders.Remove(order);
            dbContext.SaveChanges();
        }

        return order != null;
    }

    public List<Order> GetAll()
    {
        return [.. dbContext.Orders];
    }

    public Order? GetById(int id)
    {
        return dbContext.Orders.FirstOrDefault(o => o.Id == id);
    }

    public bool Update(int id, Order order)
    {
        var existingOrder = dbContext.Orders.FirstOrDefault(o => o.Id == id);
        if (existingOrder == null)
        {
            throw new BadRequestException(message: "Order not found!", nameof(id));
        }

        existingOrder.User = order.User;
        existingOrder.Date = order.Date;
        existingOrder.TotalPrice = order.TotalPrice;
        existingOrder.OrdersProducts = order.OrdersProducts;

        dbContext.SaveChanges();
        return true;
    }
}