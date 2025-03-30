using ClothingStoreApp.Exceptions;
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
            throw new NotFoundException(message: "Orders not found!");
        }
        return orders;
    }

    public Order GetOrderById(int id)
    {
        var order = ordersRepository.GetById(id);
        if(order == null){
            throw new NotFoundException(message: "Order not found!");
        }
        return order;
    }

    public void RemoveOrder(int id)
    {
        if(id <= 0){
            throw new BadRequestException(message: "Invalid id!", nameof(id));
        }
        var deletedLines = ordersRepository.Delete(id);
        if(!deletedLines){
            throw new BadRequestException(message: "Order not deleted!", nameof(deletedLines));
        }
    }

    public void RenewOrder(int id, Order order)
    {
        if(id <= 0){
            throw new BadRequestException(message: "Invalid id!", nameof(id));
        }

        if(order.Products.Count == 0){
            throw new BadRequestException(message: "Order must have at least one product!", nameof(order.Products));
        }

        var updatedLines = ordersRepository.Update(id, order);
        if(!updatedLines){
            throw new BadRequestException(message: "Order not updated!", nameof(updatedLines));
        }
    }

    public void SendOrder(Order order)
    {
        if(order.Products.Count == 0){
            throw new BadRequestException(message: "Order must have at least one product!", nameof(order.Products));
        }

        ordersRepository.Create(order);
    }
}