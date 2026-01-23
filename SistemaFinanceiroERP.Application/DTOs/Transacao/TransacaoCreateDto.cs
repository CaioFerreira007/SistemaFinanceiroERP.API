

using SistemaFinanceiroERP.Application.DTOs.ItemTransacao;
using SistemaFinanceiroERP.Domain.Enums;

namespace SistemaFinanceiroERP.Application.DTOs.Transacao
{
    public class TransacaoCreateDto
    {
        public int EmpresaCompradoraId { get; set; }
        public int EmpresaVendedoraId { get; set; }
        public StatusTransacao StatusTransacao { get; set; }
        public decimal Desconto { get; set; }
        public string Observacao { get; set; } = string.Empty;
        public ICollection<ItemTransacaoCreateDto>? ItensTransacao { get; set; }
        public DateTime DataTransacao { get; set; } = DateTime.UtcNow;
    }
}
