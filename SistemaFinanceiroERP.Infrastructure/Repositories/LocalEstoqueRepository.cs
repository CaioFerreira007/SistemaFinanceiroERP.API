using SistemaFinanceiroERP.Domain.Interfaces;
using SistemaFinanceiroERP.Domain.Entities;
using SistemaFinanceiroERP.Infrastructure.Data;


namespace SistemaFinanceiroERP.Infrastructure.Repositories
{
    public class LocalEstoqueRepository :Repository<LocalEstoque>, ILocalEstoqueRepository
    {
        public LocalEstoqueRepository(AppDbContext context) : base(context)
        {

        }
    }
}
