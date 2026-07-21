using ClothingStoreApp.Core.Exceptions;
using ClothingStoreApp.Core.Models;
using ClothingStoreApp.Core.Repositories;
using ClothingStoreApp.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

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

    public List<Order> GetAll(string userId)
    {
        return [.. dbContext.Orders.Where(o => o.UserId == userId)];
    }

    public bool Update(int id, Order order)
    {
        var existingOrder = dbContext.Orders.FirstOrDefault(o => o.Id == id) ?? throw new BadRequestException(message: "Order not found!", nameof(id));
        existingOrder.User = order.User;
        existingOrder.Date = order.Date;
        existingOrder.TotalPrice = order.TotalPrice;
        existingOrder.OrdersProducts = order.OrdersProducts;

        dbContext.SaveChanges();
        return true;
    }

    public Order? GetById(int id)
    {
        return dbContext.Orders
            .Include(o => o.User)
            .Include(o => o.OrdersProducts)
            .ThenInclude(op => op.Product)
            .FirstOrDefault(o => o.Id == id);
    }

    public List<OrdersProducts> GetOrderProducts(int orderId)
    {
        return dbContext.OrdersProducts
            .Include(op => op.Product)
            .Where(op => op.OrderId == orderId)
            .ToList();
    }

    public void AddProductToOrder(int orderId, int productId, int quantity)
    {
        var existing = dbContext.OrdersProducts
            .FirstOrDefault(op => op.OrderId == orderId && op.ProductId == productId);

        if (existing != null)
        {
            existing.Quantity += quantity;
        }
        else
        {
            dbContext.OrdersProducts.Add(new OrdersProducts
            {
                OrderId = orderId,
                ProductId = productId,
                Quantity = quantity
            });
            dbContext.SaveChanges();
        }
    }

    public void RemoveProductFromOrder(int orderId, int productId)
    {
        var item = dbContext.OrdersProducts
            .FirstOrDefault(op => op.OrderId == orderId && op.ProductId == productId);

        if (item != null)
        {
            dbContext.OrdersProducts.Remove(item);
            dbContext.SaveChanges();
        }
    }

    public decimal CalculateTotalPrice(int orderId)
    {
        return dbContext.OrdersProducts
            .Include(op => op.Product)
            .Where(op => op.OrderId == orderId)
            .Sum(op => op.Product.Price * op.Quantity);
    }

    public List<Order> GetAll()
    {
        return dbContext.Orders
            .Include(o => o.User)
            .Include(o => o.OrdersProducts)
            .ThenInclude(op => op.Product)
            .ToList();
    }
}