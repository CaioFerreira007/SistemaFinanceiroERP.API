using Microsoft.EntityFrameworkCore;
using SistemaFinanceiroERP.Domain.Entities;
using SistemaFinanceiroERP.Domain.Interfaces;
using SistemaFinanceiroERP.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace SistemaFinanceiroERP.Infrastructure.Repositories
{
    public class ItemTransacaoRepository : Repository<ItemTransacao>, IItemTransacaoRepository
    {
        public ItemTransacaoRepository(AppDbContext context) : base(context)
        {
        }

        public new async Task<IEnumerable<ItemTransacao>> GetByTransacaoIdAsync(int transacaoId)
        {
            return await _context.Set<ItemTransacao>()
                      .Include(i => i.Produto)
        .Include(i => i.Transacao)
            .ThenInclude(t => t.EmpresaCompradora)
        .Include(i => i.Transacao)
            .ThenInclude(t => t.EmpresaVendedora)
        .Where(i => i.TransacaoId == transacaoId)
        .ToListAsync();
        }

        public new async Task<IEnumerable<ItemTransacao>> GetByProdutoIdAsync(int produtoId)
        {
            return await _context.Set<ItemTransacao>()
                .Include(i => i.Produto)
                .Where(i => i.ProdutoId == produtoId)
                .ToListAsync();

        }
    }
}
