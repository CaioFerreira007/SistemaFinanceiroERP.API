using AutoMapper;
using SistemaFinanceiroERP.Application.DTOs.AjusteEstoque;
using SistemaFinanceiroERP.Domain.Entities;
namespace SistemaFinanceiroERP.Application.Profiles
{
    public class AjusteEstoqueProfile:Profile
    {
       public AjusteEstoqueProfile()
        {
            CreateMap<AjusteEstoqueCreateDto, AjusteEstoque>();
            CreateMap<AjusteEstoque, AjusteEstoqueResponseDto>()
                .ForMember(dest => dest.ProdutoNome,
                otp => otp.MapFrom(src => src.Produto.ProdutoNome))
                .ForMember(dest => dest.LocalEstoqueNome,
                otp => otp.MapFrom(src => src.LocalEstoque.LocalNome)) 
                .ForMember(dest => dest.UsuarioNome,
                otp => otp.MapFrom(src => src.Usuario.UsuarioNome))
                .ForMember(dest => dest.Diferenca,
                otp => otp.MapFrom(src => src.QuantidadeNova - src.QuantidadeAnterior));


        }
    }
}
