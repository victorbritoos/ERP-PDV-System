namespace ERP.Application.Validators.PriceTable;

using FluentValidation;
using ERP.Application.DTOs.PriceTable;

public class CreatePriceTableValidator : AbstractValidator<CreatePriceTableDto>
{
    public CreatePriceTableValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Nome da tabela é obrigatório")
            .Length(3, 100).WithMessage("Nome deve ter entre 3 e 100 caracteres");

        RuleFor(x => x.Priority)
            .GreaterThanOrEqualTo(0).WithMessage("Prioridade deve ser maior ou igual a 0");
    }
}

public class UpdatePriceTableValidator : AbstractValidator<UpdatePriceTableDto>
{
    public UpdatePriceTableValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Nome da tabela é obrigatório")
            .Length(3, 100).WithMessage("Nome deve ter entre 3 e 100 caracteres");

        RuleFor(x => x.Priority)
            .GreaterThanOrEqualTo(0).WithMessage("Prioridade deve ser maior ou igual a 0");
    }
}
