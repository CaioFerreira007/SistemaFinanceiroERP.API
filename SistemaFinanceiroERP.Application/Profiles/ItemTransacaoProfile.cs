using AutoMapper;
using SistemaFinanceiroERP.Application.DTOs.ItemTransacao;
using SistemaFinanceiroERP.Domain.Entities;
namespace SistemaFinanceiroERP.Application.Profiles
{
    public class ItemTransacaoProfile:Profile
    {
        public ItemTransacaoProfile()
        {
            CreateMap<ItemTransacaoCreateDto, ItemTransacao>();
            CreateMap<ItemTransacao, ItemTransacaoResponseDto>()
                .ForMember(dest => dest.ProdutoNome,
                opt => opt.MapFrom(src => src.Produto.ProdutoNome))
                .ForMember(dest => dest.CodigoBarras,
                opt => opt.MapFrom(src => src.Produto.CodigoBarras))
                .ForMember(dest => dest.Subtotal,
                opt => opt.MapFrom(src => src.PrecoUnitario * src.Quantidade));
        }
    }
}
