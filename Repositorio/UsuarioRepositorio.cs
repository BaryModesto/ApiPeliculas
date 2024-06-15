using ApiPeliculas.Migraciones;
using ApiPeliculas.Modelos;
using ApiPeliculas.Modelos.Dtos;
using ApiPeliculas.Repositorio.IRepositorio;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using XSystem.Security.Cryptography;

namespace ApiPeliculas.Repositorio
{
    public class UsuarioRepositorio : IUsuarioRepositorio
    {
        AplicationDbContext bd;
        string palabra_secreta;
        public UsuarioRepositorio(AplicationDbContext _bd,IConfiguration _conf)
        {
            bd = _bd;
            palabra_secreta = _conf.GetValue<string>("Api_Settings:Palabra_Secreta");// 
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

        public async Task<UsuarioLoginRespuestaDto> Login(UsuarioLoginDto _user)
        {
            var password_encriptado = obtenermd5(_user.Password);
            var usuario = bd.Usuario.FirstOrDefault(x => x.NombreUsuario.ToLower() == _user.NombreUsuario.ToLower() && x.Password == password_encriptado);
            if (usuario == null)
            {
                UsuarioLoginRespuestaDto respuesta = new UsuarioLoginRespuestaDto()
                {
                    Usuario = null,
                    Token = ""
                };
                return respuesta;
            }
            var manejador_token = new JwtSecurityTokenHandler();
            var Key = Encoding.ASCII.GetBytes(palabra_secreta);
            var token_descriptor = new SecurityTokenDescriptor()
            {
                Subject = new System.Security.Claims.ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, usuario.NombreUsuario.ToString()),
                    new Claim(ClaimTypes.Role, usuario.Role)
                }),
                Expires = DateTime.Now.AddDays(7),
                SigningCredentials = new(new SymmetricSecurityKey(Key),SecurityAlgorithms.HmacSha256Signature)
            };
            //---
            var token = manejador_token.CreateToken(token_descriptor);
            UsuarioLoginRespuestaDto usuario_login = new UsuarioLoginRespuestaDto()
            {
                Token = manejador_token.WriteToken(token),
                Usuario = usuario
            };
            return usuario_login;
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
