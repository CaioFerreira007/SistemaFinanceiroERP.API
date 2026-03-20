using SistemaFinanceiroERP.Domain.Entities;


namespace SistemaFinanceiroERP.Domain.Interfaces
{
    public interface IItemTransacaoRepository : IRepository<ItemTransacao>
    {
        Task<IEnumerable<ItemTransacao>> GetByProdutoIdAsync(int produtoId);
        Task<IEnumerable<ItemTransacao>> GetByTransacaoIdAsync(int transacaoId);
    }
}
