using ApiPeliculas.Migraciones;
using ApiPeliculas.Modelos;
using ApiPeliculas.Modelos.Dtos;
using ApiPeliculas.PeliculasMapper;
using ApiPeliculas.Repositorio.IRepositorio;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ApiPeliculas.Controllers
{
    [Route("api/categorias")]
    [ApiController]
    public class CategoriasController : ControllerBase
    {
        readonly ICategoriaRepositorio categ_repo;
        readonly IMapper mapper;

        public CategoriasController(ICategoriaRepositorio _cat_repo, IMapper _mapper)
        {
            categ_repo = _cat_repo;
            mapper = _mapper;   
        }
        //---
        [HttpGet]
        [ResponseCache(CacheProfileName = "PorDefecto30")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public IActionResult Get_Categorias()
        {
            ICollection<Categoria> lista_categ = categ_repo.Get_Categorias();
            List<Categoria_Dto> list_cat_dto = new List<Categoria_Dto>();
            //---
            foreach (var  i in lista_categ)
            {                
                Categoria_Dto cat_dto = mapper.Map<Categoria_Dto>(i);
                list_cat_dto.Add(cat_dto);
            }            
            return Ok(list_cat_dto);        
        }
        [HttpGet("{categoriaId:int}", Name ="Coger_Categoria")]
        [ResponseCache(CacheProfileName = "PorDefecto30")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult Get_Categoria(int categoriaId)
        {
            Categoria categoria_especifica = categ_repo.GetCategoria(categoriaId);
            if (categoria_especifica == null)
            {
                return NotFound();
            }
            Categoria_Dto cat_dto = mapper.Map<Categoria_Dto>(categoria_especifica);
            return Ok(cat_dto);
        }
        [HttpPost]
        [ProducesResponseType(201,Type =typeof (Categoria_Dto))]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult Crear_Categoria([FromBody]Crear_Categoria_Dto _cat_dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            if(_cat_dto == null)
            {
                return BadRequest(ModelState);
            }
            if (categ_repo.Existe_Categorias(_cat_dto.Nombre))
            {
                ModelState.AddModelError("", "La categoria ya existe");
                return StatusCode(StatusCodes.Status404NotFound,ModelState);
            }
            Categoria categ = mapper.Map<Categoria>(_cat_dto);
            if( !categ_repo.Crear_Categoria(categ) )
            {
                ModelState.AddModelError("", $"Algo salio mal guardando el registro {_cat_dto.Nombre}");
                return StatusCode(StatusCodes.Status500InternalServerError, ModelState);
            }
            return Created();
            //return CreatedAtRoute("Get_Categoria",new { categoriaId = categ.id},categ)
        }
        //---
        [HttpPatch("{categoriaID:int}", Name = "Actualizar_Patch_Categoria")]
        [ProducesResponseType(201,Type = typeof(Categoria_Dto))]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult Actualizar_Patch_Categoria(int categoriaID,[FromBody]Categoria_Dto _cat_dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            if (_cat_dto == null || _cat_dto.id != categoriaID)
            {
                return BadRequest(ModelState);
            }
            Categoria categ = mapper.Map<Categoria>(_cat_dto);
            if (!categ_repo.Actualizar_Categorias(categ))
            {
                ModelState.AddModelError("", $"Algo salio mal actualizando el registro {_cat_dto.Nombre}");
                return StatusCode(StatusCodes.Status500InternalServerError, ModelState);
            }
            return NoContent();
        }
        //---
        [HttpDelete("{categoriaID}",Name ="Borrar_Categoria")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]

        public IActionResult Borrar_Categoria(int categoriaID)
        {
            if (!categ_repo.Existe_Categorias(categoriaID) )
            {
                return NotFound(categoriaID);
            }
            Categoria categ = categ_repo.GetCategoria(categoriaID);
            if (!categ_repo.Borrar_Categoria(categ))
            {
                ModelState.AddModelError("", $"Algo salio mal borrando el registro {categ.Nombre}");
                return StatusCode(StatusCodes.Status500InternalServerError, ModelState);
            }
            return NoContent();
        }
    }
}
