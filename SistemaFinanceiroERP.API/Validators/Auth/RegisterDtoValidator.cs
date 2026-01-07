using FluentValidation;
using SistemaFinanceiroERP.Application.DTOs.Auth;
namespace SistemaFinanceiroERP.API.Validators.Auth
{
    public class RegisterDtoValidator: AbstractValidator<RegisterDto>
    {
        public RegisterDtoValidator()
        {
            // Validações para os campos de registro da empresa
            RuleFor(x => x.NomeEmpresa)
                .NotEmpty().WithMessage("O nome da empresa é obrigatório.")
                .MaximumLength(100).WithMessage("O nome da empresa não pode exceder 100 caracteres.");
            RuleFor(x => x.RazaoSocial)
                .NotEmpty().WithMessage("A razão social é obrigatória.")
                .MaximumLength(200).WithMessage("A razão social não pode exceder 150 caracteres.");
            RuleFor(x => x.Cnpj)
                .NotEmpty().WithMessage("O CNPJ é obrigatório.")
                .Length(14).WithMessage("O CNPJ deve ter exatamente 14 caracteres.");
            RuleFor(x => x.TelefoneEmpresa)
                .NotEmpty().WithMessage("O telefone da empresa é obrigatório.")
                .MaximumLength(20).WithMessage("O telefone da empresa não pode exceder 20 caracteres.");
            RuleFor(x => x.EmailEmpresa)
                .NotEmpty().WithMessage("O email da empresa é obrigatório.")
                .EmailAddress().WithMessage("O email da empresa deve ser um endereço de email válido.")
                .MaximumLength(150).WithMessage("O email da empresa não pode exceder 150 caracteres.");
            RuleFor(x => x.Tipo)
                .IsInEnum().WithMessage("O tipo de empresa é inválido.");

            // Validações para os campos de registro do usuário

            RuleFor(x => x.NomeUsuario)
                .NotEmpty().WithMessage("O nome do usuário é obrigatório.")
                .MaximumLength(100).WithMessage("O nome do usuário não pode exceder 100 caracteres.");
            RuleFor(x => x.EmailUsuario)
                .NotEmpty().WithMessage("O email do usuário é obrigatório.")
                .EmailAddress().WithMessage("O email do usuário deve ser um endereço de email válido.")
                .MaximumLength(150).WithMessage("O email do usuário não pode exceder 150 caracteres.");
            RuleFor(x => x.Senha)
                .NotEmpty().WithMessage("A senha é obrigatória.")
                .MinimumLength(6).WithMessage("A senha deve ter pelo menos 6 caracteres.")
                .MaximumLength(100).WithMessage("A senha não pode exceder 100 caracteres.");
            RuleFor(x => x.TelefoneUsuario)
                .NotEmpty().WithMessage("O telefone do usuário é obrigatório.")
                .MaximumLength(15).WithMessage("O telefone do usuário não pode exceder 15 caracteres.");

        }
    }
}
