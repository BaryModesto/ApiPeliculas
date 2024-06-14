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
    }
}
