using FluentValidation;
using LePickaProducts.Application.Commands.Products;

namespace LePickaProducts.Application.Validators
{
    public class AddProductCommandValidator : AbstractValidator<AddProductCommand>
    {
        public AddProductCommandValidator()
        {
            RuleFor(x => x.Name).NotEmpty().WithMessage("Nazwa produktu nie może być pusta.");
            RuleFor(x => x.Category).NotEmpty().WithMessage("Kategoria nie może być pusta.");
            RuleFor(x => x.Price)
                .NotEmpty().WithMessage("Cena nie może być pusta.")
                .GreaterThan(0).WithMessage("Cena musi być większa niż 0.");
        }
    }
}
