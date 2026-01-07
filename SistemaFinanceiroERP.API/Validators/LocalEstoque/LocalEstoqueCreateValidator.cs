using FluentValidation;
using SistemaFinanceiroERP.Application.DTOs.LocalEstoque;

namespace SistemaFinanceiroERP.API.Validators.LocalEstoque
{
    public class LocalEstoqueCreateValidator: AbstractValidator<LocalEstoqueCreateDto>
    {
        public LocalEstoqueCreateValidator()
        {
            RuleFor(x => x.LocalNome)
                .NotEmpty().WithMessage("O nome do local de estoque é obrigatório.")
                .MaximumLength(100).WithMessage("O nome do local de estoque não pode exceder 100 caracteres.");

            RuleFor(x => x.Descricao)
                .NotEmpty().WithMessage("A descrição do local de estoque é obrigatória.")
                .MaximumLength(500).WithMessage("A descrição do local de estoque não pode exceder 500 caracteres.");



        }
    }
}
