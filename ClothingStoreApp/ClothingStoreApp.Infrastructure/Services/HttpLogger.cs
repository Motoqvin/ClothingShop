using System.Net;
using ClothingStoreApp.Core.Exceptions;
using ClothingStoreApp.Core.Models;
using ClothingStoreApp.Core.Repositories;
using ClothingStoreApp.Core.Services;

namespace ClothingStoreApp.Infrastructure.Services;

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