namespace ERP.Application.Validators.Customer;

using FluentValidation;
using ERP.Application.DTOs.Customer;

public class CreateCustomerValidator : AbstractValidator<CreateCustomerDto>
{
    public CreateCustomerValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Nome do cliente é obrigatório")
            .Length(3, 200).WithMessage("Nome deve ter entre 3 e 200 caracteres");

        RuleFor(x => x.Email)
            .EmailAddress().WithMessage("Email inválido")
            .When(x => !string.IsNullOrEmpty(x.Email));

        RuleFor(x => x.CreditLimit)
            .GreaterThanOrEqualTo(0).WithMessage("Limite de crédito não pode ser negativo");
    }
}

public class UpdateCustomerValidator : AbstractValidator<UpdateCustomerDto>
{
    public UpdateCustomerValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Nome do cliente é obrigatório")
            .Length(3, 200).WithMessage("Nome deve ter entre 3 e 200 caracteres");

        RuleFor(x => x.Email)
            .EmailAddress().WithMessage("Email inválido")
            .When(x => !string.IsNullOrEmpty(x.Email));

        RuleFor(x => x.CreditLimit)
            .GreaterThanOrEqualTo(0).WithMessage("Limite de crédito não pode ser negativo");
    }
}
