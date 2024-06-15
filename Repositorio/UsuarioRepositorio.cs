using ApiPeliculas.Migraciones;
using ApiPeliculas.Modelos;
using ApiPeliculas.Modelos.Dtos;
using ApiPeliculas.Repositorio.IRepositorio;
using XSystem.Security.Cryptography;

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

        public async Task<Usuario> Registro(UsuarioRegistroDto _user)
        {
            var password_encriptado = obtenermd5(_user.Password);
            Usuario usuario = new Usuario()
            {
                NombreUsuario = _user.NombreUsuario,
                Password = password_encriptado,
                NombreReal = _user.NombreReal,
                Role = _user.Role
            };
            bd.Usuario.Add(usuario);
            await bd.SaveChangesAsync();
            return usuario;
        }
        public static string obtenermd5(string valor)
        {
            MD5CryptoServiceProvider x = new MD5CryptoServiceProvider();
            byte[] data = System.Text.Encoding.UTF8.GetBytes(valor);
            data = x.ComputeHash(data);
            string resp = "";
            for (int i = 0; i < data.Length; i++)
                resp += data[i].ToString("x2").ToLower();
            return resp;
        }
    }
}
