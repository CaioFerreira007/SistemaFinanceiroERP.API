using FluentValidation;
using SistemaFinanceiroERP.Application.DTOs.AjusteEstoque;


namespace SistemaFinanceiroERP.Application.Validators.AjusteEstoque
{
    public class AjusteEstoqueCreateDtoValidator : AbstractValidator<AjusteEstoqueCreateDto>
    {
        public AjusteEstoqueCreateDtoValidator()
        {
            RuleFor(x => x.ProdutoId)
                .GreaterThan(0).WithMessage("O id não pode ser igual ou menor que zero");
            RuleFor(x => x.LocalEstoqueId)
                .GreaterThan(0).WithMessage("O id do local de estoque não pode ser igual ou menor que zero");
            // Permite zero para casos de inventário que encontrou estoque zerado
            RuleFor(x => x.QuantidadeNova)
                .Must(x => x >= 0).WithMessage("A quantidade nova de estoque não pode ser negativa");
            RuleFor(x => x.Observacao)
                .NotEmpty().WithMessage("Observação é obrigatória")
                .MaximumLength(500)
                .WithMessage("A observação não pode exceder 500 caracteres");
                

        }
    }
}
