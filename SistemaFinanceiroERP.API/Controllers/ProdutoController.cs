using Microsoft.AspNetCore.Http;
using SistemaFinanceiroERP.Domain.Entities;
using SistemaFinanceiroERP.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

using Microsoft.AspNetCore.Mvc;

namespace SistemaFinanceiroERP.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProdutoController : ControllerBase
    {
        private readonly AppDbContext _context;

        public ProdutoController(AppDbContext context)
        {
            _context = context;
        }

        [HttpPost]

        public async Task<ActionResult<Produto>> Create([FromBody] Produto produto)
        {

            var empresa = await _context.Empresas.FindAsync(produto.EmpresaId);
            if(empresa == null || !empresa.Ativo)
            {
                return BadRequest("Empresa não encontrada ou inativa");
            }
            produto.DataCriacao = DateTime.UtcNow;
            produto.Ativo = true;

            _context.Produtos.Add(produto);

            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetById), new { id = produto.Id }, produto);
        }

        [HttpGet]

        public async Task<ActionResult<IEnumerable<Produto>>> GetAll()
        {
            var produtos = await _context.Produtos.ToListAsync();

            return Ok(produtos);
        }

        [HttpGet("{id}")]

        public async Task<ActionResult<Produto>> GetById(int id)
        {
            var produto = await _context.Produtos.FindAsync(id);

            if (produto == null)
            {
                return NotFound();
            }
            return Ok(produto);
        }

        [HttpPut("{id}")]

        public async Task<ActionResult<Produto>> Update(int id, [FromBody] Produto produtoAtualizado)
        {

            var empresa = await _context.Empresas.FindAsync(produtoAtualizado.EmpresaId);
            if (empresa == null || !empresa.Ativo)
            {
                return BadRequest("Empresa não encontrada ou inativa");
            }
            if (id != produtoAtualizado.Id )
            {
                return BadRequest("O id da url não corresponde ao id do produto");
            }
            var produtoExiste = await _context.Produtos.FindAsync(id);
            if(produtoExiste == null)
            {
                return NotFound();
            }

            produtoExiste.ProdutoNome = produtoAtualizado.ProdutoNome;
            produtoExiste.Categoria = produtoAtualizado.Categoria;
            produtoExiste.Descricao = produtoAtualizado.Descricao;
                produtoExiste.PrecoUnitario = produtoAtualizado.PrecoUnitario;
                produtoExiste.CodigoBarras = produtoAtualizado.CodigoBarras;
                produtoExiste.QuantidadeEstoque = produtoAtualizado.QuantidadeEstoque;
            produtoExiste.UnidadeMedida = produtoAtualizado.UnidadeMedida;
            produtoExiste.DataAtualizacao = DateTime.UtcNow;
                produtoExiste.EmpresaId = produtoAtualizado.EmpresaId;

            await _context.SaveChangesAsync();  

            return Ok(produtoExiste);
        }
        [HttpDelete("{id}")]

        public async Task<ActionResult> Delete(int id)
        {
            var produto = await _context.Produtos.FindAsync(id);

            if (produto == null)
            {
                return NotFound();
            }
            produto.Ativo = false;
            produto.DataAtualizacao = DateTime.UtcNow;

            await _context.SaveChangesAsync();

            return NoContent();

        }
    }
}
