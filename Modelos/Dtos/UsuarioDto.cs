using System.ComponentModel.DataAnnotations;

namespace ApiPeliculas.Modelos.Dtos
{
    public class UsuarioDto
    {        
        public int Id { get; set; }
        public string NombreUsuario { get; set; }
        public string NombreReal { get; set; }
        public string Password { get; set; }
        public string Role { get; set; }
    }
}
