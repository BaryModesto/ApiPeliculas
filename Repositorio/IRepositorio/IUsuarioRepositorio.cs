using ApiPeliculas.Modelos;
using ApiPeliculas.Modelos.Dtos;

namespace ApiPeliculas.Repositorio.IRepositorio
{
    public interface IUsuarioRepositorio
    {
        ICollection<AppUsuario> Get_Usuarios();
        AppUsuario GetUsuario(string _usuarioId);
        bool ES_Unico_Usuario(string _nombre_usuario);
        Task<UsuarioLoginRespuestaDto> Login(UsuarioLoginDto _user);
        Task<UsuarioDatosDto> Registro(UsuarioRegistroDto _user);
    }
}
