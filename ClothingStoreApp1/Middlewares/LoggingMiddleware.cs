using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ClothingStoreApp.Exceptions;
using ClothingStoreApp.Models;
using ClothingStoreApp.Services.Base;

namespace ClothingStoreApp.Middlewares;
public class LoggingMiddleware
{
    private readonly RequestDelegate next;

    public LoggingMiddleware(RequestDelegate next)
    {
        this.next = next;
    }

    public async Task InvokeAsync(HttpContext context, IHttpLogger logger)
    {
        var requestId = Guid.NewGuid();
        var creationDateTime = DateTime.UtcNow;

        context.Request.EnableBuffering();
        var requestBody = await new StreamReader(context.Request.Body).ReadToEndAsync();
        context.Request.Body.Position = 0;

        var requestHeaders = string.Join(", ", context.Request.Headers.Select(h => $"{h.Key}: {h.Value}"));
        var responseHeaders = string.Join(", ", context.Response.Headers.Select(h => $"{h.Key}: {h.Value}"));
        var methodType = 1;
        switch(context.Request.Method){
            case "GET":
                methodType = 1;
                break;
            case "POST":
                methodType = 2;
                break;
            case "DELETE":
                methodType = 3;
                break;
            case "PUT":
                methodType = 4;
                break;
            default:
                throw new BadRequestException(message: $"Method {context.Request.Method} is not supported.", param: nameof(context.Request.Method));
        }

        var originalBodyStream = context.Response.Body;
        using var responseBodyStream = new MemoryStream();
        context.Response.Body = responseBodyStream;
        
        await this.next(context);

        responseBodyStream.Seek(0, SeekOrigin.Begin);
        var responseBody = await new StreamReader(responseBodyStream).ReadToEndAsync();
        responseBodyStream.Seek(0, SeekOrigin.Begin);

        await responseBodyStream.CopyToAsync(originalBodyStream);

        var endDateTime = DateTime.UtcNow;

        var log = new HttpLog
        {
            RequestId = requestId,
            Url = context.Request.Path,
            RequestBody = requestBody,
            RequestHeaders = requestHeaders,
            MethodType = methodType,
            ResponseBody = responseBody,
            ResponseHeaders = responseHeaders,
            StatusCode = (int)context.Response.StatusCode,
            CreationDateTime = creationDateTime,
            EndDateTime = endDateTime,
            ClientIp = context.Connection.RemoteIpAddress?.ToString() ?? "Unknown"
        };

        await logger.LogAsync(log);
    }
}