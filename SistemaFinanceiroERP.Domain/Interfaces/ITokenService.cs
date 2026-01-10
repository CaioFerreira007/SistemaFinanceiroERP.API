using SistemaFinanceiroERP.Domain.Entities;


namespace SistemaFinanceiroERP.Domain.Interfaces
{
    public interface ITokenService
    {

        string GerarToken(Usuario usuario);

    }
}
