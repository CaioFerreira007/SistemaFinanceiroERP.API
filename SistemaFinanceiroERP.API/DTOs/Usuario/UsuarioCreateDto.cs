namespace SistemaFinanceiroERP.API.DTOs.Usuario
{
    public class UsuarioCreateDto
    {
        public string UsuarioNome { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Senha { get; set; } = string.Empty;
        public string Telefone { get; set; } = string.Empty;
        public int EmpresaId { get; set; }
    }
}
