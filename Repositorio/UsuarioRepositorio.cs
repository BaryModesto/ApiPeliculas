using ApiPeliculas.Migraciones;
using ApiPeliculas.Modelos;
using ApiPeliculas.Modelos.Dtos;
using ApiPeliculas.Repositorio.IRepositorio;

namespace ApiPeliculas.Repositorio
{
    public class UsuarioRepositorio : IUsuarioRepositorio
    {
        AplicationDbContext bd;
        public UsuarioRepositorio(AplicationDbContext _bd)
        {
            bd = _bd;
        }

        public bool ES_Unico_Usuario(string _nombre_usuario)
        {
            var result = bd.Usuario.FirstOrDefault(x => x.NombreUsuario == _nombre_usuario);
            if (result == null)
            {
                return true;
            }
            return false;
        }
        public Usuario GetUsuario(int _usuarioId)
        {
            return bd.Usuario.FirstOrDefault(x => x.Id == _usuarioId);
        }

        public ICollection<Usuario> Get_Usuarios()
        {
            return bd.Usuario.OrderBy(x => x.NombreUsuario).ToList();
        }

        public Task<UsuarioLoginRespuestaDto> Login(UsuarioLoginDto _user)
        {
            throw new NotImplementedException();
        }

        public Task<Usuario> Registro(UsuarioRegistroDto _user)
        {
            throw new NotImplementedException();
        }
    }
}
