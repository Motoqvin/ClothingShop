using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using ClothingStoreApp.Core.Exceptions;
using ClothingStoreApp.Core.Responses;

namespace ClothingStoreApp.Presentation.Middlewares;
public class ExceptionHandlerMiddleware
{
    private readonly RequestDelegate next;

    public ExceptionHandlerMiddleware(RequestDelegate next)
    {
        this.next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await this.next.Invoke(context);
        }
        catch(NotFoundException ex)
        {
            context.Response.StatusCode = (int)HttpStatusCode.NotFound;
            await context.Response.WriteAsJsonAsync(new NotFoundResponse(message: ex.Message));

            context.Items["exception"] = ex.Message;
        }
        catch(BadRequestException ex)
        {
            context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
            await context.Response.WriteAsJsonAsync(new BadRequestResponse(message: ex.Message, param: ex.Param));

            context.Items["exception"] = ex.Message;
        }
        catch (Exception ex)
        {
            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
            await context.Response.WriteAsJsonAsync(new InternalServerErrorResponse(message: "Internal server error"));

            context.Items["exception"] = ex.Message;
        }
    }
}