namespace SistemaFinanceiroERP.API.DTOs.Produto
{
    public class ProdutoUpdateDto
    {
        public int Id { get; set; }
        public string ProdutoNome { get; set; } = string.Empty;
        public string Categoria { get; set; } = string.Empty;
        public string Descricao { get; set; } = string.Empty;
        public decimal PrecoUnitario { get; set; }
        public string CodigoBarras { get; set; } = string.Empty;
        public int QuantidadeEstoque { get; set; }
        public string UnidadeMedida { get; set; } = string.Empty;
    }
}
