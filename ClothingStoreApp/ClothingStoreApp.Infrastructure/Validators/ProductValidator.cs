using ClothingStoreApp.Core.Models;
using FluentValidation;

namespace ClothingStoreApp.Infrastructure.Validators;

public class ProductValidator : AbstractValidator<Product>
{
    private const int name_max_length = 30;
    public ProductValidator()
    {
        base.RuleFor((prod) => prod.Name)
            .NotEmpty()
                .WithMessage("Product name cannot be empty!")
            .MaximumLength(name_max_length)
                .WithMessage($"Maximum length for product name is {name_max_length} characters!");

        base.RuleFor((prod) => prod.Price)
            .GreaterThanOrEqualTo(0)
                .WithMessage("Product price must be greater than zero!");
    }
}