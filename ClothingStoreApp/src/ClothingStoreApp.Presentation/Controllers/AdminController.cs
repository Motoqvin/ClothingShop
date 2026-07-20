using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ClothingStoreApp.Core.Services;
using ClothingStoreApp.Core.Models;
using Microsoft.AspNetCore.Identity;
using ClothingStoreApp.Infrastructure.Services;
using System.Security.Claims;
using ClothingStoreApp.Presentation.ViewModels;
using ClothingStoreApp.Core.Exceptions;
using X.PagedList.Extensions;
using System.Threading.Tasks;
using System.Net;
using ClothingStoreApp.Core.Responses;

namespace ClothingStoreApp.Presentation.Controllers;

[ProducesResponseType((int)HttpStatusCode.InternalServerError, Type = typeof(InternalServerErrorResponse))]
[ProducesResponseType((int)HttpStatusCode.BadRequest, Type = typeof(BadRequestResponse))]
[ProducesResponseType((int)HttpStatusCode.NotFound, Type = typeof(NotFoundResponse))]
[ProducesResponseType((int)HttpStatusCode.OK)]
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

    public IActionResult Users(int? page)
    {
        var users = userManager.Users.ToList();
        int pageSize = 6;
        int pageNum = page ?? 1;

        return View(users.ToPagedList(pageNum, pageSize));
    }

    [HttpGet]
    public IActionResult CreateProduct() => View();

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult CreateProduct(Product product, IFormFile? ImageFile)
    {
        if (!ModelState.IsValid) throw new BadRequestException("Product must be valid", nameof(product));

        if (ImageFile != null && ImageFile.Length > 0)
        {
            product.ImageUrl = SaveProductImage(ImageFile);
        }

        productService.AddProduct(product);
        TempData["Success"] = $"\"{product.Name}\" was created successfully.";
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
    [ValidateAntiForgeryToken]
    public IActionResult EditProduct(Product product, IFormFile? ImageFile)
    {
        if (!ModelState.IsValid)
        {
            return View(product);
        }

        var existingProduct = productService.GetProductById(product.Id);

        if (ImageFile != null && ImageFile.Length > 0)
        {
            product.ImageUrl = SaveProductImage(ImageFile);

            if (!string.IsNullOrEmpty(existingProduct?.ImageUrl))
            {
                DeleteProductImage(existingProduct.ImageUrl);
            }
        }
        else
        {
            product.ImageUrl = existingProduct?.ImageUrl;
        }

        productService.ChangeProduct(product.Id, product);
        TempData["Success"] = $"\"{product.Name}\" was updated successfully.";
        return RedirectToAction("Products");
    }

    private static string SaveProductImage(IFormFile imageFile)
    {
        var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads", "products");
        Directory.CreateDirectory(uploadsFolder);

        var fileName = Guid.NewGuid() + Path.GetExtension(imageFile.FileName);
        var filePath = Path.Combine(uploadsFolder, fileName);

        using (var stream = new FileStream(filePath, FileMode.Create))
        {
            imageFile.CopyTo(stream);
        }

        return $"/uploads/products/{fileName}";
    }

    private static void DeleteProductImage(string imageUrl)
    {
        var relativePath = imageUrl.TrimStart('/', '\\').Replace('/', Path.DirectorySeparatorChar);
        var oldFilePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", relativePath);
        if (System.IO.File.Exists(oldFilePath))
        {
            System.IO.File.Delete(oldFilePath);
        }
    }

    [HttpPost]
    public IActionResult DeleteProduct(int id)
    {
        productService.RemoveProduct(id);
        TempData["Success"] = "Product was deleted successfully.";
        return RedirectToAction(nameof(Products));
    }

    public async Task<IActionResult> EditUserRoles(string id)
    {
        var user = await userManager.FindByIdAsync(id);
        if (user == null) return NotFound();

        var roles = roleManager.Roles.Select(r => r.Name).ToList();

        var userRoles = await userManager.GetRolesAsync(user!);

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
    [ValidateAntiForgeryToken]
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
