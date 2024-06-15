using ApiPeliculas.Modelos;

namespace ApiPeliculas.Repositorio.IRepositorio
{
    public interface IPeliculaRepositorio
    {
        ICollection<Pelicula> Get_Peliculas();
        Pelicula GetPelicula(int _categoria);
        bool Existe_Peliculas(string _nombre);
        bool Existe_Peliculas(int _id);
        bool Crear_Pelicula(Pelicula _pelicula);
        bool Actualizar_Peliculas(Pelicula _pelicula);
        bool Borrar_Pelicula(Pelicula _pelicula);
        ICollection<Pelicula> Get_Peliculas_Categoria(int _catID);
        ICollection<Pelicula> Get_Peliculas_Nombre(string _nombre);
        bool Guardar();
    }
}
