using AutoMapper;
using SistemaFinanceiroERP.Application.DTOs.LocalEstoque;
using SistemaFinanceiroERP.Domain.Entities;
namespace SistemaFinanceiroERP.Application.Profiles
{
    public class LocalEstoqueProfile : Profile
    {
        public LocalEstoqueProfile()
        {
            CreateMap<LocalEstoqueCreateDto, LocalEstoque>();
            CreateMap<LocalEstoqueUpdateDto, LocalEstoque>();
            CreateMap<LocalEstoque, LocalEstoqueResponseDto>();

        }
    }
}
