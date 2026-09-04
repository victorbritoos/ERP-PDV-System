namespace ERP.Application.Validators.Product;

using FluentValidation;
using ERP.Application.DTOs.Product;

public class CreateProductValidator : AbstractValidator<CreateProductDto>
{
    public CreateProductValidator()
    {
        RuleFor(x => x.Sku)
            .NotEmpty().WithMessage("SKU é obrigatório")
            .Length(3, 50).WithMessage("SKU deve ter entre 3 e 50 caracteres")
            .Matches("^[A-Za-z0-9-]*$").WithMessage("SKU deve conter apenas letras, números e hífens");

        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Nome é obrigatório")
            .Length(3, 200).WithMessage("Nome deve ter entre 3 e 200 caracteres");

        RuleFor(x => x.CategoryId)
            .NotEmpty().WithMessage("Categoria é obrigatória");

        RuleFor(x => x.CostPrice)
            .GreaterThan(0).WithMessage("Preço de custo deve ser maior que 0");

        RuleFor(x => x.StockMinimum)
            .GreaterThanOrEqualTo(0).WithMessage("Estoque mínimo não pode ser negativo");

        RuleFor(x => x.StockMaximum)
            .GreaterThan(x => x.StockMinimum).WithMessage("Estoque máximo deve ser maior que o mínimo");
    }
}

public class UpdateProductValidator : AbstractValidator<UpdateProductDto>
{
    public UpdateProductValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Nome é obrigatório")
            .Length(3, 200).WithMessage("Nome deve ter entre 3 e 200 caracteres");

        RuleFor(x => x.CategoryId)
            .NotEmpty().WithMessage("Categoria é obrigatória");

        RuleFor(x => x.CostPrice)
            .GreaterThan(0).WithMessage("Preço de custo deve ser maior que 0");

        RuleFor(x => x.StockMaximum)
            .GreaterThan(x => x.StockMinimum).WithMessage("Estoque máximo deve ser maior que o mínimo");
    }
}
