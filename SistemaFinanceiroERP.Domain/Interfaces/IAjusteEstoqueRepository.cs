using SistemaFinanceiroERP.Domain.Entities;
using System.Collections.Generic;

namespace SistemaFinanceiroERP.Domain.Interfaces
{
    public interface IAjusteEstoqueRepository
    {
        Task<AjusteEstoque>RegistrarAjusteEstoqueAsync(AjusteEstoque ajusteEstoque);
        Task<IEnumerable<AjusteEstoque>> GetAllAsync();
        Task<AjusteEstoque?> GetByIdAsync(int id);
        Task<IEnumerable<AjusteEstoque>> GetByProdutoIdAsync(int produtoId);
        Task<IEnumerable<AjusteEstoque>>GetByLocalEstoqueIdAsync(int localEstoqueId);
    }
}
