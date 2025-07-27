using ClothingStoreApp.Core.Dtos;
using ClothingStoreApp.Core.Exceptions;
using ClothingStoreApp.Core.Models;
using ClothingStoreApp.Core.Repositories;
using ClothingStoreApp.Core.Services;

namespace ClothingStoreApp.Infrastructure.Services;

public class OrdersService : IOrdersService
{
    private readonly IOrdersRepository ordersRepository;
    private readonly IProductsRepository productRepo;

    public OrdersService(IOrdersRepository ordersRepository, IProductsRepository productsRepository)
    {
        this.productRepo = productsRepository;
        this.ordersRepository = ordersRepository;
    }
    public List<Order> GetAllOrders(string userId)
    {
        if (string.IsNullOrEmpty(userId))
        {
            throw new BadRequestException(message: "User ID cannot be null or empty!", nameof(userId));
        }

        var orders = ordersRepository.GetAll(userId);
        if (orders == null || orders.Count == 0)
        {
            throw new NotFoundException(message: "Orders not found for the specified user!");
        }

        return orders;
    }

    public Order GetOrderById(int id)
    {
        var order = ordersRepository.GetById(id) ?? throw new NotFoundException(message: "Order not found!");
        return order;
    }

    public void RemoveOrder(int id)
    {
        if (id <= 0)
        {
            throw new BadRequestException(message: "Invalid id!", nameof(id));
        }
        var deletedLines = ordersRepository.Delete(id);
        if (!deletedLines)
        {
            throw new BadRequestException(message: "Order not deleted!", nameof(deletedLines));
        }
    }

    public void RenewOrder(int id, Order order)
    {
        if (id <= 0)
        {
            throw new BadRequestException(message: "Invalid id!", nameof(id));
        }

        var updatedLines = ordersRepository.Update(id, order);
        if (!updatedLines)
        {
            throw new BadRequestException(message: "Order not updated!", nameof(updatedLines));
        }
    }

    public void SendOrder(Order order)
    {
        ordersRepository.Create(order);
    }
    
    public List<object> GetOrderProducts(int orderId)
    {
        var list = ordersRepository.GetOrderProducts(orderId);
        return list.Select(op => new
        {
            id = op.ProductId,
            name = op.Product.Name,
            description = op.Product.Description,
            price = op.Product.Price,
            quantity = op.Quantity
        }).ToList<object>();
    }

    public bool AddProductToOrder(int orderId, AddProductDto dto)
    {
        var order = ordersRepository.GetById(orderId);
        var product = productRepo.GetById(dto.ProductId);
        if (order == null || product == null) return false;

        ordersRepository.AddProductToOrder(orderId, dto.ProductId, dto.Quantity);

        order.TotalPrice = ordersRepository.CalculateTotalPrice(orderId);

        return true;
    }

    public bool RemoveProductFromOrder(int orderId, int productId)
    {
        var order = ordersRepository.GetById(orderId);
        if (order == null) return false;

        ordersRepository.RemoveProductFromOrder(orderId, productId);

        order.TotalPrice = ordersRepository.CalculateTotalPrice(orderId);

        return true;
    }

    public List<Order> GetAllOrders()
    {
        var orders = ordersRepository.GetAll();
        if (orders == null || orders.Count == 0)
        {
            throw new NotFoundException(message: "No orders found!");
        }

        return orders;
    }
}