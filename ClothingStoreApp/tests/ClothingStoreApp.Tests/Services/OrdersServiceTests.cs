using ClothingStoreApp.Core.Dtos;
using ClothingStoreApp.Core.Exceptions;
using ClothingStoreApp.Core.Models;
using ClothingStoreApp.Core.Repositories;
using ClothingStoreApp.Infrastructure.Services;
using Moq;

namespace ClothingStoreApp.Tests.Services;

public class OrdersServiceTests
{
    private readonly Mock<IOrdersRepository> ordersRepoMock;
    private readonly Mock<IProductsRepository> productsRepoMock;
    private readonly OrdersService service;
    public OrdersServiceTests()
    {
        ordersRepoMock = new Mock<IOrdersRepository>();
        productsRepoMock = new Mock<IProductsRepository>();
        service = new OrdersService(ordersRepoMock.Object, productsRepoMock.Object);
    }

    [Fact]
    public void GetAllOrders_UserIdNull_ThrowBadRequest()
    {
        Assert.Throws<BadRequestException>(() => service.GetAllOrders(null!));
    }

    [Fact]
    public void GetAllOrders_NoOrders_ThrowsNotFound()
    {
        ordersRepoMock.Setup(r => r.GetAll("user123")).Returns(new List<Order>());

        Assert.Throws<NotFoundException>(() => service.GetAllOrders("user123"));
    }

    [Fact]
    public void GetAllOrders_ValidUserId_ReturnsOrders()
    {
        var expected = new List<Order> { new Order { Id = 1, UserId = "user123" } };
        ordersRepoMock.Setup(r => r.GetAll("user123")).Returns(expected);

        var result = service.GetAllOrders("user123");

        Assert.Equal(expected, result);
    }

    [Fact]
    public void GetOrderById_NotExists_ThrowsNotFound()
    {
        ordersRepoMock.Setup(r => r.GetById(1)).Returns((Order)null!);

        Assert.Throws<NotFoundException>(() => service.GetOrderById(1));
    }

    [Fact]
    public void GetOrderById_Exists_ReturnsOrder()
    {
        var order = new Order { Id = 1 };
        ordersRepoMock.Setup(r => r.GetById(1)).Returns(order);

        var result = service.GetOrderById(1);

        Assert.Equal(order, result);
    }

    [Fact]
    public void RemoveOrder_InvalidId_ThrowsBadRequest()
    {
        Assert.Throws<BadRequestException>(() => service.RemoveOrder(0));
    }

    [Fact]
    public void RemoveOrder_NotDeleted_ThrowsBadRequest()
    {
        ordersRepoMock.Setup(r => r.Delete(1)).Returns(false);

        Assert.Throws<BadRequestException>(() => service.RemoveOrder(1));
    }

    [Fact]
    public void RemoveOrder_Valid_Deletes()
    {
        ordersRepoMock.Setup(r => r.Delete(1)).Returns(true);

        service.RemoveOrder(1);

        ordersRepoMock.Verify(r => r.Delete(1), Times.Once);
    }

    [Fact]
    public void RenewOrder_InvalidId_ThrowsBadRequest()
    {
        Assert.Throws<BadRequestException>(() => service.RenewOrder(0, new Order()));
    }

    [Fact]
    public void RenewOrder_UpdateFails_ThrowsBadRequest()
    {
        ordersRepoMock.Setup(r => r.Update(1, It.IsAny<Order>())).Returns(false);

        Assert.Throws<BadRequestException>(() => service.RenewOrder(1, new Order()));
    }

    [Fact]
    public void SendOrder_CreatesOrder()
    {
        var order = new Order();
        service.SendOrder(order);

        ordersRepoMock.Verify(r => r.Create(order), Times.Once);
    }

    [Fact]
    public void GetOrderProducts_ReturnsProductList()
    {
        var products = new List<OrdersProducts>
            {
                new OrdersProducts
                {
                    ProductId = 1,
                    Quantity = 2,
                    Product = new Product { Id = 1, Name = "Shirt", Price = 10.0m, Description = "Cotton" }
                }
            };
        ordersRepoMock.Setup(r => r.GetOrderProducts(1)).Returns(products);

        var result = service.GetOrderProducts(1);

        Assert.Single(result);
    }

    [Fact]
    public void AddProductToOrder_NotFound_ReturnsFalse()
    {
        ordersRepoMock.Setup(r => r.GetById(1)).Returns((Order)null!);
        var result = service.AddProductToOrder(1, new AddProductDto());

        Assert.False(result);
    }

    [Fact]
    public void AddProductToOrder_Adds()
    {
        var order = new Order { Id = 1 };
        var product = new Product { Id = 2, Price = 10 };
        ordersRepoMock.Setup(r => r.GetById(1)).Returns(order);
        productsRepoMock.Setup(r => r.GetById(2)).Returns(product);
        ordersRepoMock.Setup(r => r.CalculateTotalPrice(1)).Returns(100);

        var result = service.AddProductToOrder(1, new AddProductDto { ProductId = 2, Quantity = 1 });

        Assert.True(result);
        Assert.Equal(100, order.TotalPrice);
    }

    [Fact]
    public void RemoveProductFromOrder_NotFound_ReturnsFalse()
    {
        ordersRepoMock.Setup(r => r.GetById(1)).Returns((Order)null!);
        var result = service.RemoveProductFromOrder(1, 2);

        Assert.False(result);
    }

    [Fact]
    public void RemoveProductFromOrder_RemovesSuccessfully()
    {
        var order = new Order { Id = 1 };
        ordersRepoMock.Setup(r => r.GetById(1)).Returns(order);
        ordersRepoMock.Setup(r => r.CalculateTotalPrice(1)).Returns(50);

        var result = service.RemoveProductFromOrder(1, 2);

        Assert.True(result);
        Assert.Equal(50, order.TotalPrice);
    }

    [Fact]
    public void GetAllOrders_ReturnsAllOrders()
    {
        var orders = new List<Order> { new Order { Id = 1 } };
        ordersRepoMock.Setup(r => r.GetAll()).Returns(orders);

        var result = service.GetAllOrders();

        Assert.Single(result);
    }

    [Fact]
    public void GetAllOrders_ThrowsNotFound_WhenNoneExist()
    {
        ordersRepoMock.Setup(r => r.GetAll()).Returns(new List<Order>());

        Assert.Throws<NotFoundException>(() => service.GetAllOrders());
    }
}