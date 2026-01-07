using AutoMapper;
using SistemaFinanceiroERP.Application.DTOs.Usuario;
using SistemaFinanceiroERP.Domain.Entities;

namespace SistemaFinanceiroERP.API.Profiles
{
    public class UsuarioProfile : Profile
    {

        public UsuarioProfile()
        {
            CreateMap<UsuarioCreateDto, Usuario>();
            CreateMap<UsuarioUpdateDto, Usuario>().ForMember(dest => dest.Senha, opt => opt.Ignore());
            CreateMap<Usuario, UsuarioResponseDto>();
        }
    }
}
