using SistemaFinanceiroERP.Domain.Enums;

namespace SistemaFinanceiroERP.API.DTOs.Auth
{
    public class RegisterDto
    {

        //registro Empresa
        public string NomeEmpresa { get; set; } = string.Empty;
        public string RazaoSocial { get; set; } = string.Empty;
        public string Cnpj { get; set; } = string.Empty;
        public string TelefoneEmpresa { get; set; } = string.Empty;
        public string EmailEmpresa { get; set; } = string.Empty;
        public  TipoEmpresa Tipo{ get; set; }

        //registro Usuario
        public string NomeUsuario { get; set; } = string.Empty;
        public string EmailUsuario { get; set; } = string.Empty;
        public string Senha { get; set; } = string.Empty;
        public string TelefoneUsuario { get; set; } = string.Empty;
    }
}
