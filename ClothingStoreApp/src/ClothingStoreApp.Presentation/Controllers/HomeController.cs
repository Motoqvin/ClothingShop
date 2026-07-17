using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using ClothingStoreApp.Core.Models;
using ClothingStoreApp.Core.Services;
using Microsoft.AspNetCore.Authorization;
using X.PagedList.Extensions;
using System.Net;
using ClothingStoreApp.Core.Responses;

namespace ClothingStoreApp.Presentation.Controllers;

[ProducesResponseType((int)HttpStatusCode.InternalServerError, Type = typeof(InternalServerErrorResponse))]
[ProducesResponseType((int)HttpStatusCode.BadRequest, Type = typeof(BadRequestResponse))]
[ProducesResponseType((int)HttpStatusCode.NotFound, Type = typeof(NotFoundResponse))]
[ProducesResponseType((int)HttpStatusCode.OK)]
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
    public IActionResult Index(int? page)
    {
        var products = productsService.GetAllProducts();
        int pageSize = 6;
        int pageNum = page ?? 1;

        return View(products.ToPagedList(pageNum, pageSize));
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
