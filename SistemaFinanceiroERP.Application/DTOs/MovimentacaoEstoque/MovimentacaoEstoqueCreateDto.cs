using SistemaFinanceiroERP.Domain.Enums;

namespace SistemaFinanceiroERP.Application.DTOs.MovimentacaoEstoque
{
   public class MovimentacaoEstoqueCreateDto
    {
        public int ProdutoId { get; set; }
        public int LocalEstoqueId { get; set; }
        public TipoMovimentacao TipoMovimentacao { get; set; }
        public DateTime DataMovimentacao { get; set; }
        public decimal Quantidade { get; set; }
        public string? Observacao { get; set; }
    }
}
