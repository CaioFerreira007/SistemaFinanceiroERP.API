namespace SistemaFinanceiroERP.Application.DTOs.LocalEstoque
{
    public class LocalEstoqueCreateDto
    {
        public string LocalNome { get; set; } = string.Empty;
        public string Descricao { get; set; } = string.Empty;
        public string Estado { get; set; } = string.Empty;
        public string Cidade { get; set; } = string.Empty;
        public string Bairro { get; set; } = string.Empty;
        public string Rua { get; set; } = string.Empty;
    }
}
