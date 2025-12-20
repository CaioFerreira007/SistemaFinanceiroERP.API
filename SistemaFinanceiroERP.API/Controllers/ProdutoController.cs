using Microsoft.AspNetCore.Http;
using SistemaFinanceiroERP.Domain.Entities;
using SistemaFinanceiroERP.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

using Microsoft.AspNetCore.Mvc;
using SistemaFinanceiroERP.API.DTOs.Produto;
using AutoMapper;

namespace SistemaFinanceiroERP.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProdutoController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;

        public ProdutoController(AppDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        [HttpPost]

        public async Task<ActionResult<ProdutoResponseDto>> Create([FromBody] ProdutoCreateDto dto)
        {

            var empresa = await _context.Empresas.FindAsync(dto.EmpresaId);
            if (empresa == null || !empresa.Ativo)
            {
                return BadRequest("Empresa não encontrada ou inativa");

            }
            var produtoNovo = _mapper.Map<Produto>(dto);
            produtoNovo.DataCriacao = DateTime.UtcNow;
            produtoNovo.Ativo = true;

            _context.Produtos.Add(produtoNovo);

            await _context.SaveChangesAsync();

            var response = _mapper.Map<ProdutoResponseDto>(produtoNovo);


            return CreatedAtAction(nameof(GetById), new { id = produtoNovo.Id }, response);
        }

        [HttpGet]

        public async Task<ActionResult<IEnumerable<ProdutoResponseDto>>> GetAll()
        {
            var produtos = await _context.Produtos.ToListAsync();

            var response = _mapper.Map<List<ProdutoResponseDto>>(produtos);

            return Ok(response);
        }

        [HttpGet("{id}")]

        public async Task<ActionResult<ProdutoResponseDto>> GetById(int id)
        {
            var produto = await _context.Produtos.FindAsync(id);

            if (produto == null)
            {
                return NotFound();
            }

            var response = _mapper.Map<ProdutoResponseDto>(produto);

            return Ok(response);
        }

        [HttpPut("{id}")]

        public async Task<ActionResult<ProdutoResponseDto>> Update(int id, [FromBody] ProdutoUpdateDto dto)
        {


                var produtoExiste = await _context.Produtos.FindAsync(id);

             if (produtoExiste == null)
             {
                return NotFound();
             }

                 if (id != dto.Id)
                    {
                             return BadRequest("O id da url não corresponde ao id do produto");
                    }

             _mapper.Map(dto, produtoExiste);

                   produtoExiste.DataAtualizacao = DateTime.UtcNow;

                    var response = _mapper.Map<ProdutoResponseDto>(produtoExiste);

                     await _context.SaveChangesAsync();

                         return Ok(response);
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
