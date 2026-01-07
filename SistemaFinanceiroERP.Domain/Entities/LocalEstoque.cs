using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaFinanceiroERP.Domain.Entities
{
    public class LocalEstoque : BaseEntity
    {
        public string LocalNome { get; set; } = string.Empty;
        public string Descricao { get; set; } = string.Empty;
        public string Estado { get; set; } = string.Empty;
        public string Cidade { get; set; } = string.Empty;
        public string Bairro { get; set; } = string.Empty;
        public string Rua { get; set; } = string.Empty;

        //chave estrangeira para Empresa
        public int EmpresaId { get; set; }
        public Empresa Empresa { get; set; } = null!;


        public ICollection<ProdutoLocalEstoque> ProdutosLocaisEstoque { get; set; } = new List<ProdutoLocalEstoque>();



    }
}
