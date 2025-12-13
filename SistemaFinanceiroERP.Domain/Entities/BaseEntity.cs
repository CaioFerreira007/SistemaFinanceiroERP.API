using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaFinanceiroERP.Domain.Entities
{
    public abstract class BaseEntity
    {
        public int Id { get; set; }
        public DateTime DataCriacao { get; set; }

        public DateTime DataAtualizacao{ get; set; }

        public bool Ativo { get; set; } = true;
    }
}
