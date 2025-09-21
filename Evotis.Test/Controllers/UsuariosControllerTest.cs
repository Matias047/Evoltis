using AutoMapper;
using Evoltis.Controllers;
using Evoltis.Dto;
using Evoltis.Models;
using Evoltis.Services;
using FluentAssertions;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc;
using Moq;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Evoltis.Tests.UnitTests.Controllers
{
    public class UsuariosControllerTests : TestBase
    {
        private readonly Mock<IUsuarioService> _mockUsuarioService;
        private readonly Mock<IValidator<CrearUsuarioDTO>> _mockCrearUsuarioValidator;
        private readonly Mock<IValidator<EditarUsuarioDTO>> _mockEditarUsuarioValidator;
        private readonly UsuariosController _controller;
        private readonly Mock<IMapper> _mockMapper;

        public UsuariosControllerTests()
        {
            _mockUsuarioService = new Mock<IUsuarioService>();
            _mockUsuarioService.Setup(s => s.BuscarUsuariosAsync(It.IsAny<FiltrosUsuarioDTO>()))
            .ReturnsAsync(new List<Usuario> { new Usuario { ID = 1, Nombre = "Juan Pérez", Email = "juan.perez@example.com", Domicilio= new Domicilio
                    {
                        Calle = "Av. Siempre Viva",
                        Numero = "742",
                        Ciudad = "Springfield",
                        Provincia = "Buenos Aires"
                    } } });

            _mockCrearUsuarioValidator = new Mock<IValidator<CrearUsuarioDTO>>();
            _mockEditarUsuarioValidator = new Mock<IValidator<EditarUsuarioDTO>>();
            _mockMapper = new Mock<IMapper>();
            _mockMapper.Setup(m => m.Map<Usuario>(It.IsAny<CrearUsuarioDTO>()))
                .Returns((CrearUsuarioDTO dto) => new Usuario
                {
                    Nombre = dto.Nombre,
                    Email = dto.Email,
                    FechaCreacion = DateTime.Now
                });
            _mockMapper.Setup(m => m.Map<RespuestaUsuarioDTO>(It.IsAny<Usuario>())).Returns((Usuario usuario) => new RespuestaUsuarioDTO
            {
                ID = usuario.ID,
                Nombre = usuario.Nombre,
                Email = usuario.Email,
                FechaCreacion = usuario.FechaCreacion
            });
            _mockMapper.Setup(m => m.Map<DomicilioDTO>(It.IsAny<Domicilio>()))
                .Returns((Domicilio domicilio) => new DomicilioDTO
                {
                    Calle = domicilio.Calle,
                    Numero = domicilio.Numero,
                    Ciudad = domicilio.Ciudad,
                    Provincia = domicilio.Provincia
                });
            _mockMapper.Setup(m => m.Map<IEnumerable<RespuestaUsuarioDTO>>(It.IsAny<IEnumerable<Usuario>>()))
                .Returns((IEnumerable<Usuario> usuarios) => usuarios.Select(u => new RespuestaUsuarioDTO
                {
                    ID = u.ID,
                    Nombre = u.Nombre,
                    Email = u.Email,
                    FechaCreacion = u.FechaCreacion,
                    Domicilio = new DomicilioDTO
                    {
                        Calle = u.Domicilio.Calle,
                        Numero = u.Domicilio.Numero,
                        Ciudad = u.Domicilio.Ciudad,
                        Provincia = u.Domicilio.Provincia
                    }
                }));
            
            _controller = new UsuariosController(_mockUsuarioService.Object, _mockMapper.Object);
            
        }

        //#region CrearUsuario Tests

        [Fact]
        public async Task CrearUsuario_ConDatosValidos_DeberiaRetornarOk()
        {
            // Arrange
            var dto = CrearUsuarioDTOValido();
            var usuario = CrearUsuarioValido();

            _mockCrearUsuarioValidator
                .Setup(v => v.ValidateAsync(dto, default))
                .ReturnsAsync(new ValidationResult());

            _mockUsuarioService
                .Setup(s => s.CrearUsuarioAsync(It.IsAny<Usuario>()))
                .Returns(Task.CompletedTask);

            // Act
            var result = await _controller.CrearUsuario(dto, _mockCrearUsuarioValidator.Object);

            // Assert
            result.Result.Should().BeOfType<OkObjectResult>();
            var okResult = result.Result as OkObjectResult;
            okResult.Value.Should().BeOfType<RespuestaUsuarioDTO>();

            var respuesta = okResult.Value as RespuestaUsuarioDTO;
            respuesta.Nombre.Should().Be(dto.Nombre);
            respuesta.Email.Should().Be(dto.Email);

            _mockUsuarioService.Verify(s => s.CrearUsuarioAsync(It.IsAny<Usuario>()), Times.Once);
        }

        [Fact]
        public async Task CrearUsuario_ConDatosInvalidos_DeberiaRetornarBadRequest()
        {
            // Arrange
            var dto = new CrearUsuarioDTO { Nombre = "", Email = "email-invalido" };
            var validationResult = new ValidationResult(new[]
            {
                new ValidationFailure("Nombre", "El nombre es obligatorio."),
                new ValidationFailure("Email", "El email no tiene un formato válido.")
            });

            _mockCrearUsuarioValidator
                .Setup(v => v.ValidateAsync(dto, default))
                .ReturnsAsync(validationResult);

            // Act
            var result = await _controller.CrearUsuario(dto, _mockCrearUsuarioValidator.Object);

            // Assert
            result.Result.Should().BeOfType<BadRequestObjectResult>();
            var badRequestResult = result.Result as BadRequestObjectResult;

            var response = badRequestResult.Value;
            response.Should().NotBeNull();

            _mockUsuarioService.Verify(s => s.CrearUsuarioAsync(It.IsAny<Usuario>()), Times.Never);
        }

        //#endregion

        //#region BuscarUsuarios Tests

        [Fact]
        public async Task BuscarUsuarios_DeberiaRetornarListaUsuarios()
        {
            // Arrange
            var filtros = CrearFiltrosUsuarioDTO();
            var usuarios = new List<Usuario> { CrearUsuarioValido() };

            //_mockMapper
            //    .Setup(s => s.BuscarUsuariosAsync(filtros))
            //    .ReturnsAsync(usuarios);

            // Act
            var result = await _controller.BuscarUsuarios(filtros);

            // Assert
            result.Result.Should().BeOfType<OkObjectResult>();
            var okResult = result.Result as OkObjectResult;

            var respuesta = okResult.Value as IEnumerable<RespuestaUsuarioDTO>;
            respuesta.Should().NotBeNull();
            respuesta.Should().HaveCount(1);
            respuesta.First().Nombre.Should().Be("Juan Pérez");

            _mockUsuarioService.Verify(s => s.BuscarUsuariosAsync(filtros), Times.Once);
        }

        [Fact]
        public async Task BuscarUsuarios_SinResultados_DeberiaRetornarListaVacia()
        {
            // Arrange
            var filtros = CrearFiltrosUsuarioDTO();
            var usuarios = new List<Usuario>();

            _mockUsuarioService
                .Setup(s => s.BuscarUsuariosAsync(filtros))
                .ReturnsAsync(usuarios);

            // Act
            var result = await _controller.BuscarUsuarios(filtros);

            // Assert
            result.Result.Should().BeOfType<OkObjectResult>();
            var okResult = result.Result as OkObjectResult;

            var respuesta = okResult.Value as IEnumerable<RespuestaUsuarioDTO>;
            respuesta.Should().NotBeNull();
            respuesta.Should().BeEmpty();
        }

        //#endregion

        //#region EditarUsuario Tests

        [Fact]
        public async Task EditarUsuario_ConDatosValidos_DeberiaRetornarOk()
        {
            // Arrange
            var id = 1;
            var dto = EditarUsuarioDTOValido();
            var usuario = CrearUsuarioValido();
            var usuarioEditado = CrearUsuarioValido();
            usuarioEditado.Nombre = dto.Nombre;
            usuarioEditado.Email = dto.Email;

            _mockEditarUsuarioValidator
                .Setup(v => v.ValidateAsync(dto, default))
                .ReturnsAsync(new ValidationResult());

            _mockUsuarioService
                .Setup(s => s.BuscarUsuarioPorIdAsync(id))
                .ReturnsAsync(usuario);

            _mockUsuarioService
                .Setup(s => s.EditarUsuarioAsync(usuario, dto))
                .ReturnsAsync(usuarioEditado);

            // Act
            var result = await _controller.EditarUsuario(id, dto, _mockEditarUsuarioValidator.Object);

            // Assert
            result.Result.Should().BeOfType<OkObjectResult>();
            var okResult = result.Result as OkObjectResult;

            var respuesta = okResult.Value as RespuestaUsuarioDTO;
            respuesta.Should().NotBeNull();
            respuesta.Nombre.Should().Be(dto.Nombre);
            respuesta.Email.Should().Be(dto.Email);

            _mockUsuarioService.Verify(s => s.BuscarUsuarioPorIdAsync(id), Times.Once);
            _mockUsuarioService.Verify(s => s.EditarUsuarioAsync(usuario, dto), Times.Once);
        }

        [Fact]
        public async Task EditarUsuario_UsuarioNoExiste_DeberiaRetornarNotFound()
        {
            // Arrange
            var id = 999;
            var dto = EditarUsuarioDTOValido();

            _mockEditarUsuarioValidator
                .Setup(v => v.ValidateAsync(dto, default))
                .ReturnsAsync(new ValidationResult());

            _mockUsuarioService
                .Setup(s => s.BuscarUsuarioPorIdAsync(id))
                .ReturnsAsync((Usuario)null);

            // Act
            var result = await _controller.EditarUsuario(id, dto, _mockEditarUsuarioValidator.Object);

            // Assert
            result.Result.Should().BeOfType<NotFoundResult>();

            _mockUsuarioService.Verify(s => s.BuscarUsuarioPorIdAsync(id), Times.Once);
            _mockUsuarioService.Verify(s => s.EditarUsuarioAsync(It.IsAny<Usuario>(), dto), Times.Never);
        }

        [Fact]
        public async Task EditarUsuario_ConDatosInvalidos_DeberiaRetornarBadRequest()
        {
            // Arrange
            var id = 1;
            var dto = new EditarUsuarioDTO { Nombre = "", Email = "email-invalido" };
            var validationResult = new ValidationResult(new[]
            {
                new ValidationFailure("Nombre", "El nombre es obligatorio."),
                new ValidationFailure("Email", "El email no tiene un formato válido.")
            });

            _mockEditarUsuarioValidator
                .Setup(v => v.ValidateAsync(dto, default))
                .ReturnsAsync(validationResult);

            // Act
            var result = await _controller.EditarUsuario(id, dto, _mockEditarUsuarioValidator.Object);

            // Assert
            result.Result.Should().BeOfType<BadRequestObjectResult>();

            _mockUsuarioService.Verify(s => s.BuscarUsuarioPorIdAsync(It.IsAny<int>()), Times.Never);
            _mockUsuarioService.Verify(s => s.EditarUsuarioAsync(It.IsAny<Usuario>(), dto), Times.Never);
        }

        //#endregion

        //#region EliminarUsuario Tests

        [Fact]
        public async Task EliminarUsuario_DeberiaRetornarNoContent()
        {
            // Arrange
            var id = 1;

            _mockUsuarioService
                .Setup(s => s.EliminarUsuarioAsync(id))
                .Returns(Task.CompletedTask);

            // Act
            var result = await _controller.EliminarUsuario(id);

            // Assert
            result.Should().BeOfType<NoContentResult>();

            _mockUsuarioService.Verify(s => s.EliminarUsuarioAsync(id), Times.Once);
        }

        //#endregion
    }
}