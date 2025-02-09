using System.Net;
using InitProject.Exceptions;
using InitProject.Models;
using InitProject.Repositories;
using InitProject.Repositories.Base;
using InitProject.Services;
using InitProject.Services.Base;
using Microsoft.AspNetCore.Mvc;

namespace InitProject.Controllers;

[Route("api/[controller]")]
[ProducesResponseType(400, Type = typeof(List<ValidationResponse>))]
public class ProductController : ControllerBase
{
    private readonly IProductService productsService;

    public ProductController(IProductService productsService)
    {
        this.productsService = productsService;
    }

    [HttpPost]
    public ActionResult CreateProduct([FromBody]Product product){
        try{
            productsService.AddProduct(product);

            return base.Ok();
        }
        catch(ValidationException ex){
            return base.BadRequest(ex.validationResponses);
        }
        catch(Exception ex){
            System.Console.WriteLine(ex);
            return base.StatusCode((int)HttpStatusCode.InternalServerError);
        }
    }

    [HttpGet]
    [Route("{id}")]
    public ActionResult<Product> GetProductById(int id){
        try{
            var product = productsService.GetProductById(id);

            return base.Ok(product);
        }
        catch (ValidationException ex){
            return base.BadRequest(ex.validationResponses);
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
        catch (ValidationException ex){
            return base.BadRequest(ex.validationResponses);
        }
        catch (Exception) { 
            return base.StatusCode((int)HttpStatusCode.InternalServerError);
        }
    }

    [HttpPut]
    public ActionResult UpdateProduct([FromBody]Product product){
        try{
            productsService.ChangeProduct(product.Id, product);

            return base.Ok();
        }
        catch (ValidationException ex){
            return base.BadRequest(ex.validationResponses);
        }
        catch (Exception) {
            return base.StatusCode((int)HttpStatusCode.InternalServerError);
        }
    }
}