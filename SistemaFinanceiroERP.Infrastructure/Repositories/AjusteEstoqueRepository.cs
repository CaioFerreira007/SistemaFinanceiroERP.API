using Microsoft.EntityFrameworkCore;
using SistemaFinanceiroERP.Domain.Entities;
using SistemaFinanceiroERP.Domain.Enums;
using SistemaFinanceiroERP.Domain.Interfaces;
using SistemaFinanceiroERP.Infrastructure.Data;

namespace SistemaFinanceiroERP.Infrastructure.Repositories
{
    public class AjusteEstoqueRepository : Repository<AjusteEstoque>, IAjusteEstoqueRepository
    {

        public AjusteEstoqueRepository(AppDbContext context) : base(context)
        {
        }

        public new async Task<IEnumerable<AjusteEstoque>> GetAllAsync()
        {
            return await _context.Set<AjusteEstoque>()
                .Include(a => a.Produto)       
                .Include(a => a.LocalEstoque)   
                .Include(a => a.Usuario)        
                .AsNoTracking()
                .OrderByDescending(a => a.DataCriacao)
                .ToListAsync();
        }

        public new async Task<AjusteEstoque?> GetByIdAsync(int id)
        {
            return await _context.Set<AjusteEstoque>()
                .Include(a => a.Produto)        
                .Include(a => a.LocalEstoque)   
                .Include(a => a.Usuario)       
                .AsNoTracking()
                .FirstOrDefaultAsync(a => a.Id == id);
        }

        public async Task<AjusteEstoque> RegistrarAjusteEstoqueAsync(AjusteEstoque ajuste)
        {
            var produto = await _context.Set<Produto>()
                .Include(p => p.ProdutosLocaisEstoque)
                .FirstOrDefaultAsync(p => p.Id == ajuste.ProdutoId);

            if (produto == null)
                throw new InvalidOperationException("Produto nao encontrado.");

            var local = await _context.Set<LocalEstoque>()
                .FirstOrDefaultAsync(l => l.Id == ajuste.LocalEstoqueId);

            if (local == null)
                throw new InvalidOperationException("Local de estoque nao encontrado.");

            var produtoLocal = await _context.Set<ProdutoLocalEstoque>()
                .FirstOrDefaultAsync(pl => pl.ProdutoId == ajuste.ProdutoId && pl.LocalEstoqueId == ajuste.LocalEstoqueId);

            if (produtoLocal == null)
            {
                produtoLocal = new ProdutoLocalEstoque
                {
                    ProdutoId = ajuste.ProdutoId,
                    LocalEstoqueId = ajuste.LocalEstoqueId,
                    EmpresaId = ajuste.EmpresaId,
                    QuantidadeNoLocal = 0,
                    DataCriacao = DateTime.UtcNow,
                    Ativo = true
                };

                _context.Set<ProdutoLocalEstoque>().Add(produtoLocal);
                await _context.SaveChangesAsync();
            }

            var quantidadeAnterior = produtoLocal.QuantidadeNoLocal;
            var quantidadeNova = ajuste.QuantidadeNova;

            if (quantidadeNova < 0)
                throw new InvalidOperationException("QuantidadeNova nao pode ser negativa.");

            var diferenca = quantidadeNova - quantidadeAnterior;

            ajuste.QuantidadeAnterior = quantidadeAnterior;
            ajuste.Diferenca = diferenca;
            ajuste.DataCriacao = DateTime.UtcNow;
            ajuste.Ativo = true;

            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                produtoLocal.QuantidadeNoLocal = quantidadeNova;
                produtoLocal.DataAtualizacao = DateTime.UtcNow;

                var mov = new MovimentacaoEstoque
                {
                    ProdutoId = ajuste.ProdutoId,
                    LocalEstoqueId = ajuste.LocalEstoqueId,
                    TipoMovimentacao = TipoMovimentacao.Ajuste,
                    Quantidade = Math.Abs(diferenca),
                    Observacao = $"Ajuste de estoque: {quantidadeAnterior} -> {quantidadeNova}. Motivo: {ajuste.Observacao}",
                    UsuarioId = ajuste.UsuarioId,
                    EmpresaId = ajuste.EmpresaId,
                    DataCriacao = DateTime.UtcNow,
                    Ativo = true
                };

                _context.Set<MovimentacaoEstoque>().Add(mov);
                _context.Set<AjusteEstoque>().Add(ajuste);

                await _context.SaveChangesAsync();
                await transaction.CommitAsync();

                return ajuste;
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }
        }

        public async Task<IEnumerable<AjusteEstoque>> GetByProdutoIdAsync(int produtoId)
        {
            return await _context.Set<AjusteEstoque>()
                .Include(a => a.Produto)        
                .Include(a => a.LocalEstoque)   
                .Include(a => a.Usuario)       
                .AsNoTracking()
                .Where(a => a.ProdutoId == produtoId)
                .OrderByDescending(a => a.DataCriacao)
                .ToListAsync();
        }

        public async Task<IEnumerable<AjusteEstoque>> GetByLocalEstoqueIdAsync(int localEstoqueId)
        {
            return await _context.Set<AjusteEstoque>()
                .Include(a => a.Produto)        
                .Include(a => a.LocalEstoque)   
                .Include(a => a.Usuario)       
                .AsNoTracking()
                .Where(a => a.LocalEstoqueId == localEstoqueId)
                .OrderByDescending(a => a.DataCriacao)
                .ToListAsync();
        }
    }
}