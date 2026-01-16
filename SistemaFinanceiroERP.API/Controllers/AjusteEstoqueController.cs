using AutoMapper;
using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SistemaFinanceiroERP.Application.DTOs.AjusteEstoque;
using SistemaFinanceiroERP.Domain.Entities;
using SistemaFinanceiroERP.Domain.Interfaces;

namespace SistemaFinanceiroERP.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class AjusteEstoqueController : ControllerBase
    {
        private readonly IAjusteEstoqueRepository _repository;
        private readonly IMapper _mapper;
        private readonly IValidator<AjusteEstoqueCreateDto> _createValidator;
        private readonly ITenantProvider _tenantProvider;

        public AjusteEstoqueController(
            IAjusteEstoqueRepository repository,
            IMapper mapper,
            IValidator<AjusteEstoqueCreateDto> createValidator,
            ITenantProvider tenantProvider)
        {
            _repository = repository;
            _mapper = mapper;
            _createValidator = createValidator;
            _tenantProvider = tenantProvider;
        }

        [HttpPost]
        public async Task<ActionResult<AjusteEstoqueResponseDto>> Create([FromBody] AjusteEstoqueCreateDto dto)
        {
            var validationResult = await _createValidator.ValidateAsync(dto);
            if (!validationResult.IsValid)
                return BadRequest(validationResult.Errors.Select(e => e.ErrorMessage));

            var ajusteNovo = _mapper.Map<AjusteEstoque>(dto);
            ajusteNovo.EmpresaId = _tenantProvider.GetEmpresaId();
            ajusteNovo.UsuarioId = _tenantProvider.GetUsuarioId();

            try
            {
                var ajusteCriado = await _repository.RegistrarAjusteEstoqueAsync(ajusteNovo);
                var response = _mapper.Map<AjusteEstoqueResponseDto>(ajusteCriado);
                return CreatedAtAction(nameof(GetById), new { id = ajusteCriado.Id }, response);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<AjusteEstoqueResponseDto>>> GetAll()
        {
            var ajustes = await _repository.GetAllAsync();
            var response = _mapper.Map<IEnumerable<AjusteEstoqueResponseDto>>(ajustes);
            return Ok(response);
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<AjusteEstoqueResponseDto>> GetById(int id)
        {
            var ajuste = await _repository.GetByIdAsync(id);
            if (ajuste == null) return NotFound();

            var response = _mapper.Map<AjusteEstoqueResponseDto>(ajuste);
            return Ok(response);
        }

        [HttpGet("produto/{produtoId:int}")]
        public async Task<ActionResult<IEnumerable<AjusteEstoqueResponseDto>>> GetByProduto(int produtoId)
        {
            var ajustes = await _repository.GetByProdutoIdAsync(produtoId);
            var response = _mapper.Map<IEnumerable<AjusteEstoqueResponseDto>>(ajustes);
            return Ok(response);
        }

        [HttpGet("localEstoque/{localEstoqueId:int}")]
        public async Task<ActionResult<IEnumerable<AjusteEstoqueResponseDto>>> GetByLocalEstoque(int localEstoqueId)
        {
            var ajustes = await _repository.GetByLocalEstoqueIdAsync(localEstoqueId);
            var response = _mapper.Map<IEnumerable<AjusteEstoqueResponseDto>>(ajustes);
            return Ok(response);
        }
    }
}
