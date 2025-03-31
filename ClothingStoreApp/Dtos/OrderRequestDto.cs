using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ClothingStoreApp.Models;

namespace ClothingStoreApp.Dtos;
public class OrderRequestDto
{
    public List<Product>? Products { get; set; }
}