namespace SistemaFinanceiroERP.API.DTOs.Empresa
{
    public class EmpresaUpdateDto
    {
        public int Id { get; set; }
        public string NomeEmpresa { get; set; } = string.Empty;
        public string RazaoSocial { get; set; } = string.Empty;
        public string CNPJ { get; set; } = string.Empty;
        public string Telefone { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
   
    }
}
