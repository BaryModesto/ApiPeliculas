using ApiPeliculas.Modelos;

namespace ApiPeliculas.Repositorio.IRepositorio
{
    public interface ICategoriaRepositorio
    {
        ICollection<Categoria> Get_Categorias();
        Categoria GetCategoria(int _categoria);
        bool Existe_Categorias(string _nombre);
        bool Crear_Categoria(Categoria _categoria);
        bool Actualizar_Categorias(Categoria _categoria);
        bool Borrar_Categoria(Categoria _caategoria);
        bool Guardar();
    }
}
