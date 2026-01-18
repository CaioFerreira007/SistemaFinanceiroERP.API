using SistemaFinanceiroERP.Domain.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaFinanceiroERP.Domain.Entities
{
    public class Transacao:BaseEntity
    {
        public string NumeroTransacao { get; set; } = string.Empty;

        public int EmpresaVendedoraId { get; set; }
        public Empresa? EmpresaVendedora { get; set; }

        public int EmpresaCompradoraId { get; set; }
        public Empresa? EmpresaCompradora { get; set; }

        public DateTime DataTransacao { get; set; }

        public StatusTransacao StatusTransacao { get; set; }
        [NotMapped]
        public decimal ValorTotal => (ItemsTransacao?.Sum(item => item.Subtotal) ?? 0) - Desconto;

        public decimal Desconto { get; set; }

        public string ObservacoesTransacao { get; set; } = string.Empty;

        public int UsuarioId { get; set; }
        public Usuario? Usuario { get; set; }

        public ICollection<ItemTransacao> ItemsTransacao{ get; set; } = new List<ItemTransacao>();

    }
}
