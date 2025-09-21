using Evoltis.Dto;
using FluentValidation;

namespace Evoltis.Validators
{
    public class UsuarioValidator : AbstractValidator<CrearUsuarioDTO>
    {
        public UsuarioValidator()
        {
            RuleFor(u => u.Nombre)
                .NotEmpty().WithMessage("El nombre es obligatorio.")
                .MaximumLength(50).WithMessage("El nombre no puede superar los 50 caracteres");

            RuleFor(u => u.Email)
                .NotEmpty().WithMessage("El email es obligatorio.")
                .EmailAddress().WithMessage("El email no tiene un formato válido.")
                .MaximumLength(50).WithMessage("El email no puede superar los 50 caracteres");
        }
    }
}
