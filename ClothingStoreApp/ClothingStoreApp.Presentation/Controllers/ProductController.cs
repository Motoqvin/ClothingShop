using System.Net;
using Microsoft.AspNetCore.Mvc;
using ClothingStoreApp.Core.Responses;
using ClothingStoreApp.Core.Services;
using ClothingStoreApp.Core.Dtos;
using ClothingStoreApp.Core.Models;

namespace ClothingStoreApp.Presentation.Controllers;

[Route("[controller]")]
[ProducesResponseType((int)HttpStatusCode.InternalServerError, Type = typeof(InternalServerErrorResponse))]
[ProducesResponseType((int)HttpStatusCode.BadRequest, Type = typeof(BadRequestResponse))]
[ProducesResponseType((int)HttpStatusCode.NotFound, Type = typeof(NotFoundResponse))]
[ProducesResponseType((int)HttpStatusCode.OK)]
public class ProductController : Controller
{
    private readonly IProductService productsService;

    public ProductController(IProductService productsService)
    {
        this.productsService = productsService;
    }

    [HttpGet]
    public IActionResult Index()
    {
        return View();
    }

    [HttpPost]
    public ActionResult CreateProduct([FromBody]ProductRequestDto product){
        try{
            productsService.AddProduct(product);

            return base.Ok(product.Name);
        }
        catch(Exception ex){
            Console.WriteLine(ex);
            return base.StatusCode((int)HttpStatusCode.InternalServerError);
        }
    }

    [HttpGet]
    [Route("{id}")]
    public ActionResult<Product> GetProductById(int id){
        try{
            var product = productsService.GetProductById(id);

            return base.View("ProductInfo", model: product);
        }
        catch (Exception){
            return base.StatusCode((int)HttpStatusCode.InternalServerError);
        }
    }

    [HttpDelete]
    [Route("{id}")]
    public ActionResult DeleteProduct(int id){
        try{
            productsService.RemoveProduct(id);

            return base.Ok();
        }
        catch (Exception) { 
            return base.StatusCode((int)HttpStatusCode.InternalServerError);
        }
    }

    [HttpPut("{id}")]
    public ActionResult UpdateProduct(int id, [FromBody]Product product){
        try{
            productsService.ChangeProduct(id, product);

            return base.Ok();
        }
        catch (Exception) {
            return base.StatusCode((int)HttpStatusCode.InternalServerError);
        }
    }
}