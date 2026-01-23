using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaFinanceiroERP.Application.DTOs.ItemTransacao
{
    public class ItemTransacaoCreateDto
    {
        public  int ProdutoId { get; set; }
        public decimal PrecoUnitario{ get; set; }
        public decimal Quantidade { get; set; }
    }
}
