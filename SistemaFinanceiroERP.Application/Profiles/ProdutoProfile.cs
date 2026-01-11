using AutoMapper;
using SistemaFinanceiroERP.Application.DTOs.Produto;
using SistemaFinanceiroERP.Domain.Entities;

namespace SistemaFinanceiroERP.Application.Profiles
{
    public class ProdutoProfile : Profile
    {
        public ProdutoProfile()
        {
            // Create
            CreateMap<ProdutoCreateDto, Produto>()
                .ForMember(dest => dest.ProdutoNome, opt => opt.MapFrom(src => src.ProdutoNome));

            // Update
            CreateMap<ProdutoUpdateDto, Produto>()
                .ForMember(dest => dest.ProdutoNome, opt => opt.MapFrom(src => src.ProdutoNome));

            // Response
            CreateMap<Produto, ProdutoResponseDto>()
                .ForMember(dest => dest.QuantidadeEstoque,
                           opt => opt.MapFrom(src => src.QuantidadeEstoqueTotal));
        }
    }
}