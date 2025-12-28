using AutoMapper;
using SistemaFinanceiroERP.API.DTOs.Empresa;
using SistemaFinanceiroERP.Domain.Entities;

namespace SistemaFinanceiroERP.API.Profiles
{
    public class EmpresaProfile:Profile
    {

        public EmpresaProfile()
        {

            CreateMap<EmpresaCreateDto, Empresa>();
            CreateMap<EmpresaUpdateDto, Empresa>();
            CreateMap<Empresa, EmpresaResponseDto>();

        }

    }
}
