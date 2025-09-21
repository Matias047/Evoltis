using System.ComponentModel.DataAnnotations;

namespace Evoltis.Dto
{
    public class EditarUsuarioDTO
    {
        [MaxLength(50)]
        public string Nombre { get; set; }

        [MaxLength(50)]
        public string Email { get; set; }

        public DomicilioDTO Domicilio { get; set; }
    }
}
