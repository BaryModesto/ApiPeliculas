using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ApiPeliculas.Modelos.Dtos
{
    public class Pelicula_Dto
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "El Nombre es obligatorio")]
        public string Nombre { get; set; }
        public string RutaImagen { get; set; }
        [Required(ErrorMessage = "La Descripcion es obligatorio")]
        public string Descripcion { get; set; }
        [Required(ErrorMessage = "La Duracion es obligatorio")]
        public int Duracion { get; set; }
        public enum TipoClasificacion
        {
            Siete,
            Trece,
            Dieciseis,
            Dieciocho
        }
        public TipoClasificacion Clasificacion { get; set; }
        public DateTime FechaCreacion { get; set; }
        public int CategoriaID { get; set; }

    }
}
