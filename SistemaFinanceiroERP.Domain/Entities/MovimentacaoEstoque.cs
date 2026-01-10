using SistemaFinanceiroERP.Domain.Enums;


namespace SistemaFinanceiroERP.Domain.Entities
{
    public class MovimentacaoEstoque: BaseEntity
    {
        public int ProdutoId { get; set; }
        public Produto? Produto { get; set; }
        public int LocalEstoqueId { get; set; }
        public LocalEstoque? LocalEstoque { get; set; }
        public  TipoMovimentacao TipoMovimentacao{ get; set; }
        public decimal Quantidade { get; set; }
        public string? Observacao { get; set; }
        public DateTime DataMovimentacao { get; set; }
        public int UsuarioId { get; set; }
        public Usuario? Usuario { get; set; }
        public int EmpresaId { get; set; }
        public Empresa? Empresa { get; set; }

    }
}
