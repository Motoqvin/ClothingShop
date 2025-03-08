using ClothingStoreApp.Models;
using ClothingStoreApp.Repositories.Base;
using ClothingStoreApp.Services.Base;

namespace ClothingStoreApp.Services;

public class OrdersService : IOrdersService
{
    private readonly IOrdersRepository ordersRepository;

    public OrdersService(IOrdersRepository ordersRepository)
    {
        this.ordersRepository = ordersRepository;
    }
    public List<Order> GetAllOrders()
    {
        var orders = ordersRepository.GetAll();
        if(orders == null){
            return new List<Order>();
        }
        return orders;
    }

    public Order? GetOrderById(int id)
    {
        var order = ordersRepository.GetById(id);
        return order;
    }

    public void RemoveOrder(int id)
    {
        ordersRepository.Delete(id);
    }

    public void RenewOrder(int id, Order order)
    {
        ordersRepository.Update(id, order);
    }

    public void SendOrder(Order order)
    {
        ordersRepository.Create(order);
    }
}