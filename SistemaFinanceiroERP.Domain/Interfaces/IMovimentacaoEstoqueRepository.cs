using SistemaFinanceiroERP.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaFinanceiroERP.Domain.Interfaces
{
    public interface IMovimentacaoEstoqueRepository
    {
        // Define métodos para manipulação de movimentações de estoque

        Task<MovimentacaoEstoque> RegistrarMovimentacaoAsync(MovimentacaoEstoque movimentacao);

        Task<IEnumerable<MovimentacaoEstoque>> GetAllAsync();
        Task<MovimentacaoEstoque?> GetByIdAsync(int id);
        Task<IEnumerable<MovimentacaoEstoque>> GetByProdutoAsync(int produtoId);
        Task<IEnumerable<MovimentacaoEstoque>> GetByLocalEstoqueAsync(int localEstoqueId);



    }
}
