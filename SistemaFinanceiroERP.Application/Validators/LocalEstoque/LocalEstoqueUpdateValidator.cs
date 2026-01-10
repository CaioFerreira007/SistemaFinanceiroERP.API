using FluentValidation;
using SistemaFinanceiroERP.Application.DTOs.LocalEstoque;

namespace SistemaFinanceiroERP.Application.Validators.LocalEstoque
{
    public class LocalEstoqueUpdateValidator: AbstractValidator<LocalEstoqueUpdateDto>
    {
        public LocalEstoqueUpdateValidator()
        {

            RuleFor(x => x.Id)
                .GreaterThan(0).WithMessage("O ID do local de estoque deve ser maior que zero.");

            RuleFor(x => x.LocalNome)
                .NotEmpty().WithMessage("O nome do local de estoque é obrigatório.")
                .MaximumLength(100).WithMessage("O nome do local de estoque não pode exceder 100 caracteres.");

            RuleFor(x => x.Descricao)
                .MaximumLength(500).WithMessage("A descrição do local de estoque não pode exceder 500 caracteres.");

            RuleFor(x => x.Estado)
                .NotEmpty().WithMessage("O nome do Estado de estoque é obrigatório.")
                .MaximumLength(100).WithMessage("O nome do Estado de estoque não pode exceder 100 caracteres.");

            RuleFor(x => x.Cidade)
                .NotEmpty().WithMessage("O nome da Cidade de estoque é obrigatório.")
                .MaximumLength(100).WithMessage("O nome da Cidade de estoque não pode exceder 100 caracteres.");

            RuleFor(x => x.Bairro)
                .NotEmpty().WithMessage("O bairro do local de estoque é obrigatório.")
                .MaximumLength(100).WithMessage("O bairro do local de estoque não pode exceder 100 caracteres.");

            RuleFor(x => x.Rua)
                .NotEmpty().WithMessage("A rua do local de estoque é obrigatória.")
                .MaximumLength(100).WithMessage("A rua do local de estoque não pode exceder 100 caracteres.");

        }
    }
}
