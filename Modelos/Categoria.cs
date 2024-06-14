using System.ComponentModel.DataAnnotations;

namespace ApiPeliculas.Modelos
{
    public class Categoria
    {
        [Key]
        public int id { get; set; }
        [Required]
        public string Nombre { get; set; }        
        public DateTime FechaCreacion { get; set; }
    }
}
