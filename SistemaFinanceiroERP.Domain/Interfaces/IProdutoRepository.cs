using SistemaFinanceiroERP.Domain.Entities;

namespace SistemaFinanceiroERP.Domain.Interfaces
{
    public interface IProdutoRepository
    {
        Task AddAsync(Produto produto);
        Task UpdateAsync(Produto produto);

        Task<IEnumerable<Produto>> GetAllAsync();
        Task<Produto?> GetByIdAsync(int id);

        Task<bool> HasMovimentacoesAsync(int produtoId);

        Task SaveChangesAsync();
    }
}
