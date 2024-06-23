using ApiPeliculas.Modelos;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace ApiPeliculas.Migraciones
{
    public class AplicationDbContext:IdentityDbContext<AppUsuario>
    {
        public AplicationDbContext(DbContextOptions<AplicationDbContext> options):base(options){}

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
        }

        public DbSet<Categoria> Categoria {  get; set; }
        public DbSet<Pelicula> Pelicula { get; set; }
        public DbSet<Usuario> Usuario { get; set; }
        public DbSet<AppUsuario> AppUsuario { get; set; }
        
    }
}
