using Evoltis.Dto;
using FluentValidation;

namespace Evoltis.Validators
{
    public class EditarUsuarioValidator : AbstractValidator<EditarUsuarioDTO>
    {
        public EditarUsuarioValidator()
        {
            // Validaciones para los campos del usuario
            RuleFor(u => u.Nombre)
                .NotEmpty().WithMessage("El nombre es obligatorio.")
                .MaximumLength(50).WithMessage("El nombre no puede superar los 50 caracteres");

            RuleFor(u => u.Email)
                .NotEmpty().WithMessage("El email es obligatorio.")
                .EmailAddress().WithMessage("El email no tiene un formato válido.")
                .MaximumLength(50).WithMessage("El email no puede superar los 50 caracteres");

            // Validación condicional del domicilio
            RuleFor(u => u.Domicilio)
                .SetValidator(new DomicilioValidator())
                .When(u => u.Domicilio != null);
        }
    }
}
