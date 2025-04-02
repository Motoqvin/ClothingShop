using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ClothingStoreApp.Core.Models;

namespace ClothingStoreApp.Core.Repositories;
public interface IOrdersRepository
{
    Order? GetById(int id);
    List<Order> GetAll();
    void Create(Order order);
    bool Update(int id, Order order);
    bool Delete(int id);
}