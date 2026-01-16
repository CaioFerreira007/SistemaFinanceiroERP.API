using SistemaFinanceiroERP.Domain.Entities;

namespace SistemaFinanceiroERP.Domain.Interfaces
{
    public interface IProdutoRepository : IRepository<Produto>
    {
        Task<IEnumerable<Produto>> GetProdutosEstoqueBaixoAsync();
        Task<IEnumerable<MovimentacaoEstoque>> GetMovimentacoesPorProdutoAsync(int produtoId);
        Task<bool> PossuiMovimentacoesAsync(int produtoId);
    }
}
