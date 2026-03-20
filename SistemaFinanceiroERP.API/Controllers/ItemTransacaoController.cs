using AutoMapper;                                 
using Microsoft.AspNetCore.Authorization;                  
using Microsoft.AspNetCore.Mvc;                           
using SistemaFinanceiroERP.Application.DTOs.ItemTransacao;
using SistemaFinanceiroERP.Domain.Interfaces;

namespace SistemaFinanceiroERP.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ItemTransacaoController : ControllerBase
    {
        private readonly IItemTransacaoRepository _repository;
        private readonly IMapper _mapper;

        public ItemTransacaoController(IItemTransacaoRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ItemTransacaoResponseDto>> GetById(int id)
        {
            var item = await _repository.GetByIdAsync(id);

            if (item == null)
                return NotFound();

            var response = _mapper.Map<ItemTransacaoResponseDto>(item);

            return Ok(response);
        }

        [HttpGet("transacao/{transacaoId}")]
        public async Task<ActionResult<IEnumerable<ItemTransacaoResponseDto>>> GetByTransacaoId(int transacaoId)
        {
            var itens = await _repository.GetByTransacaoIdAsync(transacaoId);

            var response = _mapper.Map<IEnumerable<ItemTransacaoResponseDto>>(itens);

            return Ok(response);
        }

        [HttpGet("produto/{produtoId}")]
        public async Task<ActionResult<IEnumerable<ItemTransacaoResponseDto>>> GetByProdutoId(int produtoId)
        {
            var itens = await _repository.GetByProdutoIdAsync(produtoId);

            var response = _mapper.Map<IEnumerable<ItemTransacaoResponseDto>>(itens);

            return Ok(response);
        }
    }
}