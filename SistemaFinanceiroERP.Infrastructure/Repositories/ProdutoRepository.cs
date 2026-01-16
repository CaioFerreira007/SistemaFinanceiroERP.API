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
            return await _context.Set<Produto>()
                .Include(p => p.LocalEstoque) 
                .AsNoTracking()
                .ToListAsync();
        }

        public new async Task<Produto?> GetByIdAsync(int id)
        {
            return await _context.Set<Produto>()
                .Include(p => p.LocalEstoque) 
                .FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task<IEnumerable<Produto>> GetProdutosEstoqueBaixoAsync()
        {
            var produtos = await _context.Set<Produto>()
                .Include(p => p.ProdutosLocaisEstoque)
                .Include(p => p.LocalEstoque)  
                .AsNoTracking()
                .ToListAsync();

            return produtos
                .Where(p => p.EstoqueMinimo > 0 && p.QuantidadeEstoqueTotal < p.EstoqueMinimo)
                .ToList();
        }

        public async Task<IEnumerable<MovimentacaoEstoque>> GetMovimentacoesPorProdutoAsync(int produtoId)
        {
            return await _context.Set<MovimentacaoEstoque>()
                .Where(m => m.ProdutoId == produtoId)
                .OrderByDescending(m => m.DataCriacao)
                .ToListAsync();
        }

        public async Task<bool> PossuiMovimentacoesAsync(int produtoId)
        {
            return await _context.Set<MovimentacaoEstoque>()
                .AnyAsync(m => m.ProdutoId == produtoId);
        }
    }
}