using ApiPeliculas.Modelos;
using Microsoft.EntityFrameworkCore;

namespace ApiPeliculas.Migraciones
{
    public class AplicationDbContext:DbContext 
    {
        public AplicationDbContext(DbContextOptions<AplicationDbContext> options):base(options){}

        public DbSet<Categoria> Categorias {  get; set; }
    }
}
