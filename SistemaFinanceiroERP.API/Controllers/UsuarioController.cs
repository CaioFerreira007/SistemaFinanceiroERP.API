using AutoMapper;
using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SistemaFinanceiroERP.Application.DTOs.Usuario;
using SistemaFinanceiroERP.Domain.Entities;
using SistemaFinanceiroERP.Domain.Interfaces;

namespace SistemaFinanceiroERP.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class UsuarioController : ControllerBase
    {
        private readonly IUsuarioRepository _repository;
        private readonly IMapper _mapper;
        private readonly IValidator<UsuarioCreateDto> _createValidator;
        private readonly IValidator<UsuarioUpdateDto> _updateValidator;
        private readonly IPasswordHasher _passwordHasher;
        private readonly ITenantProvider _tenantProvider;

        public UsuarioController(
            IUsuarioRepository repository,
            IMapper mapper,
            IValidator<UsuarioCreateDto> createValidator,
            IValidator<UsuarioUpdateDto> updateValidator,
            IPasswordHasher passwordHasher,
            ITenantProvider tenantProvider)
        {
            _repository = repository;
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
                return BadRequest(validationResult.Errors.Select(e => e.ErrorMessage));

            var usuario = _mapper.Map<Usuario>(dto);
            usuario.EmpresaId = _tenantProvider.GetEmpresaId();
            usuario.Senha = _passwordHasher.HashPassword(dto.Senha);

            await _repository.AddAsync(usuario);
            await _repository.SaveChangesAsync();

            var response = _mapper.Map<UsuarioResponseDto>(usuario);
            return CreatedAtAction(nameof(GetById), new { id = usuario.Id }, response);
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<UsuarioResponseDto>>> GetAll()
        {
            var usuarios = await _repository.GetAllAsync();
            var response = _mapper.Map<List<UsuarioResponseDto>>(usuarios);
            return Ok(response);
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<UsuarioResponseDto>> GetById(int id)
        {
            var usuario = await _repository.GetByIdAsync(id);
            if (usuario == null) return NotFound();

            var response = _mapper.Map<UsuarioResponseDto>(usuario);
            return Ok(response);
        }

        [HttpPut("{id:int}")]
        public async Task<ActionResult<UsuarioResponseDto>> Update(int id, [FromBody] UsuarioUpdateDto dto)
        {
            var validationResult = await _updateValidator.ValidateAsync(dto);
            if (!validationResult.IsValid)
                return BadRequest(validationResult.Errors.Select(e => e.ErrorMessage));

            if (id != dto.Id)
                return BadRequest("O id da URL não corresponde ao id do usuário");

            var usuarioExiste = await _repository.GetByIdAsync(id);
            if (usuarioExiste == null) return NotFound();

            if (!string.IsNullOrWhiteSpace(dto.Senha))
            {
                bool senhaEhIgual = _passwordHasher.VerifyPassword(dto.Senha, usuarioExiste.Senha);
                if (senhaEhIgual)
                    return BadRequest("A nova senha não pode ser igual à senha atual.");

                usuarioExiste.Senha = _passwordHasher.HashPassword(dto.Senha);
            }

            _mapper.Map(dto, usuarioExiste);
            usuarioExiste.EmpresaId = _tenantProvider.GetEmpresaId();

            await _repository.UpdateAsync(usuarioExiste);
            await _repository.SaveChangesAsync();

            var response = _mapper.Map<UsuarioResponseDto>(usuarioExiste);
            return Ok(response);
        }

        [HttpDelete("{id:int}")]
        public async Task<ActionResult> Delete(int id)
        {
            var usuario = await _repository.GetByIdAsync(id);
            if (usuario == null) return NotFound();

            usuario.Ativo = false;
            await _repository.UpdateAsync(usuario);
            await _repository.SaveChangesAsync();

            return NoContent();
        }
    }
}
