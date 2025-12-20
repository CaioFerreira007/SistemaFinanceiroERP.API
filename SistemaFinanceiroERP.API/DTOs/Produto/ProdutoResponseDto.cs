namespace SistemaFinanceiroERP.API.DTOs.Produto
{
    public class ProdutoResponseDto
    {
        public int Id { get; set; }
        public string ProdutoNome { get; set; } = string.Empty;
        public string Categoria { get; set; } = string.Empty;
        public string Descricao { get; set; } = string.Empty;
        public decimal PrecoUnitario { get; set; }
        public string CodigoBarras { get; set; } = string.Empty;
        public int QuantidadeEstoque { get; set; }
        public int EmpresaId { get; set; }
        public string UnidadeMedida { get; set; } = string.Empty;
        public DateTime DataCriacao { get; set; }
        public DateTime? DataAtualizacao { get; set; }
        public bool Ativo { get; set; }
    }
}
