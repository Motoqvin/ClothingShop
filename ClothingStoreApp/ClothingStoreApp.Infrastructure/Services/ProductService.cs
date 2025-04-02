using ClothingStoreApp.Core.Dtos;
using ClothingStoreApp.Core.Exceptions;
using ClothingStoreApp.Core.Models;
using ClothingStoreApp.Core.Repositories;
using ClothingStoreApp.Core.Services;

namespace ClothingStoreApp.Infrastructure.Services;
public class ProductService : IProductService
{
    private readonly IProductsRepository productsRepository;

    public ProductService(IProductsRepository productsRepository)
    {
        this.productsRepository = productsRepository;
    }

    public void AddProduct(ProductRequestDto product)
    {
        if(product == null){
            throw new BadRequestException(message: "Product is null!", nameof(product));
        }

        if(string.IsNullOrWhiteSpace(product.Name)){
            throw new BadRequestException(message: "Product must have a name!", nameof(product.Name));
        }

        if(product.Price <= 0){
            throw new BadRequestException(message: "Invalid price!", nameof(product.Price));
        }

        productsRepository.Create(product);
    }

    public void ChangeProduct(int id, Product changedProduct)
    {
        if(id <= 0){
            throw new BadRequestException(message: "Invalid id!", nameof(id));
        }

        if(changedProduct == null){
            throw new BadRequestException(message: "Product is null!", nameof(changedProduct));
        }

        if(string.IsNullOrWhiteSpace(changedProduct.Name)){
            throw new BadRequestException(message: "Product must have a name!", nameof(changedProduct.Name));
        }

        if(changedProduct.Price <= 0){
            throw new BadRequestException(message: "Invalid price!", nameof(changedProduct.Price));
        }

        var isOk = productsRepository.Update(id, changedProduct);
        if(!isOk){
            throw new BadRequestException(message: "Product not updated!", nameof(isOk));
        }
    }

    public List<Product> GetAllProducts()
    {
        var products = productsRepository.GetAll();
        if(products == null){
            throw new NotFoundException(message: "Products not found!");
        }
        return products;
    }

    public Product GetProductById(int id)
    {
        var prod = productsRepository.GetById(id);

        if(prod == null){
            throw new NotFoundException(message: "Product not found!");
        }
        
        return prod;
    }

    public void RemoveProduct(int id)
    {
        if(id <= 0){
            throw new BadRequestException(message: "Invalid id!", nameof(id));
        }
        productsRepository.Delete(id);
    }
}