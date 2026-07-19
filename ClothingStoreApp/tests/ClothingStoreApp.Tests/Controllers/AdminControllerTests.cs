using ClothingStoreApp.Core.Models;
using ClothingStoreApp.Core.Services;
using ClothingStoreApp.Presentation.Controllers;
using ClothingStoreApp.Presentation.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace ClothingStoreApp.Tests.Controllers;
public class AdminControllerTests
{
    private AdminController GetController(
        out Mock<IProductService> productServiceMock,
        out Mock<IOrdersService> ordersServiceMock,
        out Mock<UserManager<User>> userManagerMock,
        out Mock<RoleManager<IdentityRole>> roleManagerMock)
    {
        productServiceMock = new Mock<IProductService>();
        ordersServiceMock = new Mock<IOrdersService>();

        userManagerMock = new Mock<UserManager<User>>(
            Mock.Of<IUserStore<User>>(), null, null, null, null, null, null, null, null);

        roleManagerMock = new Mock<RoleManager<IdentityRole>>(
            Mock.Of<IRoleStore<IdentityRole>>(), null, null, null, null);

        return new AdminController(
            productServiceMock.Object,
            ordersServiceMock.Object,
            userManagerMock.Object,
            roleManagerMock.Object
        );
    }

    [Fact]
    public void Products_ReturnsViewWithProducts()
    {
        var controller = GetController(out var productService, out _, out _, out _);
        var products = new List<Product> { new Product { Id = 1 } };
        productService.Setup(s => s.GetAllProducts()).Returns(products);

        var result = controller.Products();

        var view = Assert.IsType<ViewResult>(result);
        Assert.Equal(products, view.Model);
    }

    [Fact]
    public void Orders_ReturnsViewWithOrders()
    {
        var controller = GetController(out _, out var ordersService, out _, out _);
        var orders = new List<Order> { new Order { Id = 1 } };
        ordersService.Setup(s => s.GetAllOrders()).Returns(orders);

        var result = controller.Orders();

        var view = Assert.IsType<ViewResult>(result);
        Assert.Equal(orders, view.Model);
    }

    [Fact]
    public void Users_ReturnsViewWithUsers()
    {
        var controller = GetController(out _, out _, out var userManager, out _);
        var users = new List<User> { new User { Id = "1", Email = "test@example.com" } }.AsQueryable();
        userManager.SetupGet(um => um.Users).Returns(users);

        var result = controller.Users(1);

        var view = Assert.IsType<ViewResult>(result);
        Assert.Equal(users.ToList(), view.Model);
    }

    [Fact]
    public void CreateProduct_Post_RedirectsToProducts()
    {
        var controller = GetController(out var productService, out _, out _, out _);
        var product = new Product { Id = 1 };

        var result = controller.CreateProduct(product);

        productService.Verify(s => s.AddProduct(product), Times.Once);
        var redirect = Assert.IsType<RedirectToActionResult>(result);
        Assert.Equal("Products", redirect.ActionName);
    }

    [Fact]
    public void EditProduct_Get_ReturnsProduct()
    {
        var controller = GetController(out var productService, out _, out _, out _);
        var product = new Product { Id = 1 };
        productService.Setup(s => s.GetProductById(1)).Returns(product);

        var result = controller.EditProduct(1);

        var view = Assert.IsType<ViewResult>(result);
        Assert.Equal(product, view.Model);
    }

    [Fact]
    public void EditProduct_Get_ReturnsNotFound_IfNull()
    {
        var controller = GetController(out var productService, out _, out _, out _);
        productService.Setup(s => s.GetProductById(99)).Returns((Product)null!);

        var result = controller.EditProduct(99);

        Assert.IsType<NotFoundResult>(result);
    }

    [Fact]
    public void EditProduct_Post_UpdatesAndRedirects()
    {
        var controller = GetController(out var productService, out _, out _, out _);
        var product = new Product { Id = 1 };

        var result = controller.EditProduct(product);

        productService.Verify(s => s.ChangeProduct(product.Id, product), Times.Once);
        var redirect = Assert.IsType<RedirectToActionResult>(result);
        Assert.Equal("Products", redirect.ActionName);
    }

    [Fact]
    public void DeleteProduct_Post_RemovesAndRedirects()
    {
        var controller = GetController(out var productService, out _, out _, out _);

        var result = controller.DeleteProduct(1);

        productService.Verify(s => s.RemoveProduct(1), Times.Once);
        var redirect = Assert.IsType<RedirectToActionResult>(result);
        Assert.Equal("Products", redirect.ActionName);
    }

    [Fact]
    public async Task EditUserRoles_Post_UpdatesRolesAndRedirects()
    {
        var controller = GetController(out _, out _, out var userManager, out _);
        var user = new User { Id = "1", Email = "test@example.com" };
        var roles = new List<string> { "User" };

        userManager.Setup(x => x.FindByIdAsync(user.Id)).ReturnsAsync(user);
        userManager.Setup(x => x.GetRolesAsync(user)).ReturnsAsync(roles);
        userManager.Setup(x => x.RemoveFromRolesAsync(user, roles)).ReturnsAsync(IdentityResult.Success);
        userManager.Setup(x => x.AddToRolesAsync(user, It.IsAny<IList<string>>())).ReturnsAsync(IdentityResult.Success);

        var model = new EditUserRolesViewModel
        {
            UserId = "1",
            Email = "test@example.com",
            SelectedRoles = new List<string> { "Admin" }
        };

        var result = await controller.EditUserRoles(model);

        var redirect = Assert.IsType<RedirectToActionResult>(result);
        Assert.Equal("Users", redirect.ActionName);
    }

    [Fact]
    public void OrderDetails_ReturnsView_WhenOrderFound()
    {
        var controller = GetController(out _, out var ordersService, out _, out _);
        var order = new Order { Id = 1 };
        ordersService.Setup(s => s.GetOrderById(1)).Returns(order);

        var result = controller.OrderDetails(1);

        var view = Assert.IsType<ViewResult>(result);
        Assert.Equal(order, view.Model);
    }

    [Fact]
    public void OrderDetails_ReturnsNotFound_WhenNull()
    {
        var controller = GetController(out _, out var ordersService, out _, out _);
        ordersService.Setup(s => s.GetOrderById(1)).Returns((Order)null!);

        var result = controller.OrderDetails(1);

        Assert.IsType<NotFoundResult>(result);
    }

    [Fact]
    public void Dashboard_ReturnsViewModelWithStats()
    {
        var controller = GetController(out var productService, out var ordersService, out var userManager, out _);
        var products = new List<Product> { new Product(), new Product() };
        var orders = new List<Order> { new Order { TotalPrice = 10 }, new Order { TotalPrice = 20 } };
        var users = new List<User> { new User(), new User(), new User() }.AsQueryable();

        productService.Setup(s => s.GetAllProducts()).Returns(products);
        ordersService.Setup(s => s.GetAllOrders()).Returns(orders);
        userManager.SetupGet(um => um.Users).Returns(users);

        var result = controller.Dashboard();

        var view = Assert.IsType<ViewResult>(result);
        var model = Assert.IsType<AdminDashboardViewModel>(view.Model);

        Assert.Equal(2, model.TotalProducts);
        Assert.Equal(3, model.TotalUsers);
        Assert.Equal(2, model.TotalOrders);
        Assert.Equal(30, model.TotalRevenue);
    }
}