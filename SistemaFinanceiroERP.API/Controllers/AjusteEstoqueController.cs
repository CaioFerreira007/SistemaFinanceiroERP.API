using AutoMapper;
using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SistemaFinanceiroERP.Application.DTOs.AjusteEstoque;
using SistemaFinanceiroERP.Domain.Entities;
using SistemaFinanceiroERP.Domain.Interfaces;
using System.Security.Claims;

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

        public AjusteEstoqueController(IAjusteEstoqueRepository repository,
            IMapper mapper, IValidator<AjusteEstoqueCreateDto> createValidator,
            ITenantProvider tenantProvider)
        {
            _repository = repository;
            _mapper = mapper;
            _createValidator = createValidator;
            _tenantProvider = tenantProvider;
        }
        [HttpPost]

        public async Task<ActionResult<AjusteEstoqueCreateDto>> Create([FromBody] AjusteEstoqueCreateDto dto)
        {
            var validationResult = await _createValidator.ValidateAsync(dto);
            if (!validationResult.IsValid)
            {
                return BadRequest(validationResult.Errors.Select(e => e.ErrorMessage));
            }
            var ajusteEstoqueNovo = _mapper.Map<AjusteEstoque>(dto);
            ajusteEstoqueNovo.EmpresaId = _tenantProvider.GetEmpresaId();
            ajusteEstoqueNovo.DataDoAjuste = DateTime.UtcNow;

            var usuarioClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(usuarioClaim))
            {
                return Unauthorized("Usuário não identificado!");
            }
            ajusteEstoqueNovo.UsuarioId = int.Parse(usuarioClaim);
            try
            {
                var ajusteCriado = await _repository.RegistrarAjusteEstoqueAsync(ajusteEstoqueNovo);


                var response = _mapper.Map<AjusteEstoqueResponseDto>(ajusteCriado);

                return CreatedAtAction(nameof(GetById), new { id = ajusteCriado.Id }, response);
            } catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]

        public async Task<ActionResult<IEnumerable<AjusteEstoqueResponseDto>>> GetAll()
        {
            var ajustesEstoque = await _repository.GetAllAsync();
            var response = _mapper.Map<IEnumerable<AjusteEstoqueResponseDto>>(ajustesEstoque);
            return Ok(response);
        }


        [HttpGet("{id}")]

        public async Task<ActionResult<AjusteEstoqueResponseDto>> GetById(int id)
        {
            var ajusteEstoque = await _repository.GetByIdAsync(id);
            if (ajusteEstoque == null)
            {
                return NotFound();
            }
            var response = _mapper.Map<AjusteEstoqueResponseDto>(ajusteEstoque);
            return Ok(response);
        }
        [HttpGet("produto/{produtoId}")]
        public async Task<ActionResult<IEnumerable<AjusteEstoqueResponseDto>>> GetByProduto(int produtoId)
        {
            var ajustesEstoque = await _repository.GetByProdutoIdAsync(produtoId);
            var response = _mapper.Map<IEnumerable<AjusteEstoqueResponseDto>>(ajustesEstoque);
            return Ok(response);
        }

        [HttpGet("localEstoque/{localEstoqueId}")]
        public async Task<ActionResult<IEnumerable<AjusteEstoqueResponseDto>>> GetByLocalEstoque(int localEstoqueId)
        {
            var ajustesEstoque = await _repository.GetByLocalEstoqueIdAsync(localEstoqueId);
            var response = _mapper.Map<IEnumerable<AjusteEstoqueResponseDto>>(ajustesEstoque);
            return Ok(response);
        }
    }
}

