using SistemaFinanceiroERP.Domain.Entities;

namespace SistemaFinanceiroERP.Application.DTOs.LocalEstoque
{
   public  class LocalEstoqueResponseDto
    {
        public int Id { get; set; }
        public string LocalNome { get; set; } = string.Empty;
        public string Descricao { get; set; } = string.Empty;
        public string Estado { get; set; } = string.Empty;
        public string Cidade { get; set; } = string.Empty;
        public string Bairro { get; set; } = string.Empty;
        public string Rua { get; set; } = string.Empty;
        public DateTime DataCriacao { get; set; }
        public DateTime? DataAtualizacao { get; set; }
        public bool Ativo { get; set; }

    }
}
