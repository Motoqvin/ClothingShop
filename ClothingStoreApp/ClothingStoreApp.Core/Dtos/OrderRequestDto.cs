using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ClothingStoreApp.Core.Models;

namespace ClothingStoreApp.Core.Dtos;
public class OrderRequestDto
{
    public List<Product>? Products { get; set; }
}