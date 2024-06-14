using System.ComponentModel.DataAnnotations;

namespace ApiPeliculas.Modelos.Dtos
{
    public class Crear_Categoria_Dto
    {
        [Required(ErrorMessage ="El Nombre es obligatorio")]
        [MaxLength(60,ErrorMessage ="El # maximo de caracteres es 60")]
        public string Nombre { get; set; }               
    }
}
