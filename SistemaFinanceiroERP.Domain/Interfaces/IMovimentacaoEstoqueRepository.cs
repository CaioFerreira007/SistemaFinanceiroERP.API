using SistemaFinanceiroERP.Domain.Entities;

namespace SistemaFinanceiroERP.Domain.Interfaces
{
    public interface IMovimentacaoEstoqueRepository
    {
        Task<MovimentacaoEstoque> RegistrarMovimentacaoAsync(MovimentacaoEstoque movimentacao);
        Task<IEnumerable<MovimentacaoEstoque>> GetAllAsync();
        Task<MovimentacaoEstoque?> GetByIdAsync(int id);
        Task<IEnumerable<MovimentacaoEstoque>> GetByProdutoAsync(int produtoId);
        Task<IEnumerable<MovimentacaoEstoque>> GetByLocalEstoqueAsync(int localEstoqueId);
    }
}