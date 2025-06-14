using FluentValidation;
using StockAvaibleTest_API.DTOs;

namespace StockAvaibleTest_API.Validators
{
    public class CategoryValidator : AbstractValidator<CreateCategoryDTO>
    {
        public CategoryValidator()
        {
            RuleFor(x => x.Code)
                .NotEmpty().WithMessage("El código es requerido")
                .MaximumLength(50).WithMessage("El código no puede exceder los 50 caracteres");

            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("El nombre es requerido")
                .MaximumLength(100).WithMessage("El nombre no puede exceder los 100 caracteres");

            RuleFor(x => x.Description)
                .MaximumLength(255).WithMessage("La descripción no puede exceder los 255 caracteres");
        }
    }

    public class UpdateCategoryValidator : AbstractValidator<UpdateCategoryDTO>
    {
        public UpdateCategoryValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("El nombre es requerido")
                .MaximumLength(100).WithMessage("El nombre no puede exceder los 100 caracteres");

            RuleFor(x => x.Description)
                .MaximumLength(255).WithMessage("La descripción no puede exceder los 255 caracteres");
        }
    }
}
