namespace Evoltis.Dto
{
    public class RespuestaUsuarioDTO
    {
        public int ID { get; set; }
        public string Nombre { get; set; }
        public string Email { get; set; }
        public DateTime FechaCreacion { get; set; }
        public DomicilioDTO Domicilio { get; set; }
    }
}
