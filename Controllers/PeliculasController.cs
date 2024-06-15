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
    }
}
