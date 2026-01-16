using Microsoft.EntityFrameworkCore;
using SistemaFinanceiroERP.Domain.Entities;
using SistemaFinanceiroERP.Domain.Enums;
using SistemaFinanceiroERP.Domain.Interfaces;
using SistemaFinanceiroERP.Infrastructure.Data;

namespace SistemaFinanceiroERP.Infrastructure.Repositories
{
    public class AjusteEstoqueRepository : IAjusteEstoqueRepository
    {
        private readonly AppDbContext _context;
        private readonly ITenantProvider _tenantProvider;

        public AjusteEstoqueRepository(AppDbContext context, ITenantProvider tenantProvider)
        {
            _context = context;
            _tenantProvider = tenantProvider;
        }

        public async Task<IEnumerable<AjusteEstoque>> GetAllAsync()
        {
            return await _context.AjusteEstoque
            .Include(m => m.Produto)
            .Include(m => m.LocalEstoque)
            .Include(m => m.Usuario)
            .ToListAsync();
        }

        public async Task<AjusteEstoque?> GetByIdAsync(int id)
        {
            return await _context.AjusteEstoque
                .Include(m => m.Produto)
                .Include(m => m.LocalEstoque)
                .Include(m => m.Usuario)
                .FirstOrDefaultAsync(m => m.Id == id);

        }

        public async Task<IEnumerable<AjusteEstoque>> GetByProdutoIdAsync(int produtoId)
        {
            return await _context.AjusteEstoque
                .Include(m => m.Produto)
                .Include(m => m.LocalEstoque)
                .Include(m => m.Usuario)
                .Where(m => m.ProdutoId == produtoId)
                .ToListAsync();
        }

        public async Task<IEnumerable<AjusteEstoque>> GetByLocalEstoqueIdAsync(int localEstoqueId)
        {
            return await _context.AjusteEstoque
                .Include(m => m.Produto)
                .Include(m => m.LocalEstoque)
                .Include(m => m.Usuario)
                .Where(m => m.LocalEstoqueId == localEstoqueId)
                .ToListAsync();
        }
        public async Task<AjusteEstoque> RegistrarAjusteEstoqueAsync(AjusteEstoque ajusteEstoque)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();

            try
            {
                var produto = await _context.Produtos.
                    SingleOrDefaultAsync(p => p.Id == ajusteEstoque.ProdutoId);
                if (produto == null)
                {
                    throw new Exception("Produto não encontrado");
                }
                var localEstoque = await _context.LocaisEstoque
             .FirstOrDefaultAsync(l => l.Id == ajusteEstoque.LocalEstoqueId);

                if (localEstoque == null)
                    throw new Exception("Local de estoque não encontrado");


                var produtoLocal = await _context.ProdutosLocaisEstoque
                    .FirstOrDefaultAsync(pl => pl.ProdutoId == ajusteEstoque.ProdutoId
                                            && pl.LocalEstoqueId == ajusteEstoque.LocalEstoqueId);

                if (produtoLocal == null)
                {
                    produtoLocal = new ProdutoLocalEstoque
                    {
                        ProdutoId = ajusteEstoque.ProdutoId,
                        LocalEstoqueId = ajusteEstoque.LocalEstoqueId,
                        EmpresaId = _tenantProvider.GetEmpresaId(),
                        QuantidadeNoLocal = 0,
                        DataCriacao = DateTime.UtcNow,
                        Ativo = true
                    };
                    await _context.ProdutosLocaisEstoque.AddAsync(produtoLocal);
                }

                ajusteEstoque.QuantidadeAnterior = produtoLocal.QuantidadeNoLocal;
                produtoLocal.QuantidadeNoLocal = ajusteEstoque.QuantidadeNova;
                produtoLocal.DataAtualizacao = DateTime.UtcNow;
                produto.DataAtualizacao = DateTime.UtcNow;
                ajusteEstoque.DataCriacao = DateTime.UtcNow;
                ajusteEstoque.DataAtualizacao = DateTime.UtcNow;
                ajusteEstoque.DataDoAjuste = DateTime.UtcNow;
                await _context.AjusteEstoque.AddAsync(ajusteEstoque);
                await _context.SaveChangesAsync();
                await transaction.CommitAsync();
                return await _context.AjusteEstoque
                     .Include(a => a.Produto)
                     .Include(a => a.LocalEstoque)
                     .Include(a => a.Usuario)
                     .FirstAsync(a => a.Id == ajusteEstoque.Id);

            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }

        }
    }
}
