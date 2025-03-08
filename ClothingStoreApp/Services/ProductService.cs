using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ClothingStoreApp.Dtos;
using ClothingStoreApp.Exceptions;
using ClothingStoreApp.Models;
using ClothingStoreApp.Repositories.Base;
using ClothingStoreApp.Services.Base;

namespace ClothingStoreApp.Services;
public class ProductService : IProductService
{
    private readonly IProductsRepository productsRepository;

    public ProductService(IProductsRepository productsRepository)
    {
        this.productsRepository = productsRepository;
    }

    public void AddProduct(ProductRequestDto product)
    {
        productsRepository.Create(product);
    }

    public void ChangeProduct(int id, Product changedProduct)
    {
        var isOk = productsRepository.Update(id, changedProduct);
    }

    public List<Product> GetAllProducts()
    {
        var products = productsRepository.GetAll();
        if(products == null){
            return new List<Product>();
        }
        return products;
    }

    public Product? GetProductById(int id)
    {
        var prod = productsRepository.GetById(id);
        
        return prod;
    }

    public void RemoveProduct(int id)
    {
        productsRepository.Delete(id);
    }
}