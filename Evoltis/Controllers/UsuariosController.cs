using AutoMapper;
using Evoltis.Data;
using Evoltis.Dto;
using Evoltis.Models;
using Evoltis.Services;
using Evoltis.Validators;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;

namespace Evoltis.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsuariosController : ControllerBase
    {
        private readonly IUsuarioService _usuariosService;
        private readonly IMapper _mapper;

        public UsuariosController(IUsuarioService usuariosService, IMapper mapper)
        {
            _usuariosService = usuariosService;
            _mapper = mapper;
        }

        [HttpPost]
        public async Task<ActionResult<RespuestaUsuarioDTO>> CrearUsuario(
        [FromBody] CrearUsuarioDTO dto,
        [FromServices] IValidator<CrearUsuarioDTO> validator)
        {
            var validationResult = await validator.ValidateAsync(dto);

            if (!validationResult.IsValid)
            {
                var errores = validationResult.Errors.Select(e => e.ErrorMessage);
                return BadRequest(new { success = false, errors = errores });
            }

            var usuario = _mapper.Map<Usuario>(dto);
            await _usuariosService.CrearUsuarioAsync(usuario);
            var response = _mapper.Map<RespuestaUsuarioDTO>(usuario);
            return Ok(response);
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<RespuestaUsuarioDTO>>> BuscarUsuarios(
        [FromQuery] FiltrosUsuarioDTO filtros)
        {
            var usuarios = await _usuariosService.BuscarUsuariosAsync(filtros);

            return Ok(_mapper.Map<IEnumerable<RespuestaUsuarioDTO>>(usuarios));
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<RespuestaUsuarioDTO>> EditarUsuario(
        int id,
        [FromBody] EditarUsuarioDTO dto,
        [FromServices] IValidator<EditarUsuarioDTO> validator)
        {
            var validationResult = await validator.ValidateAsync(dto);

            if (!validationResult.IsValid)
            {
                var errores = validationResult.Errors.Select(e => e.ErrorMessage);
                return BadRequest(new { success = false, errors = errores });
            }

            var usuario = await _usuariosService.BuscarUsuarioPorIdAsync(id);
            if (usuario == null) return NotFound();

            var response = await _usuariosService.EditarUsuarioAsync(usuario, dto);
            return Ok(_mapper.Map<RespuestaUsuarioDTO>(response));
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> EliminarUsuario(int id)
        {
            await _usuariosService.EliminarUsuarioAsync(id);
            return NoContent();
        }

    }
}
