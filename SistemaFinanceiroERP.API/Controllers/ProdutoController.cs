using AutoMapper;
using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SistemaFinanceiroERP.Application.DTOs.Produto;
using SistemaFinanceiroERP.Domain.Interfaces;
using SistemaFinanceiroERP.Domain.Entities;

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

        public ProdutoController(
            IProdutoRepository repository,
            IMapper mapper,
            IValidator<ProdutoCreateDto> createValidator,
            IValidator<ProdutoUpdateDto> updateValidator,
            ITenantProvider tenantProvider)
        {
            _repository = repository;
            _mapper = mapper;
            _createValidator = createValidator;
            _updateValidator = updateValidator;
            _tenantProvider = tenantProvider;
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
            return CreatedAtAction(nameof(GetById), new { id = produtoNovo.Id }, response);
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProdutoResponseDto>>> GetAll()
        {
            // Query Filter aplica filtro automático por EmpresaId
            var produtos = await _repository.GetAllAsync();
            var response = _mapper.Map<List<ProdutoResponseDto>>(produtos);
            return Ok(response);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ProdutoResponseDto>> GetById(int id)
        {
            // Query Filter aplica filtro automático por EmpresaId
            var produto = await _repository.GetByIdAsync(id);
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

            if (id != dto.Id)
            {
                return BadRequest("O id da URL não corresponde ao id do produto");
            }

            // Query Filter garante que só busca produtos da empresa
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
            return Ok(response);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            // Query Filter garante que só busca produtos da empresa
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