using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using ClothingStoreApp.Core.Models;
using ClothingStoreApp.Core.Services;
using Microsoft.AspNetCore.Authorization;

namespace ClothingStoreApp.Presentation.Controllers;


public class HomeController : Controller
{
    private readonly IProductService productsService;
    public HomeController(IProductService productsService)
    {
        this.productsService = productsService;
    }

    public IActionResult Welcome()
    {
        return View();
    }

    [Authorize(Roles = "Admin, User")]
    public IActionResult Index()
    {
        var products = new List<Product>
        {
            new Product
    {
        Id = 1,
        Name = "Classic White T-Shirt",
        Description = "A timeless white tee made from 100% organic cotton.",
        Price = 24.99m,
        ImageUrl = "https://plus.unsplash.com/premium_photo-1718913931807-4da5b5dd27fa?q=80&w=1744&auto=format&fit=crop&ixlib=rb-4.1.0&ixid=M3wxMjA3fDB8MHxwaG90by1wYWdlfHx8fGVufDB8fHx8fA%3D%3D"
    },
    new Product
    {
        Id = 2,
        Name = "Denim Jacket",
        Description = "Stylish and warm. Perfect for casual wear.",
        Price = 59.99m,
        ImageUrl = "https://images.unsplash.com/photo-1591047139829-d91aecb6caea?q=80&w=872&auto=format&fit=crop&ixlib=rb-4.1.0&ixid=M3wxMjA3fDB8MHxwaG90by1wYWdlfHx8fGVufDB8fHx8fA%3D%3D"
    },
    new ()
    {
        Id = 3,
        Name = "Running Sneakers",
        Description = "Lightweight running shoes designed for comfort and performance.",
        Price = 89.99m,
        ImageUrl = "https://images.unsplash.com/photo-1542291026-7eec264c27ff?auto=format&fit=crop&w=800&q=80"
    }
        };

        // var products = productsService.GetAllProducts();
        return View(products);
    }

    public IActionResult Privacy()
    {
        return View();
    }

    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
