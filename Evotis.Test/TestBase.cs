using AutoMapper;
using Evoltis.Dto;
using Evoltis.Mapping;
using Evoltis.Models;
using Microsoft.Extensions.DependencyInjection;

namespace Evoltis.Tests
{
    public class TestBase
    {
        //protected readonly IMapper _mapper;

        public TestBase()
        {
            //var services = new ServiceCollection();
            //services.AddAutoMapper(cfg => cfg.AddProfile<UsuarioProfile>());
            //var serviceProvider = services.BuildServiceProvider();
            //_mapper = serviceProvider.GetRequiredService<IMapper>();
        }

        protected Usuario CrearUsuarioValido()
        {
            return new Usuario
            {
                ID = 1,
                Nombre = "Juan Pérez",
                Email = "juan.perez@example.com",
                FechaCreacion = DateTime.Now,
                Domicilio = new Domicilio
                {
                    ID = 1,
                    Calle = "Av. Siempre Viva",
                    Numero = "742",
                    Ciudad = "Springfield",
                    Provincia = "Buenos Aires"
                }
            };
        }

        protected CrearUsuarioDTO CrearUsuarioDTOValido()
        {
            return new CrearUsuarioDTO
            {
                Nombre = "Juan Pérez",
                Email = "juan.perez@example.com"
            };
        }

        protected EditarUsuarioDTO EditarUsuarioDTOValido()
        {
            return new EditarUsuarioDTO
            {
                Nombre = "Juan Pérez Editado",
                Email = "juan.editado@example.com"
            };
        }

        protected DomicilioDTO CrearDomicilioDTOValido()
        {
            return new DomicilioDTO
            {
                Calle = "Av. Siempre Viva",
                Numero = "742",
                Ciudad = "Springfield",
                Provincia = "Buenos Aires"
            };
        }

        protected FiltrosUsuarioDTO CrearFiltrosUsuarioDTO()
        {
            return new FiltrosUsuarioDTO
            {
                Nombre = "Juan",
                Provincia = "Córdoba",
                Ciudad = "Río Segundo"
            };
        }
    }
}