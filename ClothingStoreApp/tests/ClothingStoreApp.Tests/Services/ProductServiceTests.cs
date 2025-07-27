using ClothingStoreApp.Core.Exceptions;
using ClothingStoreApp.Core.Models;
using ClothingStoreApp.Core.Repositories;
using ClothingStoreApp.Infrastructure.Services;
using Moq;

namespace ClothingStoreApp.Tests.Services;

public class ProductServiceTests
{
    private readonly Mock<IProductsRepository> productsRepoMock;
    private readonly ProductService service;
    public ProductServiceTests()
    {
        productsRepoMock = new Mock<IProductsRepository>();
        service = new ProductService(productsRepoMock.Object);
    }

    [Fact]
    public void GetAllProducts_ReturnsProducts()
    {
        var expected = new List<Product> { new Product { Id = 1, Name = "Test Product" } };
        productsRepoMock.Setup(r => r.GetAll()).Returns(expected);

        var result = service.GetAllProducts();

        Assert.Equal(expected, result);
    }

    [Fact]
    public void GetProductById_NotExists_ThrowsNotFound()
    {
        productsRepoMock.Setup(r => r.GetById(1)).Returns((Product)null!);

        Assert.Throws<NotFoundException>(() => service.GetProductById(1));
    }

    [Fact]
    public void GetProductById_Exists_ReturnsProduct()
    {
        var product = new Product { Id = 1, Name = "Test Product", Price = 100 };
        productsRepoMock.Setup(r => r.GetById(1)).Returns(product);
        var result = service.GetProductById(1);
        Assert.Equal(product, result);
    }

    [Fact]
    public void AddProduct_ValidProduct_AddsProduct()
    {
        var product = new Product { Id = 1, Name = "New Product", Price = 100 };
        productsRepoMock.Setup(r => r.Create(product));

        service.AddProduct(product);

        productsRepoMock.Verify(r => r.Create(product), Times.Once);
    }

    [Fact]
    public void UpdateProduct_NotExists_ThrowsNotFound()
    {
        var product = new Product { Id = 1, Name = "Updated Product", Price = 100 };
        productsRepoMock.Setup(r => r.Update(1, product)).Returns(false);

        Assert.Throws<NotFoundException>(() => service.ChangeProduct(1, product));
    }

    [Fact]
    public void UpdateProduct_ValidProduct_UpdatesProduct()
    {
        var product = new Product { Id = 1, Name = "Updated Product", Price = 100 };
        productsRepoMock.Setup(r => r.Update(1, product)).Returns(true);

        service.ChangeProduct(1, product);

        productsRepoMock.Verify(r => r.Update(1, product), Times.Once);
    }

    [Fact]
    public void DeleteProduct_NotExists_ThrowsNotFound()
    {
        productsRepoMock.Setup(r => r.Delete(1));

        Assert.Throws<NotFoundException>(() => service.RemoveProduct(1));
    }

    [Fact]
    public void DeleteProduct_ValidProduct_DeletesProduct()
    {
        productsRepoMock.Setup(r => r.Delete(1));
        productsRepoMock.Setup(r => r.GetById(1)).Returns(new Product { Id = 1 });

        service.RemoveProduct(1);

        productsRepoMock.Verify(r => r.Delete(1), Times.Once);
    }
}