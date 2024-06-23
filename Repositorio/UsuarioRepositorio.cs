using ApiPeliculas.Migraciones;
using ApiPeliculas.Modelos;
using ApiPeliculas.Modelos.Dtos;
using ApiPeliculas.Repositorio.IRepositorio;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using XSystem.Security.Cryptography;

namespace ApiPeliculas.Repositorio
{    public class UsuarioRepositorio : IUsuarioRepositorio
    {
        AplicationDbContext bd;
        string palabra_secreta;
        private readonly UserManager<AppUsuario> userManager;
        private readonly RoleManager<IdentityRole> roleManager;
        private readonly IMapper mapper;

        public UsuarioRepositorio(AplicationDbContext _bd,IConfiguration _conf, 
            RoleManager<IdentityRole> _roleManager, UserManager<AppUsuario> _userManager,
            IMapper _mapper)
        {
            bd = _bd;
            palabra_secreta = _conf.GetValue<string>("Api_Settings:Palabra_Secreta");// 
            roleManager = _roleManager;
            userManager = _userManager;
            mapper = _mapper;
        }

        public bool ES_Unico_Usuario(string _nombre_usuario)
        {
            var result = bd.AppUsuario.FirstOrDefault(x => x.UserName== _nombre_usuario);
            if (result == null)
            {
                return true;
            }
            return false;
        }
        public AppUsuario GetUsuario(string _usuarioId)
        {
            return bd.AppUsuario.FirstOrDefault(x => x.Id == _usuarioId);
        }

        public ICollection<AppUsuario> Get_Usuarios()
        {
            return bd.AppUsuario.OrderBy(x => x.UserName).ToList();
        }

        public async Task<UsuarioLoginRespuestaDto> Login(UsuarioLoginDto _user)
        {
            //var password_encriptado = obtenermd5(_user.Password);
            var usuario = bd.AppUsuario.FirstOrDefault(x => x.UserName.ToLower() == _user.NombreUsuario.ToLower());

            bool es_valida = await userManager.CheckPasswordAsync(usuario,_user.Password);
            if (usuario == null || !es_valida )
            {
                UsuarioLoginRespuestaDto respuesta = new UsuarioLoginRespuestaDto()
                {
                    Usuario = null,
                    Token = ""
                };
                return respuesta;
            }

            var roles = await userManager.GetRolesAsync(usuario);

            var manejador_token = new JwtSecurityTokenHandler();
            var Key = Encoding.ASCII.GetBytes(palabra_secreta);
            var token_descriptor = new SecurityTokenDescriptor()
            {
                Subject = new System.Security.Claims.ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, usuario.UserName.ToString()),
                    new Claim(ClaimTypes.Role, roles.FirstOrDefault())
                }),
                Expires = DateTime.Now.AddDays(7),
                SigningCredentials = new(new SymmetricSecurityKey(Key),SecurityAlgorithms.HmacSha256Signature)
            };
            //---
            var token = manejador_token.CreateToken(token_descriptor);
            UsuarioLoginRespuestaDto usuario_login = new UsuarioLoginRespuestaDto()
            {
                Token = manejador_token.WriteToken(token),
                Usuario = mapper.Map<UsuarioDatosDto>(usuario),
            };
            return usuario_login;
        }

        public async Task<UsuarioDatosDto> Registro(UsuarioRegistroDto _user)
        {
            //var password_encriptado = obtenermd5(_user.Password);

            AppUsuario usuario = new AppUsuario()
            {
                UserName= _user.NombreUsuario,
                Email = _user.NombreUsuario,
                NormalizedEmail = _user.NombreUsuario.ToUpper(),
                Nombre = _user.NombreReal
            };
            var result = await userManager.CreateAsync(usuario,_user.Password);
            if (result.Succeeded)
            {
                if (!roleManager.RoleExistsAsync("admin").GetAwaiter().GetResult())
                {
                    await roleManager.CreateAsync(new IdentityRole("admin") ) ;
                    await roleManager.CreateAsync(new IdentityRole("registrado"));
                }
                await userManager.AddToRoleAsync(usuario, "admin");
                var usuario_retornado = bd.AppUsuario.FirstOrDefault(x => x.UserName == _user.NombreUsuario);

                //---una forma de hacerlo
                //return new UsuarioDatosDto()
                //{
                //    Id = usuario_retornado.Id,
                //    UserName = usuario_retornado.UserName,
                //    Nombre = usuario_retornado.Nombre
                //};
                return mapper.Map<UsuarioDatosDto>(usuario_retornado);

            }
            //bd.Usuario.Add(usuario);
            //await bd.SaveChangesAsync();
            //return usuario;
            return new UsuarioDatosDto();
        }
        //public static string obtenermd5(string valor)
        //{
        //    MD5CryptoServiceProvider x = new MD5CryptoServiceProvider();
        //    byte[] data = System.Text.Encoding.UTF8.GetBytes(valor);
        //    data = x.ComputeHash(data);
        //    string resp = "";
        //    for (int i = 0; i < data.Length; i++)
        //        resp += data[i].ToString("x2").ToLower();
        //    return resp;
        //}
    }
}
