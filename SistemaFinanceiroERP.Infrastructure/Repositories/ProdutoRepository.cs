using Microsoft.EntityFrameworkCore;
using SistemaFinanceiroERP.Domain.Entities;
using SistemaFinanceiroERP.Domain.Interfaces;
using SistemaFinanceiroERP.Infrastructure.Data;

namespace SistemaFinanceiroERP.Infrastructure.Repositories
{
    public class ProdutoRepository : Repository<Produto>, IProdutoRepository
    {
        public ProdutoRepository(AppDbContext context) : base(context)
        {
        }

        public new async Task<IEnumerable<Produto>> GetAllAsync()
        {
            return await _context.Produtos
                .Include(p => p.LocalEstoque)
                .ToListAsync();
        }
        public new async Task<Produto?> GetByIdAsync(int id)
        {
            return await _context.Produtos
                .Include(p => p.LocalEstoque)
                .FirstOrDefaultAsync(p => p.Id == id);
        }
    }
}