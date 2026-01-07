using FluentValidation;
using SistemaFinanceiroERP.Application.DTOs.Empresa;
namespace SistemaFinanceiroERP.API.Validators.Empresa
{
    public class EmpresaUpdateDtoValidator:AbstractValidator<EmpresaUpdateDto>
    {

        public EmpresaUpdateDtoValidator()
        {
            
            RuleFor(x=>x.Id)
                .GreaterThan(0).WithMessage("O ID da empresa deve ser maior que zero.");

            RuleFor(x => x.NomeEmpresa)
                .NotEmpty()
                .WithMessage("O nome da empresa é obrigatório.")
                .MaximumLength(100)
                .WithMessage("O nome da empresa não pode exceder 100 caracteres.");
            RuleFor(x => x.RazaoSocial)
                .NotEmpty()
                .WithMessage("A razão social é obrigatória.")
                .MaximumLength(200)
                .WithMessage("A razão social não pode exceder 200 caracteres.");
            RuleFor(x => x.Cnpj)
                .NotEmpty()
                .WithMessage("O CNPJ é obrigatório.")
                .MaximumLength(18)
                .WithMessage("O CNPJ não pode exceder 18 caracteres.");
            RuleFor(x => x.Telefone)
                .NotEmpty()
                .WithMessage("O telefone da empresa é obrigatório.")
                .MaximumLength(20)
                .WithMessage("O telefone não pode exceder 20 caracteres.");
            RuleFor(x => x.Email)
                .NotEmpty()
                .WithMessage("O e-mail da empresa é obrigatório.")
                .EmailAddress()
                .WithMessage("O e-mail deve ser um endereço válido.")
                .MaximumLength(150)
                .WithMessage("O e-mail não pode exceder 150 caracteres.");
        }

    }
}
