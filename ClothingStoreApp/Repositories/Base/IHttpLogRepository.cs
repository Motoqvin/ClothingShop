using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ClothingStoreApp.Models;

namespace ClothingStoreApp.Repositories.Base;

public interface IHttpLogRepository
{
    public Task InsertAsync(HttpLog log);
}