using ClothingStoreApp.Core.Models;
using ClothingStoreApp.Core.Repositories;
using ClothingStoreApp.Infrastructure.Data;

namespace ClothingStoreApp.Infrastructure.Repositories;

public class HttpLogEFRepository : IHttpLogRepository
{
    private readonly StoreDbContext dbContext;

    public HttpLogEFRepository(StoreDbContext context)
    {
        dbContext = context;
    }

    public void InsertAsync(HttpLog log)
    {
        if (log == null)
        {
            throw new ArgumentNullException(nameof(log), "Log cannot be null");
        }

        dbContext.HttpLogs.Add(log);
        dbContext.SaveChanges();
    }
}