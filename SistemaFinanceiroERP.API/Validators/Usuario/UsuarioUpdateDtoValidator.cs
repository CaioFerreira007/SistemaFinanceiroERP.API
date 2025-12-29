using FluentValidation;
using SistemaFinanceiroERP.API.DTOs.Usuario;
namespace SistemaFinanceiroERP.API.Validators.Usuario
{
    public class UsuarioUpdateDtoValidator : AbstractValidator<UsuarioUpdateDto>
    {

        public UsuarioUpdateDtoValidator()
        {
            RuleFor(x => x.Id)
    .GreaterThan(0).WithMessage("Id inválido.");
            RuleFor(x => x.UsuarioNome)
                .NotEmpty().WithMessage("O nome do usuário é obrigatório.")
                .MaximumLength(100).WithMessage("O nome do usuário não pode exceder 100 caracteres.");

            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("O e-mail é obrigatório.")
                .EmailAddress().WithMessage("O e-mail deve ser válido.")
                .MaximumLength(150).WithMessage("O e-mail não pode exceder 150 caracteres.");

            RuleFor(x => x.Senha)
                .MaximumLength(100).WithMessage("A senha do usuário não pode exceder 100 caracteres.")
                .NotEmpty().WithMessage("A senha é obrigatória")
                .MinimumLength(6).WithMessage("A senha deve conter no mínimo 6 caracteres.");
            RuleFor(x => x.Telefone)
            .NotEmpty().WithMessage("O telefone é obrigatório.")
            .MaximumLength(20).WithMessage("O telefone não pode exceder 20 caracteres.");
        }

    }
}
