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
            var empresaIdClaim = _httpContextAccessor.HttpContext?.User
                .FindFirst("EmpresaId")?.Value;

            if (string.IsNullOrEmpty(empresaIdClaim))
            {
                throw new UnauthorizedAccessException("EmpresaId não encontrado no token.");
            }

            return int.Parse(empresaIdClaim);
        }

        public int GetUsuarioId()
        {
            var userIdClaim = _httpContextAccessor.HttpContext?.User
                .FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(userIdClaim))
            {
                throw new UnauthorizedAccessException("UsuarioId não encontrado no token.");
            }

            return int.Parse(userIdClaim);
        }
    }
}