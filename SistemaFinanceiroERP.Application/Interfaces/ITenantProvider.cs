namespace SistemaFinanceiroERP.Application.Interfaces
{
    public interface ITenantProvider
    {
        int GetEmpresaId();
        int GetUsuarioId();
    }
}