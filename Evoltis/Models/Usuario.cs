namespace Evoltis.Models
{
    public class Usuario
    {
        //ID(int) : Identificador único del usuario(clave primaria).
        //Nombre(string) : Nombre del usuario.
        //Email(string): Dirección de correo electrónico del usuario.
        //FechaCreacion(DateTime): Fecha de creación del usuario.
        public int ID { get; set; }
        public string Nombre { get; set; }
        public string Email { get; set; }
        public DateTime FechaCreacion { get; set; }
        public Domicilio Domicilio { get; set; }
    }
}
