using FluentValidation;
using SistemaFinanceiroERP.Application.DTOs.Auth;
using System.Data;
namespace SistemaFinanceiroERP.API.Validators.Auth
{
    public class LoginDtoValidator : AbstractValidator<LoginDto>
    {
        public LoginDtoValidator()
        {
            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("O email é obrigatório.")
                .EmailAddress().WithMessage("O email deve ser um endereço de email válido.")
                .MaximumLength(150).WithMessage("O email não pode exceder 150 caracteres.");

            RuleFor(x => x.Senha)
                .NotEmpty().WithMessage("A senha é obrigatória.")
                .MinimumLength(6).WithMessage("A senha deve ter pelo menos 6 caracteres.")
                .MaximumLength(100).WithMessage("A senha não pode exceder 100 caracteres.");
        }
    }
}
