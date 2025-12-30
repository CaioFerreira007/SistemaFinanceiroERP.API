using AutoMapper;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SistemaFinanceiroERP.API.DTOs.Usuario;
using SistemaFinanceiroERP.Domain.Entities;
using SistemaFinanceiroERP.Infrastructure.Data;

namespace SistemaFinanceiroERP.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsuarioController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;
        private readonly IValidator<UsuarioCreateDto> _createValidator;
        private readonly IValidator<UsuarioUpdateDto> _updateValidator;

        public UsuarioController(AppDbContext context, IMapper mapper, IValidator<UsuarioCreateDto> createValidator, IValidator <UsuarioUpdateDto>updateValidator)
        {
            _context = context;
            _mapper = mapper;
            _createValidator = createValidator;
            _updateValidator = updateValidator;
        }

        [HttpPost]
        public async Task<ActionResult<UsuarioResponseDto>> Create([FromBody] UsuarioCreateDto dto)
        {

            var validationResult = await _createValidator.ValidateAsync(dto);
            if (!validationResult.IsValid)
            {
                return BadRequest(validationResult.Errors.Select(e => e.ErrorMessage));
            }

            var empresa = await _context.Empresas.FindAsync(dto.EmpresaId);
            if (empresa == null || !empresa.Ativo)
            {
                return BadRequest("Empresa não encontrada ou inativa");
            }

            var usuario = _mapper.Map<Usuario>(dto);
            usuario.DataCriacao = DateTime.UtcNow;
            usuario.Ativo = true;

            _context.Usuarios.Add(usuario);


            await _context.SaveChangesAsync();

            var response = _mapper.Map<UsuarioResponseDto>(usuario);
            return CreatedAtAction(nameof(GetById), new { id = usuario.Id }, response);


        }


        [HttpGet]

        public async Task<ActionResult<IEnumerable<UsuarioResponseDto>>> GetAll()
        {
            var usuarios = await _context.Usuarios.ToListAsync();

            var response = _mapper.Map<List<UsuarioResponseDto>>(usuarios);

            return Ok(response);
        }

        [HttpGet("{id}")]

        public async Task<ActionResult<UsuarioResponseDto>> GetById(int id)
        {
            var usuario = await _context.Usuarios.FindAsync(id);

            if (usuario == null)
            {
                return NotFound();
            }
            var response = _mapper.Map<UsuarioResponseDto>(usuario);
            return Ok(response);
        }

        [HttpPut("{id}")]

        public async Task<ActionResult<UsuarioResponseDto>> Update(int id, [FromBody] UsuarioUpdateDto dto)
        {


            var validationResult = await _updateValidator.ValidateAsync(dto);
            if (!validationResult.IsValid)
            {
                return BadRequest(validationResult.Errors.Select(e => e.ErrorMessage));
            }
            var empresa = await _context.Empresas.FindAsync(id);
          
            if (id != dto.Id)
            {
                return BadRequest("O id da url não corresponde ao id do usuário");
            }
            var usuarioExiste = await _context.Usuarios.FindAsync(id);
            if (usuarioExiste == null)
            {
                return NotFound();
            }


            var empresaDoUsuario = await _context.Empresas.FindAsync(usuarioExiste.EmpresaId);
            if (empresaDoUsuario == null || !empresaDoUsuario.Ativo)
            {
                return NotFound("A empresa associada ao usuário não foi encontrada ou está inativa.");
            }

           
            _mapper.Map(dto, usuarioExiste);

            usuarioExiste.DataAtualizacao = DateTime.UtcNow;
            var response = _mapper.Map<UsuarioResponseDto>(usuarioExiste);


            await _context.SaveChangesAsync();

            return Ok(response);
        }
        [HttpDelete("{id}")]

        public async Task<ActionResult> Delete(int id)
        {
            var usuario = await _context.Usuarios.FindAsync(id);

            if (usuario == null)
            {
                return NotFound();
            }
            usuario.Ativo = false;
            usuario.DataAtualizacao = DateTime.UtcNow;

            await _context.SaveChangesAsync();

            return NoContent();

        }

    }
}