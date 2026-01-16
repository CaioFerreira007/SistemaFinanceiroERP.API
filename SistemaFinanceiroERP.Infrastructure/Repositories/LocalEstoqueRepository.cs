using Microsoft.EntityFrameworkCore;
using SistemaFinanceiroERP.Domain.Entities;
using SistemaFinanceiroERP.Domain.Interfaces;
using SistemaFinanceiroERP.Infrastructure.Data;

namespace SistemaFinanceiroERP.Infrastructure.Repositories
{
    public class LocalEstoqueRepository : ILocalEstoqueRepository
    {
        private readonly AppDbContext _context;
        private readonly ITenantProvider _tenantProvider;

        public LocalEstoqueRepository(AppDbContext context, ITenantProvider tenantProvider)
        {
            _context = context;
            _tenantProvider = tenantProvider;
        }

        public async Task AddAsync(LocalEstoque local)
        {
            _context.LocaisEstoque.Add(local);
            await Task.CompletedTask;
        }

        public async Task UpdateAsync(LocalEstoque local)
        {
            _context.LocaisEstoque.Update(local);
            await Task.CompletedTask;
        }

        public async Task<IEnumerable<LocalEstoque>> GetAllAsync()
        {
            return await _context.LocaisEstoque
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<LocalEstoque?> GetByIdAsync(int id)
        {
            return await _context.LocaisEstoque
                .FirstOrDefaultAsync(l => l.Id == id);
        }

        public async Task<bool> HasProdutosAssociadosAsync(int localEstoqueId)
        {
            var empresaId = _tenantProvider.GetEmpresaId();

            return await _context.ProdutosLocaisEstoque
                .IgnoreQueryFilters()
                .AnyAsync(pl =>
                    pl.EmpresaId == empresaId &&
                    pl.LocalEstoqueId == localEstoqueId &&
                    pl.Ativo);
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
