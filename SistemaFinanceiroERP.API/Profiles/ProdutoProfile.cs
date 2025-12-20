using AutoMapper;
using SistemaFinanceiroERP.API.DTOs.Produto;
using SistemaFinanceiroERP.Domain.Entities;

namespace SistemaFinanceiroERP.API.Profiles
{
    public class ProdutoProfile : Profile
    {
        public ProdutoProfile()
        {
            CreateMap<ProdutoCreateDto, Produto>();
            CreateMap<Produto, ProdutoResponseDto>();
            CreateMap<ProdutoUpdateDto, Produto>();
        }
    }
}
