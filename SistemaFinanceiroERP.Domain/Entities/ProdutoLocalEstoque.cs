namespace SistemaFinanceiroERP.Domain.Entities
{
    public class ProdutoLocalEstoque:BaseEntity
    {
        public int ProdutoId { get; set; }
        public Produto Produto { get; set; } = null!;

        public int LocalEstoqueId { get; set; }
        public LocalEstoque LocalEstoque { get; set; } = null!;

        public decimal QuantidadeNoLocal { get; set; }

        public int EmpresaId { get; set; }
        public Empresa Empresa { get; set; } = null!;

    }
}
