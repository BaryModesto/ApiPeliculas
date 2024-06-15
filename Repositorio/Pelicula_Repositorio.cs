using ApiPeliculas.Migraciones;
using ApiPeliculas.Modelos;
using ApiPeliculas.Repositorio.IRepositorio;
using Microsoft.EntityFrameworkCore;

namespace ApiPeliculas.Repositorio
{
    public class Pelicula_Repositorio : IPeliculaRepositorio
    {
        AplicationDbContext bd;
        public Pelicula_Repositorio(AplicationDbContext _bd)
        {
            bd = _bd;
        }
        public bool Actualizar_Peliculas(Pelicula _pelicula)
        {
            _pelicula.FechaCreacion = DateTime.Now;
            bd.Update(_pelicula);
            return Guardar();
        }

        public bool Borrar_Pelicula(Pelicula _pelicula)
        {
            bd.Remove(_pelicula);
            return Guardar();
        }

        public bool Crear_Pelicula(Pelicula _pelicula)
        {
            _pelicula.FechaCreacion = DateTime.Now;
            bd.Pelicula.Add(_pelicula);
            return Guardar();
        }

        public bool Existe_Pelicula(string _nombre)
        {
            bool encomtrado = false;
            try
            {
                encomtrado = bd.Pelicula.Any(x => x.Nombre.ToLower().Trim() == _nombre.ToLower().Trim());
            }
            catch (Exception)
            {
                return false;
            }
            return encomtrado;
        }
        public bool Existe_Pelicula(int _id)
        {
            if ( bd.Pelicula.Find(_id) == null)
            {
                return false;
            }
            return true;
        }

        public Pelicula GetPelicula(int _pelicula)
        {
            Pelicula cat = bd.Pelicula.Find(_pelicula);
            return cat;
        }

        public ICollection<Pelicula> Get_Peliculas()
        {
            return bd.Pelicula.OrderBy(x =>  x.Nombre).ToList();
        }

        public ICollection<Pelicula> Get_Peliculas_Categoria(int _catID)
        {
            return bd.Pelicula.Include(x=> x.Categoria).Where(x=> x.Id == _catID).OrderBy(x => x.Id).ToList();
        }

        public ICollection<Pelicula> Get_Peliculas_Nombre(string _nombre)
        {
            IQueryable<Pelicula> query = bd.Pelicula;
            if (!string.IsNullOrEmpty(_nombre))
            {
                query.Where(x => x.Nombre.Contains(_nombre) || x.Descripcion.Contains(_nombre));
            }
            return query.ToList();
        }

        public bool Guardar()
        {
            return (bd.SaveChanges() >0)? true:false;
        }
    }
}
