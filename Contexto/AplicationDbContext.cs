using ApiPeliculas.Modelos;
using Microsoft.EntityFrameworkCore;

namespace ApiPeliculas.Migraciones
{
    public class AplicationDbContext:DbContext 
    {
        public AplicationDbContext(DbContextOptions<AplicationDbContext> options):base(options){}

        public DbSet<Categoria> Categoria {  get; set; }
        public DbSet<Pelicula> Pelicula { get; set; }
        public DbSet<Usuario> Usuario { get; set; }
    }
}
