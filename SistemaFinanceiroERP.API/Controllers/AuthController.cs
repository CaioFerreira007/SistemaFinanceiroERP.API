using AutoMapper;
using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SistemaFinanceiroERP.Application.DTOs.Auth;
using SistemaFinanceiroERP.Application.DTOs.Usuario;
using SistemaFinanceiroERP.Domain.Interfaces;
using SistemaFinanceiroERP.Infrastructure.Data;

namespace SistemaFinanceiroERP.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly IValidator<LoginDto> _loginValidator;
        private readonly IValidator<RegisterDto> _registerValidator;
        private readonly IPasswordHasher _passwordHasher;
        private readonly ITokenService _tokenService;
        private readonly IMapper _mapper;
        private readonly IConfiguration _configuration;
        private readonly ITenantProvider _tenantProvider;

        public AuthController(
            AppDbContext context,
            IValidator<LoginDto> loginValidator,
            IValidator<RegisterDto> registerValidator,
            IPasswordHasher passwordHasher,
            ITokenService tokenService,
            IMapper mapper,
            IConfiguration configuration,
            ITenantProvider tenantProvider)
        {
            _context = context;
            _loginValidator = loginValidator;
            _registerValidator = registerValidator;
            _passwordHasher = passwordHasher;
            _tokenService = tokenService;
            _mapper = mapper;
            _configuration = configuration;
            _tenantProvider = tenantProvider;
        }

        [HttpPost("login")]
        public async Task<ActionResult<TokenDto>> Login([FromBody] LoginDto dto)
        {
            var validationResult = await _loginValidator.ValidateAsync(dto);
            if (!validationResult.IsValid)
                return BadRequest(validationResult.Errors);

            var usuario = await _context.Usuarios
                .IgnoreQueryFilters()
                .FirstOrDefaultAsync(u => u.Email.ToLower() == dto.Email.ToLower());

            if (usuario == null) return Unauthorized("Credenciais inválidas");
            if (!usuario.Ativo) return Unauthorized("Usuário inativo.");

            bool senhaValida = _passwordHasher.VerifyPassword(dto.Senha, usuario.Senha);
            if (!senhaValida) return Unauthorized("Credenciais inválidas.");

            var token = _tokenService.GerarToken(usuario);
            var expirationMinutes = int.Parse(_configuration["Jwt:ExpirationMinutes"] ?? "60");

            return Ok(new TokenDto
            {
                Token = token,
                ExpiresIn = expirationMinutes * 60,
                UsuarioId = usuario.Id,
                EmailUsuario = usuario.Email,
                UsuarioNome = usuario.UsuarioNome,
                EmpresaId = usuario.EmpresaId
            });
        }

        [HttpPost("register")]
        public async Task<ActionResult<TokenDto>> Register([FromBody] RegisterDto dto)
        {
            var validationResult = await _registerValidator.ValidateAsync(dto);
            if (!validationResult.IsValid)
                return BadRequest(validationResult.Errors);

            var emailExiste = await _context.Usuarios
                .IgnoreQueryFilters()
                .AnyAsync(u => u.Email.ToLower() == dto.EmailUsuario.ToLower());
            if (emailExiste) return BadRequest("Email já cadastrado.");

            var cnpjExiste = await _context.Empresas
                .IgnoreQueryFilters()
                .AnyAsync(e => e.Cnpj == dto.Cnpj);
            if (cnpjExiste) return BadRequest("CNPJ já cadastrado.");

            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                var empresa = new Domain.Entities.Empresa
                {
                    NomeEmpresa = dto.NomeEmpresa,
                    RazaoSocial = dto.RazaoSocial,
                    Cnpj = dto.Cnpj,
                    Telefone = dto.TelefoneEmpresa,
                    Email = dto.EmailEmpresa,
                    Tipo = dto.Tipo,
                    Ativo = true
                };

                _context.Empresas.Add(empresa);
                await _context.SaveChangesAsync();

                var usuario = new Domain.Entities.Usuario
                {
                    UsuarioNome = dto.NomeUsuario,
                    Email = dto.EmailUsuario,
                    Senha = _passwordHasher.HashPassword(dto.Senha),
                    Telefone = dto.TelefoneUsuario,
                    EmpresaId = empresa.Id,
                    Ativo = true
                };

                _context.Usuarios.Add(usuario);
                await _context.SaveChangesAsync();

                await transaction.CommitAsync();

                var token = _tokenService.GerarToken(usuario);
                var expirationMinutes = int.Parse(_configuration["Jwt:ExpirationMinutes"] ?? "60");

                return Ok(new TokenDto
                {
                    Token = token,
                    ExpiresIn = expirationMinutes * 60,
                    UsuarioId = usuario.Id,
                    EmailUsuario = usuario.Email,
                    UsuarioNome = usuario.UsuarioNome,
                    EmpresaId = empresa.Id
                });
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }
        }

        [HttpGet("me")]
        [Authorize]
        public async Task<ActionResult<UsuarioResponseDto>> Me()
        {
            var userId = _tenantProvider.GetUsuarioId();
            var usuario = await _context.Usuarios.FindAsync(userId);

            if (usuario == null) return NotFound("Usuário não encontrado.");

            var dto = _mapper.Map<UsuarioResponseDto>(usuario);
            return Ok(dto);
        }
    }
}
