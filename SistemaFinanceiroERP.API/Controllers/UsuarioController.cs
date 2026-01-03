using AutoMapper;
using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SistemaFinanceiroERP.API.DTOs.Usuario;
using SistemaFinanceiroERP.Application.Interfaces;
using SistemaFinanceiroERP.Domain.Entities;
using SistemaFinanceiroERP.Infrastructure.Data;

namespace SistemaFinanceiroERP.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class UsuarioController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;
        private readonly IValidator<UsuarioCreateDto> _createValidator;
        private readonly IValidator<UsuarioUpdateDto> _updateValidator;
        private readonly IPasswordHasher _passwordHasher;
        private readonly ITenantProvider _tenantProvider;

        public UsuarioController(
            AppDbContext context,
            IMapper mapper,
            IValidator<UsuarioCreateDto> createValidator,
            IValidator<UsuarioUpdateDto> updateValidator,
            IPasswordHasher passwordHasher,
            ITenantProvider tenantProvider)
        {
            _context = context;
            _mapper = mapper;
            _createValidator = createValidator;
            _updateValidator = updateValidator;
            _passwordHasher = passwordHasher;
            _tenantProvider = tenantProvider;
        }

        [HttpPost]
        public async Task<ActionResult<UsuarioResponseDto>> Create([FromBody] UsuarioCreateDto dto)
        {
            var validationResult = await _createValidator.ValidateAsync(dto);
            if (!validationResult.IsValid)
            {
                return BadRequest(validationResult.Errors.Select(e => e.ErrorMessage));
            }

            var usuario = _mapper.Map<Usuario>(dto);
            usuario.EmpresaId = _tenantProvider.GetEmpresaId();
            usuario.Senha = _passwordHasher.HashPassword(dto.Senha);
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
            // Query Filter aplica filtro automático por EmpresaId
            var usuarios = await _context.Usuarios.ToListAsync();
            var response = _mapper.Map<List<UsuarioResponseDto>>(usuarios);
            return Ok(response);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<UsuarioResponseDto>> GetById(int id)
        {
            // Query Filter aplica filtro automático por EmpresaId
            var usuario = await _context.Usuarios.FirstOrDefaultAsync(u => u.Id == id);

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

            if (id != dto.Id)
            {
                return BadRequest("O id da URL não corresponde ao id do usuário");
            }

            // Query Filter garante que só busca usuários da empresa
            var usuarioExiste = await _context.Usuarios.FirstOrDefaultAsync(u => u.Id == id);

            if (usuarioExiste == null)
            {
                return NotFound();
            }

            if (!string.IsNullOrWhiteSpace(dto.Senha))
            {
                bool senhaEhIgual = _passwordHasher.VerifyPassword(dto.Senha, usuarioExiste.Senha);
                if (senhaEhIgual)
                {
                    return BadRequest("A nova senha não pode ser igual à senha atual.");
                }
                usuarioExiste.Senha = _passwordHasher.HashPassword(dto.Senha);
            }

            _mapper.Map(dto, usuarioExiste);
            usuarioExiste.EmpresaId = _tenantProvider.GetEmpresaId();
            usuarioExiste.DataAtualizacao = DateTime.UtcNow;

            await _context.SaveChangesAsync();

            var response = _mapper.Map<UsuarioResponseDto>(usuarioExiste);
            return Ok(response);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            // Query Filter garante que só busca usuários da empresa
            var usuario = await _context.Usuarios.FirstOrDefaultAsync(u => u.Id == id);

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