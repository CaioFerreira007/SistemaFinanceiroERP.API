using Microsoft.EntityFrameworkCore;
using SistemaFinanceiroERP.Domain.Entities;
using SistemaFinanceiroERP.Domain.Enums;
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
                .Include(m => m.Produto)
                .Include(m => m.LocalEstoque)
                .Include(m => m.Usuario)
                .AsNoTracking()
                .OrderByDescending(m => m.DataCriacao)
                .ToListAsync();
        }

        public async Task<MovimentacaoEstoque?> GetByIdAsync(int id)
        {
            return await _context.MovimentacoesEstoque
                .Include(m => m.Produto)
                .Include(m => m.LocalEstoque)
                .Include(m => m.Usuario)
                .AsNoTracking()
                .FirstOrDefaultAsync(m => m.Id == id);
        }

        public async Task<IEnumerable<MovimentacaoEstoque>> GetByProdutoAsync(int produtoId)
        {
            return await _context.MovimentacoesEstoque
                .Include(m => m.Produto)
                .Include(m => m.LocalEstoque)
                .Include(m => m.Usuario)
                .AsNoTracking()
                .Where(m => m.ProdutoId == produtoId)
                .OrderByDescending(m => m.DataCriacao)
                .ToListAsync();
        }

        public async Task<IEnumerable<MovimentacaoEstoque>> GetByLocalEstoqueAsync(int localEstoqueId)
        {
            return await _context.MovimentacoesEstoque
                .Include(m => m.Produto)
                .Include(m => m.LocalEstoque)
                .Include(m => m.Usuario)
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
            var produto = await _context.Produtos
                .FirstOrDefaultAsync(p => p.Id == movimentacao.ProdutoId);

            if (produto == null)
                throw new InvalidOperationException("Produto não encontrado.");

            var local = await _context.LocaisEstoque
                .FirstOrDefaultAsync(l => l.Id == movimentacao.LocalEstoqueId);

            if (local == null)
                throw new InvalidOperationException("Local de estoque não encontrado.");

            // Buscar ou criar ProdutoLocalEstoque
            var produtoLocal = await _context.ProdutosLocaisEstoque
                .FirstOrDefaultAsync(pl =>
                    pl.ProdutoId == movimentacao.ProdutoId &&
                    pl.LocalEstoqueId == movimentacao.LocalEstoqueId);

            if (produtoLocal == null)
            {
                produtoLocal = new ProdutoLocalEstoque
                {
                    ProdutoId = movimentacao.ProdutoId,
                    LocalEstoqueId = movimentacao.LocalEstoqueId,
                    EmpresaId = movimentacao.EmpresaId,
                    QuantidadeNoLocal = 0,
                    DataCriacao = DateTime.UtcNow,
                    Ativo = true
                };
                _context.ProdutosLocaisEstoque.Add(produtoLocal);
            }

            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                switch (movimentacao.TipoMovimentacao)
                {
                    case TipoMovimentacao.Entrada:
                        produtoLocal.QuantidadeNoLocal += movimentacao.Quantidade;
                        break;

                    case TipoMovimentacao.Saida:
                        if (produtoLocal.QuantidadeNoLocal < movimentacao.Quantidade)
                        {
                            throw new InvalidOperationException(
                                $"Estoque insuficiente. Disponível: {produtoLocal.QuantidadeNoLocal}, " +
                                $"Solicitado: {movimentacao.Quantidade}");
                        }
                        produtoLocal.QuantidadeNoLocal -= movimentacao.Quantidade;
                        break;

                    case TipoMovimentacao.Transferencia:
                        throw new NotImplementedException(
                            "Transferência entre locais deve ser implementada com dois registros de movimentação.");

                    case TipoMovimentacao.Ajuste:
                        throw new InvalidOperationException(
                            "Use o AjusteEstoqueRepository para registrar ajustes de estoque.");
                }

                produtoLocal.DataAtualizacao = DateTime.UtcNow;

                movimentacao.DataCriacao = DateTime.UtcNow;
                movimentacao.Ativo = true;
                _context.MovimentacoesEstoque.Add(movimentacao);

                await _context.SaveChangesAsync();
                await transaction.CommitAsync();
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}