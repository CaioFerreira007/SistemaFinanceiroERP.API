using Microsoft.AspNetCore.Http;
using SistemaFinanceiroERP.Domain.Interfaces;
using System.Security.Claims;

namespace SistemaFinanceiroERP.Infrastructure.Security
{
    public class TenantProvider : ITenantProvider
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public TenantProvider(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public int GetEmpresaId()
        {
            var user = _httpContextAccessor.HttpContext?.User;

            if (user == null || user.Identity?.IsAuthenticated != true)
                throw new UnauthorizedAccessException("Usuário não autenticado.");

            var empresaIdStr = user.FindFirst("EmpresaId")?.Value;

            if (string.IsNullOrWhiteSpace(empresaIdStr) || !int.TryParse(empresaIdStr, out var empresaId))
                throw new UnauthorizedAccessException("Claim EmpresaId inválida no token.");

            return empresaId;
        }

        public int GetUsuarioId()
        {
            var user = _httpContextAccessor.HttpContext?.User;

            if (user == null || user.Identity?.IsAuthenticated != true)
                throw new UnauthorizedAccessException("Usuário não autenticado.");

         
            var userIdStr =
                user.FindFirst("sub")?.Value
                ?? user.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrWhiteSpace(userIdStr) || !int.TryParse(userIdStr, out var userId))
                throw new UnauthorizedAccessException("Id do usuário inválido no token.");

            return userId;
        }
    }
}
