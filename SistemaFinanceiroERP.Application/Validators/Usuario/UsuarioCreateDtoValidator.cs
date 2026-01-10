using FluentValidation;
using SistemaFinanceiroERP.Application.DTOs.Usuario;
namespace SistemaFinanceiroERP.Application.Validators.Usuario
{
    public class UsuarioCreateDtoValidator : AbstractValidator<UsuarioCreateDto>
    {

        public UsuarioCreateDtoValidator()
        {
            RuleFor(x => x.UsuarioNome)
                .NotEmpty().WithMessage("O nome do usuário é obrigatório.")
                .MaximumLength(100).WithMessage("O nome do usuário não pode exceder 100 caracteres.");

            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("O email do usuário é obrigatório.")
                .EmailAddress().WithMessage("O email do usuário deve ser um endereço de email válido.")
                .MaximumLength(150).WithMessage("O email do usuário não pode exceder 150 caracteres.");

            RuleFor(x => x.Senha)
                .NotEmpty().WithMessage("A senha é obrigatória.")
                .MinimumLength(6).WithMessage("A senha deve ter pelo menos 6 caracteres.")
                .MaximumLength(100).WithMessage("A senha não pode exceder 100 caracteres.");
            
            RuleFor(x => x.Telefone)
                        .NotEmpty().WithMessage("O telefone do usuário é obrigatório.")
                        .MaximumLength(15).WithMessage("O telefone do usuário não pode exceder 15 caracteres.");


        }
    }
}