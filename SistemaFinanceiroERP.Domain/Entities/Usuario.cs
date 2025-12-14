

namespace SistemaFinanceiroERP.Domain.Entities
{
    public class Usuario: BaseEntity
    {


        public string UsuarioNome { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Senha { get; set; } = string.Empty;
        public string Telefone { get; set; } = string.Empty;

        //Relacionamento com a empresa

        public int EmpresaId {  get; set; }

        public Empresa? Empresa { get; set; }
    }
}
