using SistemaFinanceiroERP.Domain.Entities;

namespace SistemaFinanceiroERP.Domain.Interfaces
{
    public interface ILocalEstoqueRepository
    {
        Task AddAsync(LocalEstoque local);
        Task UpdateAsync(LocalEstoque local);

        Task<IEnumerable<LocalEstoque>> GetAllAsync();
        Task<LocalEstoque?> GetByIdAsync(int id);

        Task<bool> HasProdutosAssociadosAsync(int localEstoqueId);

        Task SaveChangesAsync();
    }
}
