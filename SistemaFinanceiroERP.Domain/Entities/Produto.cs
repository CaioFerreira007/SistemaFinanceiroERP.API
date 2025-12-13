
namespace SistemaFinanceiroERP.Domain.Entities
{
    public class Produto:BaseEntity
    {

        public string ProdutoNome { get; set; } = string.Empty;

        public string Categoria { get; set; } = string.Empty;

        public string Descricao { get; set; } = string.Empty;

        public decimal PrecoUnitario { get; set; }

        public string CodigoBarras { get; set; } = string.Empty;

        public int QuantidadeEstoque { get; set; }

        public string UnidadeMedida { get; set; } = string.Empty;

        public int EmpresaId {  get; set; }

        public Empresa? Empresa { get; set; }


    }
}
