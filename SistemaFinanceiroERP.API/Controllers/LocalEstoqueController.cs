using AutoMapper;
using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SistemaFinanceiroERP.Application.DTOs.LocalEstoque;
using SistemaFinanceiroERP.Domain.Entities;
using SistemaFinanceiroERP.Domain.Interfaces;

namespace SistemaFinanceiroERP.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class LocalEstoqueController : ControllerBase
    {
        private readonly ILocalEstoqueRepository _repository;
        private readonly IMapper _mapper;
        private readonly IValidator<LocalEstoqueCreateDto> _createValidator;
        private readonly IValidator<LocalEstoqueUpdateDto> _updateValidator;
        private readonly ITenantProvider _tenantProvider;

        public LocalEstoqueController(
            ILocalEstoqueRepository repository,
            IMapper mapper,
            IValidator<LocalEstoqueCreateDto> createValidator,
            IValidator<LocalEstoqueUpdateDto> updateValidator,
            ITenantProvider tenantProvider)
        {
            _repository = repository;
            _mapper = mapper;
            _createValidator = createValidator;
            _updateValidator = updateValidator;
            _tenantProvider = tenantProvider;
        }

        [HttpPost]
        public async Task<ActionResult<LocalEstoqueResponseDto>> Create([FromBody] LocalEstoqueCreateDto dto)
        {
            var validationResult = await _createValidator.ValidateAsync(dto);
            if (!validationResult.IsValid)
                return BadRequest(validationResult.Errors.Select(e => e.ErrorMessage));

            var localNovo = _mapper.Map<LocalEstoque>(dto);
            localNovo.EmpresaId = _tenantProvider.GetEmpresaId();

            await _repository.AddAsync(localNovo);
            await _repository.SaveChangesAsync();

            var response = _mapper.Map<LocalEstoqueResponseDto>(localNovo);
            return CreatedAtAction(nameof(GetById), new { id = localNovo.Id }, response);
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<LocalEstoqueResponseDto>>> GetAll()
        {
            var locais = await _repository.GetAllAsync();
            var response = _mapper.Map<List<LocalEstoqueResponseDto>>(locais);
            return Ok(response);
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<LocalEstoqueResponseDto>> GetById(int id)
        {
            var local = await _repository.GetByIdAsync(id);
            if (local == null) return NotFound();

            var response = _mapper.Map<LocalEstoqueResponseDto>(local);
            return Ok(response);
        }

        [HttpPut("{id:int}")]
        public async Task<ActionResult<LocalEstoqueResponseDto>> Update(int id, [FromBody] LocalEstoqueUpdateDto dto)
        {
            var validationResult = await _updateValidator.ValidateAsync(dto);
            if (!validationResult.IsValid)
                return BadRequest(validationResult.Errors.Select(e => e.ErrorMessage));

            if (id != dto.Id)
                return BadRequest("O id da URL não corresponde ao id do local");

            var existente = await _repository.GetByIdAsync(id);
            if (existente == null) return NotFound();

            _mapper.Map(dto, existente);

            await _repository.UpdateAsync(existente);
            await _repository.SaveChangesAsync();

            var response = _mapper.Map<LocalEstoqueResponseDto>(existente);
            return Ok(response);
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            var local = await _repository.GetByIdAsync(id);
            if (local == null) return NotFound();

            // ✅ Validação: não permitir deletar LocalEstoque com Produto associado
            var temProdutos = await _repository.HasProdutosAssociadosAsync(id);
            if (temProdutos)
                return BadRequest("Não é possível deletar este Local de Estoque: existem produtos associados a ele.");

            local.Ativo = false;
            await _repository.UpdateAsync(local);
            await _repository.SaveChangesAsync();

            return NoContent();
        }
    }
}
