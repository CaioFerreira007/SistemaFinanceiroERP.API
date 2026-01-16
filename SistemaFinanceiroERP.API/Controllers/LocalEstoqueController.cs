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
            {
                return BadRequest(validationResult.Errors.Select(e => e.ErrorMessage));
            }

            var localEstoqueNovo = _mapper.Map<LocalEstoque>(dto);
            localEstoqueNovo.EmpresaId = _tenantProvider.GetEmpresaId();
            localEstoqueNovo.DataCriacao = DateTime.UtcNow;
            localEstoqueNovo.Ativo = true;

            await _repository.AddAsync(localEstoqueNovo);
            await _repository.SaveChangesAsync();

            var response = _mapper.Map<LocalEstoqueResponseDto>(localEstoqueNovo);
            return CreatedAtAction(nameof(GetById), new { id = localEstoqueNovo.Id }, response);
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<LocalEstoqueResponseDto>>> GetAll()
        {
            var locaisEstoque = await _repository.GetAllAsync();
            var response = _mapper.Map<List<LocalEstoqueResponseDto>>(locaisEstoque);
            return Ok(response);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<LocalEstoqueResponseDto>> GetById(int id)
        {
            var localEstoque = await _repository.GetByIdAsync(id);
            if (localEstoque == null)
            {
                return NotFound();
            }
            var response = _mapper.Map<LocalEstoqueResponseDto>(localEstoque);
            return Ok(response);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<LocalEstoqueResponseDto>> Update(int id, [FromBody] LocalEstoqueUpdateDto dto)
        {
            var validationResult = await _updateValidator.ValidateAsync(dto);
            if (!validationResult.IsValid)
            {
                return BadRequest(validationResult.Errors.Select(e => e.ErrorMessage));
            }

            if (id != dto.Id)
            {
                return BadRequest("O id da URL nao corresponde ao id do local de estoque");
            }

            var localEstoqueExistente = await _repository.GetByIdAsync(id);
            if (localEstoqueExistente == null)
            {
                return NotFound();
            }

            _mapper.Map(dto, localEstoqueExistente);
            localEstoqueExistente.DataAtualizacao = DateTime.UtcNow;

            await _repository.UpdateAsync(localEstoqueExistente);
            await _repository.SaveChangesAsync();

            var response = _mapper.Map<LocalEstoqueResponseDto>(localEstoqueExistente);
            return Ok(response);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var localEstoque = await _repository.GetByIdAsync(id);
            if (localEstoque == null)
            {
                return NotFound();
            }

            var possuiProdutos = await _repository.HasProdutosAssociadosAsync(id);
            if (possuiProdutos)
            {
                return BadRequest("Nao e permitido deletar um local de estoque que possui produtos associados.");
            }

            localEstoque.Ativo = false;
            localEstoque.DataAtualizacao = DateTime.UtcNow;
            await _repository.UpdateAsync(localEstoque);
            await _repository.SaveChangesAsync();
            return NoContent();
        }
    }
}
