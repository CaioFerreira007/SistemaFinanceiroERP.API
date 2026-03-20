using FluentValidation;
using SistemaFinanceiroERP.Application.DTOs.Transacao;
namespace SistemaFinanceiroERP.Application.Validators.Transacao
{
    public class TransacaoCreateDtoValidator : AbstractValidator<TransacaoCreateDto>
    {
        public TransacaoCreateDtoValidator()
        {
            RuleFor(x => x.EmpresaCompradoraId)
                .NotEmpty().WithMessage("O ID da empresa compradora é obrigatório.");
            RuleFor(x => x.EmpresaCompradoraId)
                .NotEqual(x => x.EmpresaVendedoraId).WithMessage("A empresa compradora não pode ser a mesma que a vendedora.");
            RuleFor(x => x.EmpresaVendedoraId)
                .NotEmpty().WithMessage("O ID da empresa vendedora é obrigatório.");
            RuleFor(x => x.ItemsTransacao)
                .NotEmpty().WithMessage("A transação deve conter pelo menos um item.");
            RuleFor(x => x.Desconto)
                .GreaterThanOrEqualTo(0).WithMessage("O desconto não pode ser negativo.");
        }
    }
}
