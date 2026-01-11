using Microsoft.EntityFrameworkCore;
using SistemaFinanceiroERP.Domain.Entities;
using SistemaFinanceiroERP.Domain.Enums;
using SistemaFinanceiroERP.Domain.Interfaces;
using SistemaFinanceiroERP.Infrastructure.Data;
using SistemaFinanceiroERP.Infrastructure.Security;

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
                .ToListAsync();
        }

        public async Task<MovimentacaoEstoque?> GetByIdAsync(int id)
        {
            return await _context.MovimentacoesEstoque
                .Include(m => m.Produto)
                .Include(m => m.LocalEstoque)
                .Include(m => m.Usuario)
                .FirstOrDefaultAsync(m => m.Id == id);
        }


        public async Task<IEnumerable<MovimentacaoEstoque>> GetByProdutoAsync(int produtoId)
        {
            return await _context.MovimentacoesEstoque
                .Include(m => m.Produto)
                .Include(m => m.LocalEstoque)
                .Include(m => m.Usuario)
                .Where(m => m.ProdutoId == produtoId)
                .ToListAsync();
        }

        public async Task<IEnumerable<MovimentacaoEstoque>> GetByLocalEstoqueAsync(int localEstoqueId)
        {
            return await _context.MovimentacoesEstoque
                .Include(m => m.Produto)
                .Include(m => m.LocalEstoque)
                .Include(m => m.Usuario)
                .Where(m => m.LocalEstoqueId == localEstoqueId)
                .ToListAsync();
        }

        public async Task<MovimentacaoEstoque> RegistrarMovimentacaoAsync(MovimentacaoEstoque movimentacao)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();

            try
            {
           
                var produto = await _context.Produtos
                    .FirstOrDefaultAsync(p => p.Id == movimentacao.ProdutoId);

                if (produto == null)
                    throw new Exception("Produto não encontrado");

          
                var localEstoque = await _context.LocaisEstoque
                    .FirstOrDefaultAsync(l => l.Id == movimentacao.LocalEstoqueId);

                if (localEstoque == null)
                    throw new Exception("Local de estoque não encontrado");

         
                var produtoLocal = await _context.ProdutosLocaisEstoque
                    .FirstOrDefaultAsync(pl => pl.ProdutoId == movimentacao.ProdutoId
                                            && pl.LocalEstoqueId == movimentacao.LocalEstoqueId);

                if (produtoLocal == null)
                {
                    produtoLocal = new ProdutoLocalEstoque
                    {
                        ProdutoId = movimentacao.ProdutoId,
                        LocalEstoqueId = movimentacao.LocalEstoqueId,
                        EmpresaId = _tenantProvider.GetEmpresaId(),
                        QuantidadeNoLocal = 0,
                        DataCriacao = DateTime.UtcNow,
                        Ativo = true
                    };
                    await _context.ProdutosLocaisEstoque.AddAsync(produtoLocal);
                }

             
                if (movimentacao.TipoMovimentacao == TipoMovimentacao.Saida)
                {
                    if (produtoLocal.QuantidadeNoLocal < movimentacao.Quantidade)
                    {
                        throw new InvalidOperationException(
                            $"Estoque insuficiente no local '{localEstoque.LocalNome}'! " +
                            $"Disponível: {produtoLocal.QuantidadeNoLocal}, " +
                            $"Solicitado: {movimentacao.Quantidade}"
                        );
                    }
                }

           
                switch (movimentacao.TipoMovimentacao)
                {
                    case TipoMovimentacao.Entrada:
                        produtoLocal.QuantidadeNoLocal += movimentacao.Quantidade;
                        break;

                    case TipoMovimentacao.Saida:
                        produtoLocal.QuantidadeNoLocal -= movimentacao.Quantidade;
                        break;

                    case TipoMovimentacao.Ajuste:
                        throw new NotImplementedException("Ajuste ainda não implementado (Fase 2.3)");

                    case TipoMovimentacao.Transferencia:
                        throw new NotImplementedException("Transferência ainda não implementada (Fase 2.3)");
                }

                produtoLocal.DataAtualizacao = DateTime.UtcNow;
                produto.DataAtualizacao = DateTime.UtcNow;

                await _context.MovimentacoesEstoque.AddAsync(movimentacao);

                await _context.SaveChangesAsync();

       
                await transaction.CommitAsync();

         
                return movimentacao;
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }
        }
    }
}