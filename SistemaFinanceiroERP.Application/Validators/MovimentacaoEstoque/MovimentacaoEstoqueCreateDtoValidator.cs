using FluentValidation;
using SistemaFinanceiroERP.Application.DTOs.MovimentacaoEstoque;

namespace SistemaFinanceiroERP.Application.Validators.MovimentacaoEstoque
{
    public class MovimentacaoEstoqueCreateDtoValidator: AbstractValidator<MovimentacaoEstoqueCreateDto>
    {
        public MovimentacaoEstoqueCreateDtoValidator()
        {
            RuleFor(x => x.ProdutoId)
                .GreaterThan(0).WithMessage("O ID do produto deve ser maior que zero.");
            RuleFor(x => x.LocalEstoqueId)
                .GreaterThan(0).WithMessage("O ID do local de estoque deve ser maior que zero.");
            RuleFor(x => x.TipoMovimentacao)
                .IsInEnum().WithMessage("O tipo de movimentação é inválido.");
            RuleFor(x => x.DataMovimentacao)
                .NotEmpty().WithMessage("A data da movimentação é obrigatória.")
                .LessThanOrEqualTo(DateTime.UtcNow).WithMessage("A data da movimentação não pode ser no futuro.");
            RuleFor(x => x.Observacao)
                .MaximumLength(500).WithMessage("A observação não pode exceder 500 caracteres.");

        }
    }
}
