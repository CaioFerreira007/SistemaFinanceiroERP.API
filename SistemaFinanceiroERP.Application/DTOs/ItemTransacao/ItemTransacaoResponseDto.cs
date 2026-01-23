using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaFinanceiroERP.Application.DTOs.ItemTransacao
{
    public class ItemTransacaoResponseDto
    {
        public int Id { get; set; }
        public int TransacaoId { get; set; }
        public int ProdutoId { get; set; }
        public string ProdutoNome { get; set; } = string.Empty;
        public string CodigoBarras { get; set; } = string.Empty;
        public decimal Quantidade { get; set; }
        public decimal PrecoUnitario { get; set; }
        public decimal Subtotal { get; set; }
    }
}
