
using System.ComponentModel.DataAnnotations.Schema;

namespace SistemaFinanceiroERP.Domain.Entities
    {
        public class Produto:BaseEntity
        {

            public string ProdutoNome { get; set; } = string.Empty;

            public string Categoria { get; set; } = string.Empty;

            public string Descricao { get; set; } = string.Empty;

            public decimal PrecoUnitario { get; set; }

            public string CodigoBarras { get; set; } = string.Empty;

        public ICollection<ProdutoLocalEstoque> ProdutosLocaisEstoque { get; set; } = new List<ProdutoLocalEstoque>();

        [NotMapped]
        public decimal QuantidadeEstoqueTotal
        {
            get => ProdutosLocaisEstoque?.Sum(pl => pl.QuantidadeNoLocal) ?? 0;
        }

        public string UnidadeMedida { get; set; } = string.Empty;

            public int EmpresaId {  get; set; }

            public Empresa? Empresa { get; set; }


        }
    }
