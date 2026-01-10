
namespace SistemaFinanceiroERP.Application.DTOs.MovimentacaoEstoque
{
    public class MovimentacaoEstoqueResponseDto
    {
        public int Id { get; set; }
        public int ProdutoId { get; set; }
        public int LocalEstoqueId { get; set; }
        public string NomeProduto { get; set; } = string.Empty;
        public string NomeLocalEstoque { get; set; } = string.Empty;
        public string NomeUsuario { get; set; } = string.Empty;
        public string TipoMovimentacao { get; set; } = string.Empty;
        public decimal Quantidade { get; set; }
        public string? Observacao { get; set; }
        public DateTime DataMovimentacao { get; set; }
    }
}
