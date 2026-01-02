namespace SistemaFinanceiroERP.API.DTOs.Auth
{
    public class TokenDto
    {

        public string Token { get; set; } = string.Empty;
        public int ExpiresIn { get; set; }
        public int UsuarioId { get; set; }
        public string EmailUsuario { get; set; } = string.Empty;
        public string UsuarioNome { get; set; } = string.Empty;
        public int EmpresaId { get; set; }  

    }
}
