using System.Net;
using ClothingStoreApp.Exceptions;
using ClothingStoreApp.Models;
using ClothingStoreApp.Repositories.Base;
using ClothingStoreApp.Services.Base;

namespace ClothingStoreApp.Services;

public class HttpLogger : IHttpLogger
{
    private readonly IHttpLogRepository httpLogRepository;
    
    public HttpLogger(IHttpLogRepository httpLogRepository)
    {
        this.httpLogRepository = httpLogRepository;
    }

    public async Task LogAsync(HttpLog httpLog)
    {
        await httpLogRepository.InsertAsync(httpLog);
    }
}