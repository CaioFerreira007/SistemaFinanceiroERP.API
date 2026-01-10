namespace SistemaFinanceiroERP.Domain.Interfaces
{
    public interface ITenantProvider
    {
        int GetEmpresaId();
        int GetUsuarioId();
    }
}