using ClothingStoreApp.Core.Models;

namespace ClothingStoreApp.Core.Services;
public interface IHttpLogger
{
    public Task LogAsync(HttpLog log);
}