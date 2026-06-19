using CRN_Technical_Assessment.Application.DTOs;
using FluentValidation;

namespace CRN_Technical_Assessment.Application.Validators
{
    public class UpdateProductDtoValidator : AbstractValidator<UpdateProductDto>
    {
        public UpdateProductDtoValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Product name is required.")
                .Length(1, 255).WithMessage("Product name must be between 1 and 255 characters.");

            RuleFor(x => x.Description)
                .NotEmpty().WithMessage("Product description is required.")
                .Length(1, 1000).WithMessage("Product description must be between 1 and 1000 characters.");

            RuleFor(x => x.Price)
                .GreaterThan(0).WithMessage("Product price must be greater than 0.")
                .LessThanOrEqualTo(999999.99m).WithMessage("Product price cannot exceed 999,999.99.");

            RuleFor(x => x.StockQuantity)
                .GreaterThanOrEqualTo(0).WithMessage("Stock quantity cannot be negative.")
                .LessThanOrEqualTo(1000000).WithMessage("Stock quantity cannot exceed 1,000,000.");

            RuleFor(x => x.CategoryId)
                .GreaterThan(0).WithMessage("Category ID must be greater than 0.");

            RuleFor(x => x.Status)
                .IsInEnum().WithMessage("Invalid product status.");

            RuleFor(x => x.Condition)
                .IsInEnum().WithMessage("Invalid product condition.");

            RuleFor(x => x.RecordStatus)
                .IsInEnum().WithMessage("Invalid record status.");
        }
    }
}
