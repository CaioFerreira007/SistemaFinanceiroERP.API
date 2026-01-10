using FluentValidation;
using SistemaFinanceiroERP.Application.DTOs.Produto;
namespace SistemaFinanceiroERP.Application.Validators.Produto
{
    public class ProdutoUpdateDtoValidator: AbstractValidator<ProdutoUpdateDto>
    {


        public ProdutoUpdateDtoValidator()
        {
            RuleFor(x => x.Id)
                .GreaterThan(0).WithMessage("O ID do produto deve ser maior que zero.");
            RuleFor(x => x.ProdutoNome)
                .NotEmpty().WithMessage("O nome do produto é obrigatório.")
                .MaximumLength(100).WithMessage("O nome do produto não pode exceder 100 caracteres.");
            RuleFor(x => x.Categoria)
                .NotEmpty().WithMessage("A categoria do produto é obrigatória.")
                .MaximumLength(50).WithMessage("A categoria do produto não pode exceder 50 caracteres.");
            RuleFor(x => x.Descricao)
                .MaximumLength(500).WithMessage("A descrição do produto não pode exceder 500 caracteres.");
            RuleFor(x => x.PrecoUnitario)
                .GreaterThan(0).WithMessage("O preço unitário deve ser maior que zero.");
            RuleFor(x => x.CodigoBarras)
                .NotEmpty().WithMessage("O código de barras é obrigatório.")
                .MaximumLength(50).WithMessage("O código de barras não pode exceder 50 caracteres.");
            RuleFor(x => x.QuantidadeEstoque)
                .GreaterThanOrEqualTo(0).WithMessage("A quantidade em estoque não pode ser negativa.");
            RuleFor(x => x.UnidadeMedida)
                .NotEmpty().WithMessage("A unidade de medida é obrigatória.")
                .MaximumLength(20).WithMessage("A unidade de medida não pode exceder 20 caracteres.");
                
        }


    }
}
