using System.ComponentModel.DataAnnotations;

namespace ApiPeliculas.Modelos
{
    public class Usuario
    {
        [Key]
        public int Id { get; set; }
        public int NombreUsuario { get; set; }
        public string NombreReal { get; set; }
        public string Password{ get; set; }
        public string Role{ get; set; }
    }   
}
