using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ClothingStoreApp.Core.Models;

namespace ClothingStoreApp.Core.Repositories;

public interface IOrdersRepository
{
    List<Order> GetAll(string userId);
    List<Order> GetAll();
    void Create(Order order);
    bool Update(int id, Order order);
    bool Delete(int id);
    Order? GetById(int orderId);
    List<OrdersProducts> GetOrderProducts(int orderId);
    void AddProductToOrder(int orderId, int productId, int quantity);
    void RemoveProductFromOrder(int orderId, int productId);
    decimal CalculateTotalPrice(int orderId);
}