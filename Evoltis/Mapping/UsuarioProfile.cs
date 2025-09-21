using AutoMapper;
using Evoltis.Dto;
using Evoltis.Models;

namespace Evoltis.Mapping
{
    public class UsuarioProfile : Profile
    {
        public UsuarioProfile()
        {
            // Create
            CreateMap<CrearUsuarioDTO, Usuario>();
            CreateMap<DomicilioDTO, Domicilio>();

            // Update
            CreateMap<EditarUsuarioDTO, Usuario>()
                .ForAllMembers(op => op.Condition((src, dest, srcMiembro) => srcMiembro != null));

            CreateMap<DomicilioDTO, Domicilio>()
                .ForAllMembers(op => op.Condition((src, dest, srcMiembro) => srcMiembro != null));

            // Response
            CreateMap<Usuario, RespuestaUsuarioDTO>();
            CreateMap<Domicilio, DomicilioDTO>();
        }
    }
}
