using ClothingStoreApp.Core.Models;

namespace ClothingStoreApp.Core.Services;
public interface IOrdersService
{
    void SendOrder(Order order);
    void RenewOrder(int id, Order order);
    void RemoveOrder(int id);
    Order GetOrderById(int id);
    List<Order> GetAllOrders();
}