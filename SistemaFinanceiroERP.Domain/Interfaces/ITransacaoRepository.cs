using SistemaFinanceiroERP.Domain.Entities;
using SistemaFinanceiroERP.Domain.Enums;

namespace SistemaFinanceiroERP.Domain.Interfaces
{
    public interface ITransacaoRepository
    {
        Task<Transacao?> RegistrarTransacaoAsync(Transacao transacao);
        Task<IEnumerable<Transacao>> GetAllAsync();
        Task<Transacao?> GetByIdAsync(int id);
        Task<IEnumerable<Transacao>> GetTransacoesComoCompradorAsync();
        Task<IEnumerable<Transacao>> GetTransacoesComoVendedorAsync();
        Task<Transacao?> GetTransacaoComItensAsync(int id);
        Task AtualizarStatusAsync(int id, StatusTransacao novoStatus);
    }
}
