using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ClothingStoreApp.Core.Models;
public class Order
{
    public int Id { get; set; }
    public DateTime Date { get; set; } = DateTime.UtcNow;
    public decimal TotalPrice { get; set; }
    public string UserId { get; set; } = null!;
    public User User { get; set; } = null!;
    public List<OrdersProducts> OrdersProducts { get; set; } = new List<OrdersProducts>();
}