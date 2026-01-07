using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaFinanceiroERP.Application.DTOs.LocalEstoque
{
    public class LocalEstoqueUpdateDto
    {
        public int Id { get; set; }
        public string LocalNome { get; set; } = string.Empty;
        public string Descricao { get; set; } = string.Empty;   
        public string Estado { get; set; } = string.Empty;
        public string Cidade { get; set; } = string.Empty;
        public string Bairro { get; set; } = string.Empty;
        public string Rua { get; set; } = string.Empty;

    }
}
