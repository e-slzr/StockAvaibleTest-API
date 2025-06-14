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
        }
    }

    public class UpdateBoxValidator : AbstractValidator<UpdateBoxDTO>
    {
        public UpdateBoxValidator()
        {
            RuleFor(x => x.Location)
                .NotEmpty().WithMessage("La ubicación es requerida")
                .MaximumLength(255).WithMessage("La ubicación no puede exceder los 255 caracteres");
        }
    }
}
