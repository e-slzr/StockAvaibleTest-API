using FluentValidation;
using StockAvaibleTest_API.DTOs;

namespace StockAvaibleTest_API.Validators
{
    public class ProductValidator : AbstractValidator<CreateProductDTO>
    {
        public ProductValidator()
        {
            RuleFor(x => x.Code)
                .NotEmpty().WithMessage("El código es requerido")
                .MaximumLength(50).WithMessage("El código no puede exceder los 50 caracteres");

            RuleFor(x => x.Description)
                .NotEmpty().WithMessage("La descripción es requerida")
                .MaximumLength(255).WithMessage("La descripción no puede exceder los 255 caracteres");

            RuleFor(x => x.MinimumStock)
                .GreaterThanOrEqualTo(0).WithMessage("El stock mínimo no puede ser negativo");

            RuleFor(x => x.Unit)
                .NotEmpty().WithMessage("La unidad de medida es requerida")
                .MaximumLength(50).WithMessage("La unidad de medida no puede exceder los 50 caracteres");

            RuleFor(x => x.CategoryId)
                .GreaterThan(0).WithMessage("La categoría es requerida");
        }
    }

    public class UpdateProductValidator : AbstractValidator<UpdateProductDTO>
    {
        public UpdateProductValidator()
        {
            RuleFor(x => x.Description)
                .NotEmpty().WithMessage("La descripción es requerida")
                .MaximumLength(255).WithMessage("La descripción no puede exceder los 255 caracteres");

            RuleFor(x => x.MinimumStock)
                .GreaterThanOrEqualTo(0).WithMessage("El stock mínimo no puede ser negativo");

            RuleFor(x => x.Unit)
                .NotEmpty().WithMessage("La unidad de medida es requerida")
                .MaximumLength(50).WithMessage("La unidad de medida no puede exceder los 50 caracteres");

            RuleFor(x => x.CategoryId)
                .GreaterThan(0).WithMessage("La categoría es requerida");
        }
    }
}
