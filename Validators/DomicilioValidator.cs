using Evoltis.Dto;
using FluentValidation;

namespace Evoltis.Validators
{
    public class DomicilioValidator: AbstractValidator<DomicilioDTO>
    {
        public DomicilioValidator() 
        { 
            RuleFor(d => d.Calle)
                .NotEmpty().WithMessage("La calle es obligatoria.")
                .MaximumLength(50).WithMessage("La calle no puede superar los 50 caracteres");
            RuleFor(d => d.Numero)
                .MaximumLength(10).WithMessage("El número no puede superar los 10 caracteres");
            RuleFor(d => d.Provincia)
                .NotEmpty().WithMessage("La provincia es obligatoria.")
                .MaximumLength(50).WithMessage("La provincia no puede superar los 50 caracteres");
            RuleFor(d => d.Ciudad)
                .NotEmpty().WithMessage("La ciudad es obligatoria.")
                .MaximumLength(50).WithMessage("La ciudad no puede superar los 50 caracteres");
        }
    }
}
