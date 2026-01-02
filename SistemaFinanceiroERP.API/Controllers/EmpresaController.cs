using AutoMapper;
using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SistemaFinanceiroERP.API.DTOs.Empresa;
using SistemaFinanceiroERP.Domain.Entities;
using SistemaFinanceiroERP.Infrastructure.Data;

namespace SistemaFinanceiroERP.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class EmpresaController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;
        private readonly IValidator<EmpresaCreateDto> _createValidator;
        private readonly IValidator<EmpresaUpdateDto> _updateValidator;


        public EmpresaController(AppDbContext context, IMapper mapper, IValidator<EmpresaCreateDto> createValidator, IValidator<EmpresaUpdateDto> updateValidator)
        {
            _context = context;
            _mapper = mapper;
            _createValidator = createValidator;
            _updateValidator = updateValidator;
        }

        [HttpPost]

        public async Task<ActionResult<EmpresaResponseDto>> Create([FromBody] EmpresaCreateDto dto)
        {

            var validationResult = await _createValidator.ValidateAsync(dto);
            if (!validationResult.IsValid)
            {
                return BadRequest(validationResult.Errors.Select(e => e.ErrorMessage));
            }

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

            var validationResult = await _updateValidator.ValidateAsync(dto);
            if (!validationResult.IsValid)
            {
                return BadRequest(validationResult.Errors.Select(e => e.ErrorMessage));
            }


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
