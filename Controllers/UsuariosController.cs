using ApiPeliculas.Modelos.Dtos;
using ApiPeliculas.Modelos;
using ApiPeliculas.Repositorio.IRepositorio;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ApiPeliculas.Controllers
{
    [Route("api/usuarios")]
    [ApiController]
    public class UsuariosController : ControllerBase
    {
        IUsuarioRepositorio user_repo;
        IMapper mapper;
        public UsuariosController(IUsuarioRepositorio _user_repo,IMapper _mapper)
        {
            user_repo = _user_repo;
            mapper = _mapper;
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
    }
}
