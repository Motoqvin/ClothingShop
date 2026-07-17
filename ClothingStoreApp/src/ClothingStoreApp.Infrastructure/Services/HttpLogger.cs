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

    public void Log(HttpLog httpLog)
    {
        httpLogRepository.InsertAsync(httpLog);
    }
}