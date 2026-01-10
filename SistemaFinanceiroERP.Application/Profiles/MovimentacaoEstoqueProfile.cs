

using AutoMapper;
using SistemaFinanceiroERP.Application.DTOs.MovimentacaoEstoque;
using SistemaFinanceiroERP.Domain.Entities;

namespace SistemaFinanceiroERP.Application.Profiles
{
    public class MovimentacaoEstoqueProfile:Profile
    {
        public MovimentacaoEstoqueProfile()
        {
            CreateMap<MovimentacaoEstoqueCreateDto, MovimentacaoEstoque>();
            CreateMap<MovimentacaoEstoque, MovimentacaoEstoqueResponseDto>()
                .ForMember(dest => dest.NomeProduto, opt => opt.MapFrom(src => src.Produto != null ? src.Produto.ProdutoNome : string.Empty))
                .ForMember(dest => dest.NomeLocalEstoque, opt => opt.MapFrom(src => src.LocalEstoque != null ? src.LocalEstoque.LocalNome : string.Empty))
                .ForMember(dest => dest.NomeUsuario, opt => opt.MapFrom(src => src.Usuario != null ? src.Usuario.UsuarioNome : string.Empty))
                .ForMember(dest => dest.TipoMovimentacao, opt => opt.MapFrom(src => src.TipoMovimentacao.ToString()));

        }
    }
}
