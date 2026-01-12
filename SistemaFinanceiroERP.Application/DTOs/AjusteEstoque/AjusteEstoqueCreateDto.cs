using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaFinanceiroERP.Application.DTOs.AjusteEstoque
{
    public class AjusteEstoqueCreateDto
    {
        public int ProdutoId { get; set; }
        public int LocalEstoqueId { get; set; }
        public decimal QuantidadeNova { get; set; }
        public string Observacao { get; set; } = string.Empty;
        public DateTime DataDoAjuste { get; set; }
    }
}
