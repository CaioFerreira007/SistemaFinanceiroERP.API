using System.ComponentModel.DataAnnotations.Schema;

namespace SistemaFinanceiroERP.Domain.Entities
{
    public class AjusteEstoque : BaseEntity
    {
        public int LocalEstoqueId { get; set; }
        public LocalEstoque? LocalEstoque { get; set; }
        public int ProdutoId { get; set; }
        public Produto? Produto { get; set; }
        public int EmpresaId { get; set; }
        public Empresa? Empresa { get; set; }
        public int UsuarioId { get; set; }
        public Usuario? Usuario { get; set; }
        public decimal QuantidadeAnterior { get; set; }
        public decimal QuantidadeNova { get; set; }
        public string Observacao { get; set; } = string.Empty;
        public DateTime DataDoAjuste { get; set; }

        [NotMapped]
        public decimal Diferenca => QuantidadeNova - QuantidadeAnterior;
    }
}