using SistemaFinanceiroERP.Domain.Enums;


namespace SistemaFinanceiroERP.Domain.Entities
{
    public class Empresa : BaseEntity
    {
        public string Cnpj { get; set; } = string.Empty;
        public string RazaoSocial { get; set; } = string.Empty;
        public string NomeEmpresa { get; set; } = string.Empty;
        public string Telefone { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public TipoEmpresa Tipo { get; set; }

        public ICollection<Usuario> Usuarios { get; set; } = new List<Usuario>();
        public ICollection<Produto> Produtos { get; set; } = new List<Produto>();



    }
}
