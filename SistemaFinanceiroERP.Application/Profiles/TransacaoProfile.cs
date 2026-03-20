using AutoMapper;
using SistemaFinanceiroERP.Application.DTOs.Transacao;
using SistemaFinanceiroERP.Domain.Entities;

namespace SistemaFinanceiroERP.Application.Profiles
{
    public class TransacaoProfile:Profile
    {
        public TransacaoProfile()
        {
            CreateMap<TransacaoCreateDto, Transacao>();
            CreateMap<Transacao, TransacaoResponseDto>()
                .ForMember(dest => dest.EmpresaCompradoraNome,
                opt => opt.MapFrom(src => src.EmpresaCompradora.NomeEmpresa))
                .ForMember(dest => dest.EmpresaCompradoraRazaoSocial,
                opt => opt.MapFrom(src => src.EmpresaCompradora.RazaoSocial))

                .ForMember(dest => dest.EmpresaVendedoraNome,
                opt => opt.MapFrom(src => src.EmpresaVendedora.NomeEmpresa))
                .ForMember(dest => dest.EmpresaVendedoraRazaoSocial,
                opt => opt.MapFrom(src => src.EmpresaVendedora.RazaoSocial))

                .ForMember(dest => dest.UsuarioNome,
                opt => opt.MapFrom(src => src.Usuario.UsuarioNome))

                .ForMember(dest => dest.ValorTotal,
                opt => opt.MapFrom(src => src.ValorTotal))

                .ForMember(dest => dest.Items,
                opt => opt.MapFrom(src => src.ItemsTransacao));



        }
    }
}
