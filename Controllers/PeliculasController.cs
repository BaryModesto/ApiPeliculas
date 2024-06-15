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
        public PeliculasController(IPeliculaRepositorio _peli_repo, IMapper _mapper)
        {
            peli_repo = _peli_repo;
            mapper = _mapper;
        }
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public IActionResult Get_Peliculas()
        {
            ICollection<Pelicula> lista_peli = peli_repo.Get_Peliculas();
            List<Pelicula_Dto> list_peli_dto = new List<Pelicula_Dto>();
            //---
            foreach (var i in lista_peli)
            {
                list_peli_dto.Add(mapper.Map<Pelicula_Dto>(i));
            }
            return Ok(list_peli_dto);
        }
        
        [HttpGet("{pelicula_id:int}", Name = "Coger_Pelicula")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult Get_Pelicula(int pelicula_id)
        {
            Pelicula peli_especifica = peli_repo.GetPelicula(pelicula_id);
            if (peli_especifica == null)
            {
                return NotFound();
            }
            return Ok(mapper.Map<Pelicula_Dto>(peli_especifica));
        }
        
        [HttpPost]
        [ProducesResponseType(201, Type = typeof(Categoria_Dto))]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult Crear_Pelicula([FromBody] Pelicula_Dto _peli_dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            if (_peli_dto == null)
            {
                return BadRequest(ModelState);
            }
            if (peli_repo.Existe_Pelicula(_peli_dto.Nombre))
            {
                ModelState.AddModelError("", "La pelicula ya existe");
                return StatusCode(StatusCodes.Status404NotFound, ModelState);
            }
            Pelicula p = mapper.Map<Pelicula>(_peli_dto);
            if (!peli_repo.Crear_Pelicula(p))
            {
                ModelState.AddModelError("", $"Algo salio mal guardando el registro {_peli_dto.Nombre}");
                return StatusCode(StatusCodes.Status500InternalServerError, ModelState);
            }
            return Created();
            //return CreatedAtRoute("Get_Categoria",new { categoriaId = categ.id},categ)            
        }
        
        [HttpPatch("{pelicula_id:int}", Name = "Actualizar_Patch_Pelicula")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult Actualizar_Patch_Pelicula(int pelicula_id, [FromBody] Pelicula_Dto _peli_dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var peli = mapper.Map<Pelicula>(_peli_dto);
            if (!peli_repo.Actualizar_Peliculas(peli))
            {
                ModelState.AddModelError("", $"Algo salio mal actualizando el registro {_peli_dto.Nombre}");
                return StatusCode(StatusCodes.Status500InternalServerError, ModelState);
            }
            return NoContent();
        }
        
        [HttpDelete("{pelicula_id}", Name = "Borrar_Pelicula")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult Borrar_Categoria(int pelicula_id)
        {
            if (!peli_repo.Existe_Pelicula(pelicula_id))
            {
                return NotFound(pelicula_id);
            }
            var result = peli_repo.GetPelicula(pelicula_id);
            if (!peli_repo.Borrar_Pelicula(result))
            {
                ModelState.AddModelError("", $"Algo salio mal borrando el registro {result.Nombre}");
                return StatusCode(StatusCodes.Status500InternalServerError, ModelState);
            }
            return NoContent();
        }
        
        [HttpGet("Get_Peliculas_Categoria/{categoria_id:int}")]
        public IActionResult Get_Peliculas_Categoria(int categoria_id)
        {
            ICollection<Pelicula> lista_peli = peli_repo.Get_Peliculas_Categoria(categoria_id);
            if (lista_peli == null || !lista_peli.Any())
            {
                return NotFound();
            }
            var list_peli_dto = new List<Pelicula_Dto>();
            //---
            foreach (var i in lista_peli)
            {
                list_peli_dto.Add(mapper.Map<Pelicula_Dto>(i));
            }
            return Ok(list_peli_dto);
        }
        
        [HttpGet("Get_Peliculas_Nombre")]
        public IActionResult Get_Peliculas_Nombre(string categoria_nombre)
        {
            try
            {
                var lista_peli = peli_repo.Get_Peliculas_Nombre(categoria_nombre.Trim());
                if (lista_peli.Any())
                {
                    return Ok(lista_peli);
                }
                return NotFound();
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Error recuperando datos");
            }
        }
        
    }
}
