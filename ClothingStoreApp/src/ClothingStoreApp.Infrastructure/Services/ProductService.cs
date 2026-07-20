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

    public void AddProduct(Product product)
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
            throw new NotFoundException(message: "Product not updated!");
        }
    }

    public List<Product> GetAllProducts(string? search = null)
    {
        var products = productsRepository.GetAll() ?? throw new NotFoundException(message: "Products not found!");

        if (!string.IsNullOrWhiteSpace(search))
        {
            search = search.Trim().ToLower();

            products = products.Where(p =>
                p.Name!.ToLower().Contains(search) ||
                p.Description!.ToLower().Contains(search)).ToList();
        }

        return products;
    }

    public Product GetProductById(int id)
    {
        var prod = productsRepository.GetById(id) ?? throw new NotFoundException(message: "Product not found!");
        return prod;
    }

    public void RemoveProduct(int id)
    {
        if(id <= 0){
            throw new BadRequestException(message: "Invalid id!", nameof(id));
        }
        var product = productsRepository.GetById(id) ?? throw new NotFoundException(message: "Product not found!");
        productsRepository.Delete(id);
    }
}