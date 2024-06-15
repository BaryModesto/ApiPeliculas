using ApiPeliculas.Modelos.Dtos;
using ApiPeliculas.Modelos;
using ApiPeliculas.Repositorio.IRepositorio;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ApiPeliculas.Controllers
{
    [Route("api/peliculas")]
    [ApiController]
    public class PeliculasController : ControllerBase
    {
        IPeliculaRepositorio peli_repo;
        IMapper mapper;
        public PeliculasController(IPeliculaRepositorio _peli_repo,IMapper _mapper)
        {
            peli_repo = _peli_repo;
            mapper = _mapper;
        }

        [HttpPost]
        [ProducesResponseType(201, Type = typeof(Categoria_Dto))]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult Crear_Pelicula([FromBody] Crear_Categoria_Dto _cat_dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            if (_cat_dto == null)
            {
                return BadRequest(ModelState);
            }
            if (peli_repo.Existe_Pelicula(_cat_dto.Nombre))
            {
                ModelState.AddModelError("", "La categoria ya existe");
                return StatusCode(StatusCodes.Status404NotFound, ModelState);
            }
            Categoria categ = mapper.Map<Categoria>(_cat_dto);
            if (!peli_repo.Crear_Pelicula(categ))
            {
                ModelState.AddModelError("", $"Algo salio mal guardando el registro {_cat_dto.Nombre}");
                return StatusCode(StatusCodes.Status500InternalServerError, ModelState);
            }
            return Created();
            //return CreatedAtRoute("Get_Categoria",new { categoriaId = categ.id},categ)
        }

    }
}
