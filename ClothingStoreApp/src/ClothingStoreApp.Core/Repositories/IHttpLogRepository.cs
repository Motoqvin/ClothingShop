using ClothingStoreApp.Core.Models;

namespace ClothingStoreApp.Core.Repositories;

public interface IHttpLogRepository
{
    void InsertAsync(HttpLog log);
}