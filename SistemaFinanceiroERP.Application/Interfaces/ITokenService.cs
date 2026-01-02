using SistemaFinanceiroERP.Domain.Entities;


namespace SistemaFinanceiroERP.Application.Interfaces
{
    public interface ITokenService
    {

        string GerarToken(Usuario usuario);

    }
}
