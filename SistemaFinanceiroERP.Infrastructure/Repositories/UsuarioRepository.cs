using SistemaFinanceiroERP.Domain.Interfaces;
using SistemaFinanceiroERP.Domain.Entities;
using SistemaFinanceiroERP.Infrastructure.Data;

namespace SistemaFinanceiroERP.Infrastructure.Repositories
{
    public class UsuarioRepository : Repository<Usuario>, IUsuarioRepository
    {
        public UsuarioRepository(AppDbContext context) : base(context)
        {
        }
    }
}