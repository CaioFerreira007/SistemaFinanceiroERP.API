using AutoMapper;
using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SistemaFinanceiroERP.Application.DTOs.MovimentacaoEstoque;
using SistemaFinanceiroERP.Domain.Entities;
using SistemaFinanceiroERP.Domain.Interfaces;

namespace SistemaFinanceiroERP.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
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
                return BadRequest(validationResult.Errors.Select(e => e.ErrorMessage));

            var mov = _mapper.Map<MovimentacaoEstoque>(dto);
            mov.EmpresaId = _tenantProvider.GetEmpresaId();
            mov.UsuarioId = _tenantProvider.GetUsuarioId();

            try
            {
                await _repository.RegistrarMovimentacaoAsync(mov);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }

            var response = _mapper.Map<MovimentacaoEstoqueResponseDto>(mov);
            return CreatedAtAction(nameof(GetById), new { id = mov.Id }, response);
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<MovimentacaoEstoqueResponseDto>>> GetAll()
        {
            var movs = await _repository.GetAllAsync();
            var response = _mapper.Map<IEnumerable<MovimentacaoEstoqueResponseDto>>(movs);
            return Ok(response);
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<MovimentacaoEstoqueResponseDto>> GetById(int id)
        {
            var mov = await _repository.GetByIdAsync(id);
            if (mov == null) return NotFound();

            var response = _mapper.Map<MovimentacaoEstoqueResponseDto>(mov);
            return Ok(response);
        }

     
        [HttpGet("produto/{produtoId:int}")]  
        public async Task<ActionResult<IEnumerable<MovimentacaoEstoqueResponseDto>>> GetByProduto(int produtoId)  // ✅ CORRIGIDO
        {
            var movs = await _repository.GetByProdutoAsync(produtoId); 
            var response = _mapper.Map<IEnumerable<MovimentacaoEstoqueResponseDto>>(movs);
            return Ok(response);
        }

      
        [HttpGet("local/{localEstoqueId:int}")]  
        public async Task<ActionResult<IEnumerable<MovimentacaoEstoqueResponseDto>>> GetByLocalEstoque(int localEstoqueId)  // ✅ CORRIGIDO
        {
            var movs = await _repository.GetByLocalEstoqueAsync(localEstoqueId); 
            var response = _mapper.Map<IEnumerable<MovimentacaoEstoqueResponseDto>>(movs);
            return Ok(response);
        }
    }
}