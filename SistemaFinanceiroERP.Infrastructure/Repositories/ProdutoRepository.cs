using SistemaFinanceiroERP.Domain.Interfaces;
using SistemaFinanceiroERP.Domain.Entities;
using SistemaFinanceiroERP.Infrastructure.Data;

namespace SistemaFinanceiroERP.Infrastructure.Repositories
{
    public class ProdutoRepository : Repository<Produto>, IProdutoRepository
    {
        public ProdutoRepository(AppDbContext context) : base(context)
        {
        }
    }
}