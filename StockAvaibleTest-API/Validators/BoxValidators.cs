using FluentValidation;
using StockAvaibleTest_API.DTOs;

namespace StockAvaibleTest_API.Validators
{
    public class BoxValidator : AbstractValidator<CreateBoxDTO>
    {
        public BoxValidator()
        {
            RuleFor(x => x.Code)
                .NotEmpty().WithMessage("El código es requerido")
                .MaximumLength(50).WithMessage("El código no puede exceder los 50 caracteres");

            RuleFor(x => x.Location)
                .NotEmpty().WithMessage("La ubicación es requerida")
                .MaximumLength(255).WithMessage("La ubicación no puede exceder los 255 caracteres");

            RuleFor(x => x.TotalCapacity)
                .GreaterThan(0).WithMessage("La capacidad total debe ser mayor a 0")
                .NotEmpty().WithMessage("La capacidad total es requerida");
        }
    }

    public class UpdateBoxValidator : AbstractValidator<UpdateBoxDTO>
    {
        public UpdateBoxValidator()
        {
            RuleFor(x => x.Location)
                .NotEmpty().WithMessage("La ubicación es requerida")
                .MaximumLength(255).WithMessage("La ubicación no puede exceder los 255 caracteres");

            When(x => x.TotalCapacity.HasValue, () => {
                RuleFor(x => x.TotalCapacity)
                    .GreaterThan(0).WithMessage("La capacidad total debe ser mayor a 0");
            });
        }
    }
}
