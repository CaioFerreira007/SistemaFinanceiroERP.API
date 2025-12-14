using Microsoft.AspNetCore.Mvc;
using SistemaFinanceiroERP.Domain.Entities;
using SistemaFinanceiroERP.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace SistemaFinanceiroERP.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmpresaController : ControllerBase
    {
        private readonly AppDbContext _context;

        public EmpresaController(AppDbContext context)  
        {
            _context = context;
        }

        [HttpPost]

        public async Task<ActionResult<Empresa>> Create([FromBody] Empresa empresa)
        {
            empresa.DataCriacao = DateTime.UtcNow;
            empresa.Ativo = true;
            _context.Empresas.Add(empresa);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetById), new { id = empresa.Id }, empresa);
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Empresa>>> GetAll()
        {
            var empresas = await _context.Empresas.ToListAsync();
            return Ok(empresas);
        }

        [HttpGet("{id}")]

        public async Task <ActionResult<Empresa>> GetById(int id)
        {
            var empresa = await _context.Empresas.FindAsync(id);
            if(empresa == null)
            {
                return NotFound();
            }
            return Ok(empresa);
        }

        [HttpPut("{id}")]

        public async Task<ActionResult<Empresa>> Update(int id, [FromBody]Empresa empresaAtualizada)
        {
            if (id != empresaAtualizada.Id)
            {
                return BadRequest("ID da URL não corresponde ao ID da empresa");
            }

            var empresaExistente = await _context.Empresas.FindAsync(id);

            if (empresaExistente == null)
            {
                return NotFound();
            }

            empresaExistente.Cnpj = empresaAtualizada.Cnpj;
            empresaExistente.RazaoSocial = empresaAtualizada.RazaoSocial;
            empresaExistente.NomeEmpresa = empresaAtualizada.NomeEmpresa;
            empresaExistente.Telefone = empresaAtualizada.Telefone;
            empresaExistente.Email = empresaAtualizada.Email;
            empresaExistente.Tipo = empresaAtualizada.Tipo;
            empresaExistente.DataAtualizacao = DateTime.UtcNow;

            await _context.SaveChangesAsync();

            return Ok(empresaExistente);
        }

        [HttpDelete("{id}")]

        public async Task<ActionResult> Delete(int id)
        {
          var empresa = await _context.Empresas.FindAsync(id);

            if (empresa == null)
            {
                return NotFound();
            }
            empresa.Ativo = false;
            empresa.DataAtualizacao = DateTime.UtcNow;

            await _context.SaveChangesAsync();

            return NoContent();

        }

       
    }
}
