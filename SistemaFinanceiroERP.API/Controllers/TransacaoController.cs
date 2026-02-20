using AutoMapper;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using SistemaFinanceiroERP.Application.DTOs.Transacao;
using SistemaFinanceiroERP.Domain.Enums;
using SistemaFinanceiroERP.Domain.Interfaces;

namespace SistemaFinanceiroERP.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TransacaoController : ControllerBase
    {
        private readonly ITransacaoRepository _transacaoRepository;
        private readonly IMapper _mapper;
        private readonly ITenantProvider _tenantProvider;
        private readonly IValidator<TransacaoCreateDto> _validadorCreate;

        public TransacaoController(ITransacaoRepository transacaoRepository, IMapper mapper, ITenantProvider tenantProvider, IValidator<TransacaoCreateDto> validatorCreate)
        {
            _transacaoRepository = transacaoRepository;
            _mapper = mapper;
            _tenantProvider = tenantProvider;
            _validadorCreate = validatorCreate;
        }

        [HttpPost]
        public async Task<ActionResult<TransacaoResponseDto>> Create([FromBody] TransacaoCreateDto dto)
        {
            var validationResult = await _validadorCreate.ValidateAsync(dto);
            if (!validationResult.IsValid)
                return BadRequest(validationResult.Errors.Select(e => e.ErrorMessage));
            var transacao = _mapper.Map<Domain.Entities.Transacao>(dto);

            transacao.UsuarioId = _tenantProvider.GetUsuarioId();
            try
            {
                var transacaoCriada = await _transacaoRepository.RegistrarTransacaoAsync(transacao);
                var response = _mapper.Map<TransacaoResponseDto>(transacaoCriada);
                return CreatedAtAction(nameof(GetById), new { id = transacaoCriada.Id }, response);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }


        }
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TransacaoResponseDto>>> GetAll()
        {
            var transacoes = await _transacaoRepository.GetAllAsync();
            var response = _mapper.Map<IEnumerable<TransacaoResponseDto>>(transacoes);
            return Ok(response);

        }
        [HttpGet("{id}")]
        public async Task<ActionResult<TransacaoResponseDto>> GetById(int id)
        {
            var transacao = await _transacaoRepository.GetByIdAsync(id);
            if (transacao == null)
                return NotFound();
            var response = _mapper.Map<TransacaoResponseDto>(transacao);
            return Ok(response);
        }



        [HttpGet("{id}/itens")]
        public async Task<ActionResult<TransacaoResponseDto>> GetItensByTransacaoId(int id)
        {
            var transacao = await _transacaoRepository.GetTransacaoComItensAsync(id);
            if (transacao is null)
                return NotFound();
            var response = _mapper.Map<TransacaoResponseDto>(transacao);
            return Ok(response);
        }


        [HttpGet("como-vendedor")]
        public async Task<ActionResult<IEnumerable<TransacaoResponseDto>>> GetTransacoesComoVendedor()
        {
            var transacoes = await _transacaoRepository.GetTransacoesComoVendedorAsync();
            var response = _mapper.Map<IEnumerable<TransacaoResponseDto>>(transacoes);
            return Ok(response);

        }
        [HttpGet("como-comprador")]

        public async Task<ActionResult<IEnumerable<TransacaoResponseDto>>> GetTransacoesComoComprador()
        {
            var transacoes = await _transacaoRepository.GetTransacoesComoCompradorAsync();
            var response = _mapper.Map<IEnumerable<TransacaoResponseDto>>(transacoes);
            return Ok(response);
        }


        [HttpPut("{id}/status")]
        public async Task<ActionResult> UpdateStatus(int id, [FromBody] StatusTransacao novoStatus)
        {
            try
            {
                await _transacaoRepository.AtualizarStatusAsync(id, novoStatus);
                return NoContent();
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
