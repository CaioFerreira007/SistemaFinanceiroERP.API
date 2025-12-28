using AutoMapper;
using SistemaFinanceiroERP.API.DTOs.Usuario;
using SistemaFinanceiroERP.Domain.Entities;

namespace SistemaFinanceiroERP.API.Profiles
{
    public class UsuarioProfile : Profile
    {

        public UsuarioProfile()
        {
            CreateMap<UsuarioCreateDto, Usuario>();
            CreateMap<UsuarioUpdateDto, Usuario>();
            CreateMap<Usuario, UsuarioResponseDto>();
        }
    }
}
