using Microsoft.AspNetCore.Mvc;
using SistemaFinanceiroERP.Domain.Entities;
using SistemaFinanceiroERP.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using AutoMapper;
using SistemaFinanceiroERP.API.DTOs.Empresa;

namespace SistemaFinanceiroERP.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmpresaController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;


        public EmpresaController(AppDbContext context, IMapper mapper)  
        {
            _context = context;
            _mapper = mapper;
        }

        [HttpPost]

        public async Task<ActionResult<EmpresaResponseDto>> Create([FromBody] EmpresaCreateDto dto)
        {
            var empresa = _mapper.Map<Empresa>(dto);
            empresa.DataCriacao = DateTime.UtcNow;
            empresa.Ativo = true;
            _context.Empresas.Add(empresa);
            var response = _mapper.Map<EmpresaResponseDto>(empresa);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetById), new { id = empresa.Id }, response);
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<EmpresaResponseDto>>> GetAll()
        {
            var empresas = await _context.Empresas.ToListAsync();
            var response = _mapper.Map<IEnumerable<EmpresaResponseDto>>(empresas);
            return Ok(response);
        }

        [HttpGet("{id}")]

        public async Task <ActionResult<EmpresaResponseDto>> GetById(int id)
        {
            var empresa = await _context.Empresas.FindAsync(id);
            if(empresa == null)
            {
                return NotFound();
            }
            var response = _mapper.Map<EmpresaResponseDto>(empresa);
            return Ok(response);
        }

        [HttpPut("{id}")]

        public async Task<ActionResult<EmpresaResponseDto>> Update(int id, [FromBody]EmpresaUpdateDto dto)
        {
            if (id != dto.Id)
            {
                return BadRequest("ID da URL não corresponde ao ID da empresa");
            }

            var empresaExistente = await _context.Empresas.FindAsync(id);

            if (empresaExistente == null)
            {
                return NotFound();
            }

            _mapper.Map(dto,empresaExistente);

            empresaExistente.DataAtualizacao = DateTime.UtcNow;
            await _context.SaveChangesAsync();
            var response = _mapper.Map<EmpresaResponseDto>(empresaExistente);


            return Ok(response);
        }

        [HttpDelete("{id}")]

        public async Task<ActionResult> Delete(int id)
        {
          var empresa = await _context.Empresas.FindAsync(id);

            if (empresa == null)
            {
                return NotFound();
            }
            empresa.Ativo = false;
            empresa.DataAtualizacao = DateTime.UtcNow;

            await _context.SaveChangesAsync();

            return NoContent();

        }

       
    }
}
