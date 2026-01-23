using SistemaFinanceiroERP.Application.DTOs.ItemTransacao;
using SistemaFinanceiroERP.Domain.Entities;
using SistemaFinanceiroERP.Domain.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaFinanceiroERP.Application.DTOs.Transacao
{
    public class TransacaoResponseDto
    {
        public int Id { get; set; }
        public string NumeroTransacao { get; set; } = string.Empty;
        public DateTime DataTransacao { get; set; } 
        public StatusTransacao StatusTransacao { get; set; }
        public decimal Desconto { get; set; }
        public string Observacao { get; set; } = string.Empty;

        public int EmpresaVendedoraId { get; set; }
        public string EmpresaVendedoraRazaoSocial { get; set; } = string.Empty;
        public string EmpresaVendedoraNome { get; set; } = string.Empty;

        public int EmpresaCompradoraId { get; set; }
        public string EmpresaCompradoraRazaoSocial { get; set; } = string.Empty;
        public string EmpresaCompradoraNome { get; set; } = string.Empty;

        public int UsuarioId { get; set; }
        public string UsuarioNome { get; set; } = string.Empty;

        public decimal ValorTotal { get; set; }

        public ICollection<ItemTransacaoResponseDto>? Itens { get; set; }


    }
}
