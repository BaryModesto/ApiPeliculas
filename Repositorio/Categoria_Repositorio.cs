using ApiPeliculas.Migraciones;
using ApiPeliculas.Modelos;
using ApiPeliculas.Repositorio.IRepositorio;

namespace ApiPeliculas.Repositorio
{
    public class Categoria_Repositorio : ICategoriaRepositorio
    {
        AplicationDbContext bd;
        public Categoria_Repositorio(AplicationDbContext _bd)
        {
            bd = _bd;
        }
        public bool Actualizar_Categorias(Categoria _categoria)
        {
            _categoria.FechaCreacion = DateTime.Now;
            bd.Update(_categoria);
            return Guardar();
        }

        public bool Borrar_Categoria(Categoria _categoria)
        {
            bd.Remove(_categoria);
            return Guardar();
        }

        public bool Crear_Categoria(Categoria _categoria)
        {
            _categoria.FechaCreacion = DateTime.Now;
            bd.Categoria.Add(_categoria);
            return Guardar();
        }

        public bool Existe_Categorias(string _nombre)
        {
            bool encomtrado = false;
            try
            {
                encomtrado = bd.Categoria.Any(x => x.Nombre.ToLower().Trim() == _nombre.ToLower().Trim());
            }
            catch (Exception)
            {
                return false;
            }
            return encomtrado;
        }
        public bool Existe_Categorias(int _id)
        {
            if ( bd.Categoria.Find(_id) == null)
            {
                return false;
            }
            return true;
        }

        public Categoria GetCategoria(int _categoria)
        {
            Categoria cat = bd.Categoria.Find(_categoria);
            return cat;
        }

        public ICollection<Categoria> Get_Categorias()
        {
            return bd.Categoria.OrderBy(x =>  x.Nombre).ToList();
        }

        public bool Guardar()
        {
            return (bd.SaveChanges() >0)? true:false;
        }
    }
}
