using FluentValidation;
using StockAvaibleTest_API.DTOs;

namespace StockAvaibleTest_API.Validators
{
    public class TransactionValidator : AbstractValidator<CreateTransactionDTO>
    {
        public TransactionValidator()
        {
            RuleFor(x => x.BoxId)
                .GreaterThan(0).WithMessage("La caja es requerida");

            RuleFor(x => x.ProductId)
                .GreaterThan(0).WithMessage("El producto es requerido");

            RuleFor(x => x.Quantity)
                .GreaterThan(0).WithMessage("La cantidad debe ser mayor a 0");

            RuleFor(x => x.Type)
                .NotEmpty().WithMessage("El tipo de transacción es requerido")
                .Must(type => type == "IN" || type == "OUT")
                .WithMessage("El tipo de transacción debe ser 'IN' o 'OUT'");
        }
    }
}
