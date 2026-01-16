using AutoMapper;
using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SistemaFinanceiroERP.Application.DTOs.Produto;
using SistemaFinanceiroERP.Domain.Entities;
using SistemaFinanceiroERP.Domain.Interfaces;
using SistemaFinanceiroERP.Infrastructure.Data;

namespace SistemaFinanceiroERP.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ProdutoController : ControllerBase
    {
        private readonly IProdutoRepository _repository;
        private readonly IMapper _mapper;
        private readonly IValidator<ProdutoCreateDto> _createValidator;
        private readonly IValidator<ProdutoUpdateDto> _updateValidator;
        private readonly ITenantProvider _tenantProvider;
        private readonly AppDbContext _context;

        public ProdutoController(
            AppDbContext context,
            IProdutoRepository repository,
            IMapper mapper,
            IValidator<ProdutoCreateDto> createValidator,
            IValidator<ProdutoUpdateDto> updateValidator,
            ITenantProvider tenantProvider)
        {
            _context = context;              
            _repository = repository;
            _mapper = mapper;
            _createValidator = createValidator;
            _updateValidator = updateValidator;
            _tenantProvider = tenantProvider;
        }

        private async Task<string?> GetLocalEstoqueNomeAsync(int? localEstoqueId)
        {
            if (!localEstoqueId.HasValue)
                return null;

            return await _context.LocaisEstoque
                .Where(l => l.Id == localEstoqueId.Value)
                .Select(l => l.LocalNome)
                .FirstOrDefaultAsync();
        }

        [HttpPost]
        public async Task<ActionResult<ProdutoResponseDto>> Create([FromBody] ProdutoCreateDto dto)
        {
            var validationResult = await _createValidator.ValidateAsync(dto);
            if (!validationResult.IsValid)
            {
                return BadRequest(validationResult.Errors.Select(e => e.ErrorMessage));
            }

            var produtoNovo = _mapper.Map<Produto>(dto);
            produtoNovo.EmpresaId = _tenantProvider.GetEmpresaId();
            produtoNovo.DataCriacao = DateTime.UtcNow;
            produtoNovo.Ativo = true;

            await _repository.AddAsync(produtoNovo);
            await _repository.SaveChangesAsync();

            var response = _mapper.Map<ProdutoResponseDto>(produtoNovo);
            response.LocalEstoqueNome = await GetLocalEstoqueNomeAsync(produtoNovo.LocalEstoqueId);  

            return CreatedAtAction(nameof(GetById), new { id = produtoNovo.Id }, response);
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProdutoResponseDto>>> GetAll()
        {
            var produtos = await _repository.GetAllAsync();
            var response = _mapper.Map<List<ProdutoResponseDto>>(produtos);

            foreach (var produtoDto in response)
            {
                var produto = produtos.FirstOrDefault(p => p.Id == produtoDto.Id);
                if (produto != null)
                {
                    produtoDto.LocalEstoqueNome = await GetLocalEstoqueNomeAsync(produto.LocalEstoqueId);
                }
            }

            return Ok(response);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ProdutoResponseDto>> GetById(int id)
        {
            var produto = await _repository.GetByIdAsync(id);

            if (produto == null)
                return NotFound();

            var response = _mapper.Map<ProdutoResponseDto>(produto);
            response.LocalEstoqueNome = await GetLocalEstoqueNomeAsync(produto.LocalEstoqueId);

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

            if (id != dto.Id)
            {
                return BadRequest("O id da URL não corresponde ao id do produto");
            }

            var produtoExiste = await _repository.GetByIdAsync(id);
            if (produtoExiste == null)
            {
                return NotFound();
            }

            _mapper.Map(dto, produtoExiste);
            produtoExiste.EmpresaId = _tenantProvider.GetEmpresaId();
            produtoExiste.DataAtualizacao = DateTime.UtcNow;

            await _repository.UpdateAsync(produtoExiste);
            await _repository.SaveChangesAsync();

            var response = _mapper.Map<ProdutoResponseDto>(produtoExiste);
            response.LocalEstoqueNome = await GetLocalEstoqueNomeAsync(produtoExiste.LocalEstoqueId);  

            return Ok(response);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            var produto = await _repository.GetByIdAsync(id);
            if (produto == null)
            {
                return NotFound();
            }

            produto.Ativo = false;
            produto.DataAtualizacao = DateTime.UtcNow;
            await _repository.UpdateAsync(produto);
            await _repository.SaveChangesAsync();

            return NoContent();
        }
    }
}