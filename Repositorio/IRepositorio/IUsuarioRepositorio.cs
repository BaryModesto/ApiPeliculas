using ApiPeliculas.Modelos;
using ApiPeliculas.Modelos.Dtos;

namespace ApiPeliculas.Repositorio.IRepositorio
{
    public interface IUsuarioRepositorio
    {
        ICollection<Usuario> Get_Usuarios();
        Usuario GetUsuario(int _usuarioId);
        bool ES_Unico_Usuario(string _nombre_usuario);
        Task<UsuarioLoginRespuestaDto> Login(UsuarioLoginDto _user);
        Task<Usuario> Registro(UsuarioRegistroDto _user);
    }
}
