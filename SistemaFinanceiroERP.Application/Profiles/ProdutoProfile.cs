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
            CreateMap<ProdutoCreateDto, Produto>();

            // Update
            CreateMap<ProdutoUpdateDto, Produto>();

            // Response
            CreateMap<Produto, ProdutoResponseDto>().ForMember(dest => dest.QuantidadeEstoque,
               opt => opt.MapFrom(src => src.QuantidadeEstoqueTotal));
        }
    }
}