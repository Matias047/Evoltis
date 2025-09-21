using System.ComponentModel.DataAnnotations;

namespace Evoltis.Dto
{
    public class CrearUsuarioDTO
    {
        [Required]
        [MaxLength(50)]
        public string Nombre { get; set; }

        [Required]
        [MaxLength(50)]
        public string Email { get; set; }
    }
}
