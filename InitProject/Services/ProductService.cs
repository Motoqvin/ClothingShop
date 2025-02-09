using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using InitProject.Exceptions;
using InitProject.Models;
using InitProject.Repositories.Base;
using InitProject.Services.Base;

namespace InitProject.Services;
public class ProductService : IProductService
{
    private readonly IProductsRepository productsRepository;

    public ProductService(IProductsRepository productsRepository)
    {
        this.productsRepository = productsRepository;
    }

    public void AddProduct(Product product)
    {
        var validationException = new ValidationException();

        if(string.IsNullOrWhiteSpace(product.Name)){
            validationException.validationResponses.Add(new ValidationResponse{
                Property = nameof(product.Name),
                Message = $"{nameof(product.Name)} can't be empty!"
            });
        }

        if(string.IsNullOrWhiteSpace(product.Description)){
            validationException.validationResponses.Add(new ValidationResponse{
                Property = nameof(product.Description),
                Message = $"{nameof(product.Description)} can't be empty!"
            });
        }

        if(product.Id <= 0){
            validationException.validationResponses.Add(new ValidationResponse{
                Property = nameof(product.Id),
                Message = $"{nameof(product.Id)} can't be less or equal to 0!"
            });
        }

        if(product.Price < 0){
            validationException.validationResponses.Add(new ValidationResponse{
                Property = nameof(product.Price),
                Message = $"{nameof(product.Price)} can't be less than 0!"
            });
        }

        if(validationException.validationResponses.Any() == false){
            if(productsRepository.GetProductById(product.Id) != null){
                validationException.validationResponses.Add(new ValidationResponse{
                    Property = nameof(product.Id),
                    Message = $"Product with id {product.Id} already exists!"
                });
            }
        }

        if(validationException.validationResponses.Any()){
            throw validationException;
        }

        productsRepository.CreateProduct(product);
    }

    public void ChangeProduct(int id, Product changedProduct)
    {
        var validationException = new ValidationException();

        if(string.IsNullOrWhiteSpace(changedProduct.Description)){
            validationException.validationResponses.Add(new ValidationResponse{
                Property = nameof(changedProduct.Description),
                Message = "Description of the product can't be empty!"
            });
        }

        if(string.IsNullOrWhiteSpace(changedProduct.Name)){
            validationException.validationResponses.Add(new ValidationResponse{
                Property = nameof(changedProduct.Name),
                Message = "Name of the product can't be empty!"
            });
        }

        if(changedProduct.Price < 0){
            validationException.validationResponses.Add(new ValidationResponse{
                Property = nameof(changedProduct.Price),
                Message = "Price of the product can't be less than 0!"
            });
        }

        if(id <= 0){
            validationException.validationResponses.Add(new ValidationResponse{
                Property = nameof(id),
                Message = $"Product Id can't be less or equal to 0!"
            });
        }

        var isOk = productsRepository.UpdateProduct(id, changedProduct);

        if(isOk == false){
            validationException.validationResponses.Add(new ValidationResponse{
                Property = nameof(id),
                Message = $"There is no such product with id {id}!"
            });
        }

        if(validationException.validationResponses.Any()){
            throw validationException;
        }
    }

    public Product? GetProductById(int id)
    {
        var validationException = new ValidationException();
        var prod = productsRepository.GetProductById(id);
        if(prod == null){
            validationException.validationResponses.Add(new ValidationResponse{
                Property = nameof(id),
                Message = $"Product with id {id} doesn't exist!"
            });
            throw validationException;
        }
        return prod;
    }

    public void RemoveProduct(int id)
    {
        var validationException = new ValidationException();

        if(id <= 0){
            validationException.validationResponses.Add(new ValidationResponse{
                Property = nameof(id),
                Message = $"{nameof(id)} can't be less or equal to 0!"
            });
        }

        if(this.GetProductById(id) == null){
            validationException.validationResponses.Add(new ValidationResponse{
                Property = nameof(id),
                Message = $"Product with id {id} doesn't exist"
            });
        }

        if (validationException.validationResponses.Any()){
            throw validationException;
        }

        productsRepository.DeleteProduct(id);
    }
}