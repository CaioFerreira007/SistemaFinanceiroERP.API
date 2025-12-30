using AutoMapper;
using FluentValidation;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SistemaFinanceiroERP.API.DTOs.Produto;
using SistemaFinanceiroERP.Domain.Entities;
using SistemaFinanceiroERP.Infrastructure.Data;

namespace SistemaFinanceiroERP.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProdutoController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;
        private readonly IValidator<ProdutoCreateDto> _createValidator;
        private readonly IValidator<ProdutoUpdateDto> _updateValidator;

        public ProdutoController(AppDbContext context, IMapper mapper, IValidator<ProdutoCreateDto> createValidator, IValidator<ProdutoUpdateDto> updateValidator)
        {
            _context = context;
            _mapper = mapper;
            _createValidator = createValidator;
            _updateValidator = updateValidator;
        }

        [HttpPost]

        public async Task<ActionResult<ProdutoResponseDto>> Create([FromBody] ProdutoCreateDto dto)
        {
            var validationResult = await _createValidator.ValidateAsync(dto);
            if (!validationResult.IsValid)
            {
                return BadRequest(validationResult.Errors.Select(e => e.ErrorMessage));
            }

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

            var validationResult = await _updateValidator.ValidateAsync(dto);
            if (!validationResult.IsValid)
            {
                return BadRequest(validationResult.Errors.Select(e => e.ErrorMessage));
            }

            var produtoExiste = await _context.Produtos.FindAsync(id);

            if (produtoExiste == null)
            {
                return NotFound();
            }

            var produtoEmpresaExiste = await _context.Produtos.FindAsync(produtoExiste.EmpresaId);

            if (produtoEmpresaExiste == null || !produtoEmpresaExiste.Ativo)
            {
                return BadRequest("Empresa do produto não encontrada ou inativa");
            }

            if (id != dto.Id)
            {
                return BadRequest("O id da url não corresponde ao id do produto");
            }
            var empresa = await _context.Empresas.FindAsync(dto.EmpresaId);
            if (empresa == null || !empresa.Ativo)
            {
                return BadRequest("Empresa não encontrada ou inativa.");
            }
            produtoExiste.DataAtualizacao = DateTime.UtcNow;


            _mapper.Map(dto, produtoExiste);
            await _context.SaveChangesAsync();
            var response = _mapper.Map<ProdutoResponseDto>(produtoExiste);

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
