using Microsoft.Win32;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Evoltis.Models
{
    public class Domicilio
    {
        //ID(int) : Identificador único del Domicilio(clave primaria).
        //UsuarioID(int) : Clave foránea para relacionar con Usuario.
        //Calle(string): Nombre de la calle del domicilio.
        //Numero(string): Número del domicilio.
        //Provincia(string): Provincia del domicilio.
        //Ciudad(string): Ciudad del domicilio.
        //FechaCreacion(DateTime): Fecha de creación del registro de domicilio.
        public int ID { get; set; }
        public int UsuarioID { get; set; }
        public string Calle { get; set; }
        public string Numero { get; set; }
        public string Provincia { get; set; }
        public string Ciudad { get; set; }
        public DateTime FechaCreacion { get; set; }
        public Usuario Usuario { get; set; }
    }
}
