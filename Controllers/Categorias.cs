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
        }
    }
}
