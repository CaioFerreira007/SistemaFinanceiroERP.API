using AutoMapper;
using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SistemaFinanceiroERP.API.DTOs.Auth;
using SistemaFinanceiroERP.API.DTOs.Usuario;
using SistemaFinanceiroERP.Application.Interfaces;
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

        public AuthController(AppDbContext context, IValidator<LoginDto> loginValidator,
            IValidator<RegisterDto> registerValidator, IPasswordHasher passwordHasher,
            ITokenService tokenService, IMapper mapper, IConfiguration configuration)
        {
            _context = context;
            _loginValidator = loginValidator;
            _registerValidator = registerValidator;
            _passwordHasher = passwordHasher;
            _tokenService = tokenService;
            _mapper = mapper;
            _configuration = configuration;
        }

        [HttpPost("login")]

        public async Task<ActionResult<TokenDto>> Login([FromBody] LoginDto dto)
        {
            var validationResult = await _loginValidator.ValidateAsync(dto);
            if (!validationResult.IsValid)
            {
                return BadRequest(validationResult.Errors);
            }


            var usuario = await _context.Usuarios
                .IgnoreQueryFilters()
                .FirstOrDefaultAsync(u => u.Email.ToLower() == dto.Email.ToLower());

            if (usuario == null)
            {
                return Unauthorized("Credenciais inválidas");
            }
            if (!usuario.Ativo)
            {
                return Unauthorized("Usuário inativo.");
            }
            bool senhaValida = _passwordHasher.VerifyPassword(dto.Senha, usuario.Senha);
            if (!senhaValida)
            {
                return Unauthorized("Credenciais inválidas.");
            }
            var token = _tokenService.GerarToken(usuario);

            var expirationMinutes = int.Parse(_configuration["Jwt:ExpirationMinutes"] ?? "60");

            var tokenDto = new TokenDto
            {
                Token = token,
                ExpiresIn = expirationMinutes * 60,
                UsuarioId = usuario.Id,
                EmailUsuario = usuario.Email,
                UsuarioNome = usuario.UsuarioNome,
                EmpresaId = usuario.EmpresaId
            };

            return Ok(tokenDto);
        }

        [HttpPost("register")]
        public async Task<ActionResult<TokenDto>> Register([FromBody] RegisterDto dto)
        {
            // 1. Validar DTO
            var validationResult = await _registerValidator.ValidateAsync(dto);
            if (!validationResult.IsValid)
            {
                return BadRequest(validationResult.Errors);
            }

            // 2. Verificar Email duplicado
            var emailExiste = await _context.Usuarios
                .IgnoreQueryFilters()
                .AnyAsync(u => u.Email.ToLower() == dto.EmailUsuario.ToLower());
            if (emailExiste)
            {
                return BadRequest("Email já cadastrado.");
            }

            // 3. Verificar CNPJ duplicado
            var cnpjExiste = await _context.Empresas
                .IgnoreQueryFilters()
                .AnyAsync(e => e.Cnpj == dto.Cnpj);
            if (cnpjExiste)
            {
                return BadRequest("CNPJ já cadastrado.");
            }

            // 4. Criar Empresa + Usuario em transação
            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                // Criar Empresa
                var empresa = new Domain.Entities.Empresa
                {
                    NomeEmpresa = dto.NomeEmpresa,
                    RazaoSocial = dto.RazaoSocial,
                    Cnpj = dto.Cnpj,
                    Telefone = dto.TelefoneEmpresa,
                    Email = dto.EmailEmpresa,
                    Tipo = dto.Tipo,
                    DataCriacao = DateTime.UtcNow,
                    Ativo = true
                };

                _context.Empresas.Add(empresa);
                await _context.SaveChangesAsync();  // Gera empresa.Id

                // Criar Usuario
                var usuario = new Domain.Entities.Usuario
                {
                    UsuarioNome = dto.NomeUsuario,
                    Email = dto.EmailUsuario,
                    Senha = _passwordHasher.HashPassword(dto.Senha),
                    Telefone = dto.TelefoneUsuario,
                    EmpresaId = empresa.Id,
                    DataCriacao = DateTime.UtcNow,
                    Ativo = true
                };

                _context.Usuarios.Add(usuario);
                await _context.SaveChangesAsync();

                // Confirmar transação
                await transaction.CommitAsync();

                // 5. Gerar Token JWT
                var token = _tokenService.GerarToken(usuario);
                var expirationMinutes = int.Parse(_configuration["Jwt:ExpirationMinutes"] ?? "60");

                // 6. Montar resposta
                var tokenDto = new TokenDto
                {
                    Token = token,
                    ExpiresIn = expirationMinutes * 60,
                    UsuarioId = usuario.Id,
                    EmailUsuario = usuario.Email,
                    UsuarioNome = usuario.UsuarioNome,
                    EmpresaId = empresa.Id
                };

                return Ok(tokenDto);
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

            var userIdClaim = User.FindFirst("sub")?.Value;
            if (string.IsNullOrEmpty(userIdClaim))
            {
                return Unauthorized("Token inválido!");
            }
            var userId = int.Parse(userIdClaim);
            var usuario = await _context.Usuarios.FindAsync(userId);

            if (usuario == null)
            {
                return NotFound("Usuário não encontrado.");
            }
            var usuarioDto = _mapper.Map<UsuarioResponseDto>(usuario);
            return Ok(usuarioDto);


        }
    }
}
