using Microsoft.EntityFrameworkCore;
using SistemaFinanceiroERP.Domain.Entities;
using SistemaFinanceiroERP.Domain.Interfaces;
using SistemaFinanceiroERP.Infrastructure.Data;

namespace SistemaFinanceiroERP.Infrastructure.Repositories
{
    public class MovimentacaoEstoqueRepository : IMovimentacaoEstoqueRepository
    {
        private readonly AppDbContext _context;
        private readonly ITenantProvider _tenantProvider;

        public MovimentacaoEstoqueRepository(AppDbContext context, ITenantProvider tenantProvider)
        {
            _context = context;
            _tenantProvider = tenantProvider;
        }

        public async Task<IEnumerable<MovimentacaoEstoque>> GetAllAsync()
        {
            return await _context.MovimentacoesEstoque
                .AsNoTracking()
                .OrderByDescending(m => m.DataCriacao)
                .ToListAsync();
        }

        public async Task<MovimentacaoEstoque?> GetByIdAsync(int id)
        {
            return await _context.MovimentacoesEstoque
                .AsNoTracking()
                .FirstOrDefaultAsync(m => m.Id == id);
        }

        public async Task<IEnumerable<MovimentacaoEstoque>> GetByProdutoAsync(int produtoId)
        {
            return await _context.MovimentacoesEstoque
                .AsNoTracking()
                .Where(m => m.ProdutoId == produtoId)
                .OrderByDescending(m => m.DataCriacao)
                .ToListAsync();
        }

        public async Task<IEnumerable<MovimentacaoEstoque>> GetByLocalEstoqueAsync(int localEstoqueId)
        {
            return await _context.MovimentacoesEstoque
                .AsNoTracking()
                .Where(m => m.LocalEstoqueId == localEstoqueId)
                .OrderByDescending(m => m.DataCriacao)
                .ToListAsync();
        }

        public async Task<bool> ExistsByProdutoIdAsync(int produtoId)
        {
            var empresaId = _tenantProvider.GetEmpresaId();

            return await _context.MovimentacoesEstoque
                .IgnoreQueryFilters()
                .AnyAsync(m => m.EmpresaId == empresaId && m.ProdutoId == produtoId);
        }

        public async Task RegistrarMovimentacaoAsync(MovimentacaoEstoque movimentacao)
        {
            _context.MovimentacoesEstoque.Add(movimentacao);
            await _context.SaveChangesAsync();
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
