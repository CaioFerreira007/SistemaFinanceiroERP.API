namespace SistemaFinanceiroERP.Application.DTOs.Usuario
{
    public class UsuarioUpdateDto
    {
        public int Id { get; set; }
        public string UsuarioNome { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Senha { get; set; } = string.Empty;
        public string Telefone { get; set; } = string.Empty;

    }
}
