using ApiPeliculas.Modelos.Dtos;
using ApiPeliculas.Modelos;
using ApiPeliculas.Repositorio.IRepositorio;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace ApiPeliculas.Controllers
{
    [Route("api/usuarios")]
    [ApiController]
    public class UsuariosController : ControllerBase
    {
        IUsuarioRepositorio user_repo;
        IMapper mapper;
        protected RespiuestaAPI respuesta_api;
        public UsuariosController(IUsuarioRepositorio _user_repo,IMapper _mapper)
        {
            user_repo = _user_repo;
            mapper = _mapper;
            this.respuesta_api = new();
        }
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public IActionResult Get_Usuarios()
        {
            var lista = user_repo.Get_Usuarios();
            var list_dto = new List<UsuarioDto>();
            //---
            foreach (var i in lista)
            {                
                list_dto.Add(mapper.Map<UsuarioDto>(i));
            }
            return Ok(list_dto);
        }
        [HttpGet("{usuarioId}", Name = "Coger_Usuario")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult Get_Usuario(string usuarioId)
        {
            var especifico = user_repo.GetUsuario(usuarioId);
            if (especifico == null)
            {
                return NotFound();
            }
            var u_dto = mapper.Map<UsuarioDto>(especifico);
            return Ok(u_dto);
        }
        [HttpPost("Registro")]
        [ProducesResponseType(201, Type = typeof(UsuarioRegistroDto))]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Registro([FromBody] UsuarioRegistroDto _user_dto)
        {
            bool validar_nombre_usuario = user_repo.ES_Unico_Usuario(_user_dto.NombreUsuario);
            if (!validar_nombre_usuario)
            {
                respuesta_api.StatusCode = HttpStatusCode.BadRequest;
                respuesta_api.IsSucess = false;
                respuesta_api.ErrorMesasages.Add("El nombre d4e usuario ya existe");
                return BadRequest(respuesta_api);
            }
            var usuario = await user_repo.Registro(_user_dto);
            if (usuario == null)
            {
                respuesta_api.StatusCode = HttpStatusCode.BadRequest;
                respuesta_api.IsSucess = false;
                respuesta_api.ErrorMesasages.Add("El nombre d4e usuario ya existe");
                return BadRequest(respuesta_api);
            }

            respuesta_api.StatusCode = HttpStatusCode.OK;
            respuesta_api.IsSucess = true;            
            return Ok(respuesta_api);
        }

        [HttpPost("Login")]
        [ProducesResponseType(201, Type = typeof(UsuarioRegistroDto))]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Login([FromBody] UsuarioLoginDto _user_dto)
        {
            var respuesta = await user_repo.Login(_user_dto);
            
            
            if (respuesta == null || string.IsNullOrEmpty(respuesta.Token) )
            {
                respuesta_api.StatusCode = HttpStatusCode.BadRequest;
                respuesta_api.IsSucess = false;
                respuesta_api.ErrorMesasages.Add("El nombre de usuario o password son incorrectos");
                return BadRequest(respuesta_api);
            }           

            respuesta_api.StatusCode = HttpStatusCode.OK;
            respuesta_api.IsSucess = true;
            respuesta_api.Result = respuesta;
            return Ok(respuesta_api);
        }
    }
}
