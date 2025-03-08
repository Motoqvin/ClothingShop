using ClothingStoreApp.Models;

namespace ClothingStoreApp.Services.Base;
public interface IOrdersService
{
    void SendOrder(Order order);
    void RenewOrder(int id, Order order);
    void RemoveOrder(int id);
    Order? GetOrderById(int id);
    List<Order> GetAllOrders();
}