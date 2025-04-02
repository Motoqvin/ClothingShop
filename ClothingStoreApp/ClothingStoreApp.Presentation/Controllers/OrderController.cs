using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using ClothingStoreApp.Core.Dtos;
using ClothingStoreApp.Core.Models;
using ClothingStoreApp.Core.Responses;
using ClothingStoreApp.Core.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace ClothingStoreApp.Presentation.Controllers;

[ProducesResponseType((int)HttpStatusCode.InternalServerError, Type = typeof(InternalServerErrorResponse))]
[ProducesResponseType((int)HttpStatusCode.BadRequest, Type = typeof(BadRequestResponse))]
[ProducesResponseType((int)HttpStatusCode.NotFound, Type = typeof(NotFoundResponse))]
[ProducesResponseType((int)HttpStatusCode.OK)]
[Route("[controller]")]
public class OrderController : Controller
{
    private readonly IOrdersService ordersService;

    public OrderController(IOrdersService ordersService)
    {
        this.ordersService = ordersService;
    }

    [HttpGet]
    public IActionResult GetOrders()
    {
        return View("Index");
    }

    [HttpGet("{id:int}")]
    public IActionResult GetOrderById(int id)
    {
        var foundOrder = ordersService.GetOrderById(id);

        return foundOrder == null
         ? View("OrderInfo", model: null) 
         : View("OrderInfo", model: foundOrder);
    }

    [HttpGet("{orderId}/products")]
    public IActionResult GetOrderProducts(int orderId)
    {
        var order = ordersService.GetOrderById(orderId);
        if (order == null) return base.NotFound("Order not found");

        return base.Ok(order.Products);
    }

    [HttpGet]
    [Route("Create")]
    public IActionResult CreatePage(){
        var order = new Order();
        return View("Create", model: order);
    }

    [HttpPost]
    public IActionResult CreateOrder([FromBody]OrderRequestDto orderRequestDto){
        if(orderRequestDto == null || orderRequestDto.Products == null || !orderRequestDto.Products.Any()){
            return base.BadRequest("Order is empty");
        }

        var order = new Order(){
            Products = orderRequestDto.Products
        };
        order.UpdateTotalPrice();

        ordersService.SendOrder(order);
        return base.Ok(order);
    }

    [HttpDelete]
    [Route("{id:int}")]
    public IActionResult DeleteOrder(int id){
        var order = ordersService.GetOrderById(id);
        if(order == null){
            return base.NotFound("Order not found");
        }

        ordersService.RemoveOrder(id);
        return base.Ok();
    }

    [HttpPut("{id}")]
    public IActionResult UpdateOrder(int id, [FromBody]OrderRequestDto order){
        var orderToUpdate = ordersService.GetOrderById(id);
        if(orderToUpdate == null){
            return base.NotFound("Order not found");
        }
        orderToUpdate.Products.AddRange(order.Products ?? new List<Product>());
        orderToUpdate.UpdateTotalPrice();

        ordersService.RenewOrder(id, orderToUpdate);
        return base.Ok();
    }
}