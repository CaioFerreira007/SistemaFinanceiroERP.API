using SistemaFinanceiroERP.Domain.Enums;

namespace SistemaFinanceiroERP.Application.DTOs.Empresa
{
    public class EmpresaResponseDto
    {
        public int Id { get; set; }
        public string NomeEmpresa { get; set; } = string.Empty;
        public string RazaoSocial { get; set; } = string.Empty;
        public string Cnpj { get; set; } = string.Empty;
        public string Telefone { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string TipoEmpresa { get; set; } = string.Empty;

        public DateTime DataCriacao { get; set; }
        public DateTime? DataAtualizacao { get; set; }
        public bool Ativo { get; set; }

    }
}
