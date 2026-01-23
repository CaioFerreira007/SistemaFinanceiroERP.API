using FluentValidation;
using SistemaFinanceiroERP.Application.DTOs.ItemTransacao;
namespace SistemaFinanceiroERP.Application.Validators.ItemTransacao
{
    public class ItemTransacaoCreateDtoValidator: AbstractValidator<ItemTransacaoCreateDto>
    {
        public ItemTransacaoCreateDtoValidator()
        {
            RuleFor(x => x.ProdutoId)
                .GreaterThan(0).WithMessage("O ID do produto deve ser maior que zero.");
            RuleFor(x => x.Quantidade)
                .GreaterThan(0).WithMessage("A quantidade deve ser maior que zero.");
            RuleFor(x => x.PrecoUnitario)
                .GreaterThan(0).WithMessage("O preço unitário deve ser maior que zero.");
        }
    }
}
