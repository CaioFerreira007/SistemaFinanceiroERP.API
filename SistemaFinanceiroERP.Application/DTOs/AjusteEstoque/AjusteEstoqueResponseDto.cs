
namespace SistemaFinanceiroERP.Application.DTOs.AjusteEstoque
{
    public class AjusteEstoqueResponseDto
    {
        public int Id { get; set; }
        public int ProdutoId { get; set; }
        public string ProdutoNome { get; set; } = string.Empty;
        public int LocalEstoqueId { get; set; }
        public string LocalEstoqueNome { get; set; } = string.Empty;
        public string UsuarioNome { get; set; } = string.Empty;
        public int UsuarioId { get; set; }
        public DateTime DataCriacao { get; set; }
        public DateTime? DataAtualizacao { get; set; }
        public bool Ativo { get; set; }
        public decimal QuantidadeAnterior { get; set; }
        public decimal QuantidadeNova { get; set; }
        public decimal Diferenca { get; set; }
        public string Observacao { get; set; } = string.Empty;
        public DateTime DataDoAjuste { get; set; }
    }
}
