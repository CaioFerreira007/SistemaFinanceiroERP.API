using AutoMapper;
using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SistemaFinanceiroERP.Application.DTOs.Empresa;
using SistemaFinanceiroERP.Domain.Interfaces;

namespace SistemaFinanceiroERP.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class EmpresaController : ControllerBase
    {
        private readonly IEmpresaRepository _repository;
        private readonly IMapper _mapper;
        private readonly IValidator<EmpresaCreateDto> _createValidator;
        private readonly IValidator<EmpresaUpdateDto> _updateValidator;
        private readonly ITenantProvider _tenantProvider;

        public EmpresaController(
            IEmpresaRepository repository,
            IMapper mapper,
            IValidator<EmpresaCreateDto> createValidator,
            IValidator<EmpresaUpdateDto> updateValidator,
            ITenantProvider tenantProvider)
        {
            _repository = repository;
            _mapper = mapper;
            _createValidator = createValidator;
            _updateValidator = updateValidator;
            _tenantProvider = tenantProvider;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<EmpresaResponseDto>>> GetAll()
        {
            // Query Filter retorna SOMENTE a empresa do usuário logado
            var empresas = await _repository.GetAllAsync();
            var response = _mapper.Map<List<EmpresaResponseDto>>(empresas);
            return Ok(response);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<EmpresaResponseDto>> GetById(int id)
        {
            // Query Filter garante que só retorna se id == empresaId do token
            var empresa = await _repository.GetByIdAsync(id);
            if (empresa == null)
            {
                return NotFound();
            }

            var response = _mapper.Map<EmpresaResponseDto>(empresa);
            return Ok(response);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<EmpresaResponseDto>> Update(int id, [FromBody] EmpresaUpdateDto dto)
        {
            var validationResult = await _updateValidator.ValidateAsync(dto);
            if (!validationResult.IsValid)
            {
                return BadRequest(validationResult.Errors.Select(e => e.ErrorMessage));
            }

            if (id != dto.Id)
            {
                return BadRequest("ID da URL não corresponde ao ID da empresa");
            }

            // Query Filter garante que só busca a própria empresa
            var empresaExistente = await _repository.GetByIdAsync(id);
            if (empresaExistente == null)
            {
                return NotFound();
            }

            _mapper.Map(dto, empresaExistente);
            empresaExistente.DataAtualizacao = DateTime.UtcNow;

            await _repository.UpdateAsync(empresaExistente);
            await _repository.SaveChangesAsync();
            var response = _mapper.Map<EmpresaResponseDto>(empresaExistente);
            return Ok(response);
        }
    }
}