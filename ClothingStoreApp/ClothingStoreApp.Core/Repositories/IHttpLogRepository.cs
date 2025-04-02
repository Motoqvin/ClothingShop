using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ClothingStoreApp.Core.Models;

namespace ClothingStoreApp.Core.Repositories;

public interface IHttpLogRepository
{
    public Task InsertAsync(HttpLog log);
}