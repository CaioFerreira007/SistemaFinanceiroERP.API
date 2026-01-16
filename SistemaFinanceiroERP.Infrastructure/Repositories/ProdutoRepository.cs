using Microsoft.EntityFrameworkCore;
using SistemaFinanceiroERP.Domain.Entities;
using SistemaFinanceiroERP.Domain.Interfaces;
using SistemaFinanceiroERP.Infrastructure.Data;

namespace SistemaFinanceiroERP.Infrastructure.Repositories
{
    public class ProdutoRepository : IProdutoRepository
    {
        private readonly AppDbContext _context;
        private readonly ITenantProvider _tenantProvider;

        public ProdutoRepository(AppDbContext context, ITenantProvider tenantProvider)
        {
            _context = context;
            _tenantProvider = tenantProvider;
        }

        public async Task AddAsync(Produto produto)
        {
            _context.Produtos.Add(produto);
            await Task.CompletedTask;
        }

        public async Task UpdateAsync(Produto produto)
        {
            _context.Produtos.Update(produto);
            await Task.CompletedTask;
        }

        public async Task<IEnumerable<Produto>> GetAllAsync()
        {
            return await _context.Produtos
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<Produto?> GetByIdAsync(int id)
        {
            return await _context.Produtos
                .FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task<bool> HasMovimentacoesAsync(int produtoId)
        {
            var empresaId = _tenantProvider.GetEmpresaId();

            return await _context.MovimentacoesEstoque
                .IgnoreQueryFilters()
                .AnyAsync(m => m.EmpresaId == empresaId && m.ProdutoId == produtoId);
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
