using AutoMapper;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using SistemaFinanceiroERP.Application.DTOs.MovimentacaoEstoque;
using SistemaFinanceiroERP.Domain.Entities;
using SistemaFinanceiroERP.Domain.Interfaces;
using System.Security.Claims;

namespace SistemaFinanceiroERP.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MovimentacaoEstoqueController : ControllerBase
    {
        private readonly IMovimentacaoEstoqueRepository _repository;
        private readonly IMapper _mapper;
        private readonly IValidator<MovimentacaoEstoqueCreateDto> _createValidator;
        private readonly ITenantProvider _tenantProvider;
        public MovimentacaoEstoqueController(
            IMovimentacaoEstoqueRepository repository,
            IMapper mapper,
            IValidator<MovimentacaoEstoqueCreateDto> createValidator,
            ITenantProvider tenantProvider)
        {
            _repository = repository;
            _mapper = mapper;
            _createValidator = createValidator;
            _tenantProvider = tenantProvider;
        }

        [HttpPost]

        public async Task<ActionResult<MovimentacaoEstoqueResponseDto>> Create([FromBody] MovimentacaoEstoqueCreateDto dto)
        {
            var validationResult = await _createValidator.ValidateAsync(dto);
            if (!validationResult.IsValid)
            {
                return BadRequest(validationResult.Errors.Select(e => e.ErrorMessage));
            }
            var movimentacaoEstoqueNova = _mapper.Map<MovimentacaoEstoque>(dto);
            movimentacaoEstoqueNova.EmpresaId = _tenantProvider.GetEmpresaId();
            movimentacaoEstoqueNova.DataCriacao = DateTime.UtcNow;
            movimentacaoEstoqueNova.Ativo = true;
            var usuarioIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(usuarioIdClaim))
                return Unauthorized("Usuário não identificado");

            movimentacaoEstoqueNova.UsuarioId = int.Parse(usuarioIdClaim);
            try
            {
                await _repository.RegistrarMovimentacaoAsync(movimentacaoEstoqueNova);

            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
            var response = _mapper.Map<MovimentacaoEstoqueResponseDto>(movimentacaoEstoqueNova);

            return CreatedAtAction(nameof(GetById), new { id = movimentacaoEstoqueNova.Id }, response);
        }


        [HttpGet]
        public async Task<ActionResult<IEnumerable<MovimentacaoEstoqueResponseDto>>> GetAll()
        {
            var movimentacoesEstoque = await _repository.GetAllAsync();
            var response = _mapper.Map<IEnumerable<MovimentacaoEstoqueResponseDto>>(movimentacoesEstoque);
            return Ok(response);
        }



        [HttpGet("{id}")]

        public async Task<ActionResult<MovimentacaoEstoqueResponseDto>> GetById(int id)
        {
            var movimentacaoEstoque = await _repository.GetByIdAsync(id);
            if (movimentacaoEstoque == null)
            {
                return NotFound();
            }
            var response = _mapper.Map<MovimentacaoEstoqueResponseDto>(movimentacaoEstoque);
            return Ok(response);
        }
        [HttpGet("produto/{id}")]
        public async Task<ActionResult<IEnumerable<MovimentacaoEstoqueResponseDto>>> GetByProduto(int id)
        {
            var movimentacoesEstoque = await _repository.GetByProdutoAsync(id);
            var response = _mapper.Map<IEnumerable<MovimentacaoEstoqueResponseDto>>(movimentacoesEstoque);
            return Ok(response);
        }

        [HttpGet("local/{id}")]
        public async Task<ActionResult<IEnumerable<MovimentacaoEstoqueResponseDto>>> GetByLocalEstoque(int id)
        {
            var movimentacoesEstoque = await _repository.GetByLocalEstoqueAsync(id);
            var response = _mapper.Map<IEnumerable<MovimentacaoEstoqueResponseDto>>(movimentacoesEstoque);
            return Ok(response);
        }

    }
}
