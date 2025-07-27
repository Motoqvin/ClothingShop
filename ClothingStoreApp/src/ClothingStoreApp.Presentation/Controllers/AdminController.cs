using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ClothingStoreApp.Core.Services;
using ClothingStoreApp.Core.Models;
using Microsoft.AspNetCore.Identity;
using ClothingStoreApp.Infrastructure.Services;
using System.Security.Claims;
using ClothingStoreApp.Presentation.ViewModels;
using ClothingStoreApp.Core.Exceptions;

namespace ClothingStoreApp.Presentation.Controllers;

[Authorize(Roles = "Admin")]
public class AdminController : Controller
{
    private readonly IProductService productService;
    private readonly IOrdersService ordersService;
    private readonly UserManager<User> userManager;
    private readonly RoleManager<IdentityRole> roleManager;

    public AdminController(IProductService productRepo, IOrdersService ordersRepo, UserManager<User> userManager, RoleManager<IdentityRole> roleManager)
    {
        this.productService = productRepo;
        this.ordersService = ordersRepo;
        this.userManager = userManager;
        this.roleManager = roleManager;
    }

    public IActionResult Index()
    {
        return View();
    }

    public IActionResult Products()
    {
        var products = productService.GetAllProducts();
        return View(products);
    }

    public IActionResult Orders()
    {
        var orders = ordersService.GetAllOrders();
        return View(orders);
    }

    public IActionResult Users()
    {
        var users = userManager.Users.ToList();
        return View(users);
    }

    [HttpGet]
    public IActionResult CreateProduct() => View();

    [HttpPost]
    public IActionResult CreateProduct(Product product)
    {
        productService.AddProduct(product);
        return RedirectToAction("Products");
    }

    [HttpGet]
    public IActionResult EditProduct(int id)
    {
        var product = productService.GetProductById(id);
        if (product == null) return NotFound();
        return View(product);
    }

    [HttpPost]
    public IActionResult EditProduct(Product product)
    {
        productService.ChangeProduct(product.Id, product);
        return RedirectToAction("Products");
    }

    [HttpPost]
    public IActionResult DeleteProduct(int id)
    {
        productService.RemoveProduct(id);
        return RedirectToAction("Products");
    }

    public IActionResult EditUserRoles(string id)
    {
        var user = userManager.FindByIdAsync(id).Result;
        var roles = roleManager.Roles.Select(r => r.Name).ToList();

        var userRoles = userManager.GetRolesAsync(user!).Result;

        var model = new EditUserRolesViewModel
        {
            UserId = user!.Id,
            Email = user.Email!,
            AvailableRoles = roles!,
            SelectedRoles = (List<string>)userRoles
        };

        return View(model);
    }

    [HttpPost]
    public async Task<IActionResult> EditUserRoles(EditUserRolesViewModel model)
    {
        var user = await userManager.FindByIdAsync(model.UserId) ?? throw new NotFoundException("User not found");
        var currentRoles = await userManager.GetRolesAsync(user);
        await userManager.RemoveFromRolesAsync(user, currentRoles);
        await userManager.AddToRolesAsync(user, model.SelectedRoles ?? new List<string>());

        return RedirectToAction("Users");
    }

    public IActionResult OrderDetails(int id)
    {
        var order = ordersService.GetOrderById(id);
        if (order == null) return NotFound();
        return View(order);
    }

    public IActionResult Dashboard()
    {
        var totalOrders = ordersService.GetAllOrders().Count;
        var totalUsers = userManager.Users.Count();
        var totalProducts = productService.GetAllProducts().Count;

        var totalRevenue = ordersService.GetAllOrders().Sum(o => o.TotalPrice);

        var model = new AdminDashboardViewModel
        {
            TotalOrders = totalOrders,
            TotalUsers = totalUsers,
            TotalProducts = totalProducts,
            TotalRevenue = totalRevenue
        };

        return View(model);
    }
}
