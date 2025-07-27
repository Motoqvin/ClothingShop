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
        var products = productsService.GetAllProducts();
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
