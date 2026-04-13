// Api/Validators/CreateOrderRequestValidator.cs
using FluentValidation;
using OrderService.Core.DTOs;

namespace OrderService.Api.Validators;

public class CreateOrderRequestValidator : AbstractValidator<CreateOrderRequest>
{
    public CreateOrderRequestValidator()
    {
        RuleFor(x => x.Customer)
            .NotEmpty().WithMessage("Customer is required.")
            .MaximumLength(100).WithMessage("Customer must not exceed 100 characters.");

        RuleFor(x => x.Total)
            .GreaterThan(0).WithMessage("Total must be greater than zero.");
    }
}
