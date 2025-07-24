using ClothingStoreApp.Core.Models;
using FluentValidation;

namespace ClothingStoreApp.Presentation.Validators;

public class OrderValidator : AbstractValidator<Order>
{
    public OrderValidator()
    {
        base.RuleFor((order) => order.TotalPrice)
            .GreaterThanOrEqualTo(0)
                .WithMessage("Product price must be greater than zero!");
    }
}