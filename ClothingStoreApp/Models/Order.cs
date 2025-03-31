using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ClothingStoreApp.Repositories;

namespace ClothingStoreApp.Models;
public class Order
{
    public int Id { get; set; }
    public DateTime Date { get; set; } = DateTime.Now;
    public List<Product> Products { get; set; } = new List<Product>();
    public decimal TotalPrice { get; set; }

    public void UpdateTotalPrice()
    {
        TotalPrice = Products.Sum(p => p.Price);
    }

}