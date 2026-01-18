
using System.ComponentModel.DataAnnotations.Schema;

namespace SistemaFinanceiroERP.Domain.Entities
{
    public class ItemTransacao:BaseEntity
    {
        public int TransacaoId { get; set; }
        public Transacao? Transacao { get; set; }

        public int ProdutoId { get; set; }
        public Produto? Produto { get; set; }

        public int EmpresaId{ get; set; }
        public Empresa? Empresa { get; set; }

        public decimal Quantidade { get; set; }

        public decimal PrecoUnitario { get; set; }

        [NotMapped]
        public decimal Subtotal => PrecoUnitario * Quantidade;
    }
}
