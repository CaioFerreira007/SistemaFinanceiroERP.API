using Microsoft.EntityFrameworkCore;
using SistemaFinanceiroERP.Domain.Entities;
using SistemaFinanceiroERP.Domain.Enums;
using SistemaFinanceiroERP.Domain.Interfaces;
using SistemaFinanceiroERP.Infrastructure.Data;


namespace SistemaFinanceiroERP.Infrastructure.Repositories
{
    public class TransacaoRepository : Repository<Transacao>, ITransacaoRepository
    {
        public TransacaoRepository(AppDbContext context) : base(context)
        {
        }

        public new async Task<IEnumerable<Transacao>> GetAllAsync()
        {
            return await _context.Transacoes
                .Include(t => t.EmpresaVendedora)
                .Include(t => t.EmpresaCompradora)
                .Include(t => t.Usuario)
                .Include(t => t.ItemsTransacao)
                .ToListAsync();
        }

        public new async Task<Transacao?> GetByIdAsync(int id)
        {
            return await _context.Set<Transacao>()
                .Include(t => t.EmpresaVendedora)
                .Include(t => t.EmpresaCompradora)
                .Include(t => t.Usuario)
                .Include(t => t.ItemsTransacao)
                .FirstOrDefaultAsync(t => t.Id == id);
        }
        private async Task<string> GerarNumeroTransacaoAsync()
        {
            var anoAtual = DateTime.UtcNow.Year;
            var prefixo = $"TRN-{anoAtual}-";

            var ultimoNumero = await _context.Set<Transacao>()
                .IgnoreQueryFilters()
                .Where(t => t.NumeroTransacao.StartsWith(prefixo))
                .OrderByDescending(t => t.NumeroTransacao)
                .Select(t => t.NumeroTransacao)
                .FirstOrDefaultAsync();

            int proximoNumero = 1;

            if (!string.IsNullOrEmpty(ultimoNumero))
            {
                var partes = ultimoNumero.Split('-');
                if (partes.Length == 3 && int.TryParse(partes[2], out int numero))
                {
                    proximoNumero = numero + 1;
                }
            }

            return $"{prefixo}{proximoNumero:D6}";
        }

        public async Task<Transacao?> RegistrarTransacaoAsync(Transacao transacao)
        {
            transacao.NumeroTransacao = await GerarNumeroTransacaoAsync();

            foreach (var item in transacao.ItemsTransacao)
            {
                item.EmpresaId = transacao.EmpresaCompradoraId;
            }

            await _context.Set<Transacao>().AddAsync(transacao);

            await _context.SaveChangesAsync();

            return transacao;
        }


        public async Task<Transacao?> GetTransacaoComItensAsync(int id)
        {
            return await _context.Set<Transacao>()
                .Include(t => t.ItemsTransacao)
                .ThenInclude(i => i.Produto)
                .FirstOrDefaultAsync(t => t.Id == id);
        }

        public async Task AtualizarStatusAsync(int id, StatusTransacao novoStatus)
        {
            var transacao = await _context.Set<Transacao>()
                .FirstOrDefaultAsync(t => t.Id == id);
            if (transacao != null)
            {
                transacao.StatusTransacao = novoStatus;
                await _context.SaveChangesAsync();
            }
        }

        public async Task<IEnumerable<Transacao>> GetTransacoesComoCompradorAsync()
        {
            return await _context.Set<Transacao>()
                .Include(t => t.EmpresaVendedora)
                .Include(t => t.EmpresaCompradora)
                .Include(t => t.Usuario)
                .Include(t => t.ItemsTransacao)
                .Where(t => t.EmpresaCompradoraId == _context.CurrentEmpresaId)
                .ToListAsync();
        }

        public async Task<IEnumerable<Transacao>> GetTransacoesComoVendedorAsync()
        {
            return await _context.Set<Transacao>()
                .Include(t => t.EmpresaVendedora)
                .Include(t => t.EmpresaCompradora)
                .Include(t => t.Usuario)
                .Include(t => t.ItemsTransacao)
                .Where(t => t.EmpresaVendedoraId == _context.CurrentEmpresaId)
                .ToListAsync();
        }
    }

}