using System.Security.Claims;
using ClothingStoreApp.Core.Dtos;
using ClothingStoreApp.Core.Models;
using ClothingStoreApp.Infrastructure.Data;
using ClothingStoreApp.Presentation.Controllers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Moq;

namespace ClothingStoreApp.Tests.Controllers;
public class IdentityControllerTests
{
    private IdentityController GetController(
        out Mock<UserManager<User>> userManagerMock,
        out Mock<SignInManager<User>> signInManagerMock,
        out Mock<RoleManager<IdentityRole>> roleManagerMock)
    {
        userManagerMock = new Mock<UserManager<User>>(
            Mock.Of<IUserStore<User>>(), null, null, null, null, null, null, null, null);

        signInManagerMock = new Mock<SignInManager<User>>(
            userManagerMock.Object,
            Mock.Of<IHttpContextAccessor>(),
            Mock.Of<IUserClaimsPrincipalFactory<User>>(),
            null, null, null, null);

        roleManagerMock = new Mock<RoleManager<IdentityRole>>(
            Mock.Of<IRoleStore<IdentityRole>>(), null, null, null, null);

        var dbContextMock = new Mock<StoreDbContext>(
            new DbContextOptions<StoreDbContext>());

        return new IdentityController(userManagerMock.Object, signInManagerMock.Object, roleManagerMock.Object, dbContextMock.Object);
    }

    [Fact]
    public async Task Login_InvalidUser_ShowsErrorAndRedirects()
    {
        var controller = GetController(out var userManagerMock, out var signInManagerMock, out _);
        userManagerMock.Setup(x => x.FindByEmailAsync("test@example.com")).ReturnsAsync((User)null!);

        var loginDto = new LoginDto
        {
            Email = "test@example.com",
            Password = "123",
            RememberMe = false
        };

        var result = await controller.Login(loginDto);

        var redirect = Assert.IsType<RedirectToActionResult>(result);
        Assert.Equal("Login", redirect.ActionName);
    }

    [Fact]
    public async Task Login_SuccessfulLogin_RedirectsToHome()
    {
        var controller = GetController(out var userManagerMock, out var signInManagerMock, out _);
        var user = new User { Email = "test@example.com" };

        userManagerMock.Setup(x => x.FindByEmailAsync(user.Email)).ReturnsAsync(user);
        signInManagerMock.Setup(x =>
            x.PasswordSignInAsync(user, It.IsAny<string>(), false, true))
            .ReturnsAsync(Microsoft.AspNetCore.Identity.SignInResult.Success);

        var dto = new LoginDto
        {
            Email = "test@example.com",
            Password = "password",
            RememberMe = false
        };

        var result = await controller.Login(dto);

        var redirect = Assert.IsType<RedirectToActionResult>(result);
        Assert.Equal("Index", redirect.ActionName);
        Assert.Equal("Home", redirect.ControllerName);
    }

    [Fact]
    public async Task Logout_RedirectsToWelcome()
    {
        var controller = GetController(out _, out var signInManagerMock, out _);
        signInManagerMock.Setup(x => x.SignOutAsync()).Returns(Task.CompletedTask);

        var result = await controller.Logout();

        var redirect = Assert.IsType<RedirectToActionResult>(result);
        Assert.Equal("Welcome", redirect.ActionName);
        Assert.Equal("Home", redirect.ControllerName);
    }

    [Fact]
    public async Task EditInfo_UserNotFound_RedirectsToLogin()
    {
        var controller = GetController(out var userManagerMock, out _, out _);
        userManagerMock.Setup(x => x.GetUserAsync(It.IsAny<ClaimsPrincipal>()))
            .ReturnsAsync((User)null!);

        var result = await controller.EditInfo();

        var redirect = Assert.IsType<RedirectToActionResult>(result);
        Assert.Equal("Login", redirect.ActionName);
    }

    [Fact]
    public async Task EditInfo_ValidUser_ReturnsView()
    {
        var controller = GetController(out var userManagerMock, out _, out _);
        var user = new User { UserName = "test", Email = "test@example.com", PhoneNumber = "123" };

        userManagerMock.Setup(x => x.GetUserAsync(It.IsAny<ClaimsPrincipal>())).ReturnsAsync(user);

        var result = await controller.EditInfo();

        var view = Assert.IsType<ViewResult>(result);
        var model = Assert.IsType<User>(view.Model);
        Assert.Equal(user.UserName, model.UserName);
    }
}