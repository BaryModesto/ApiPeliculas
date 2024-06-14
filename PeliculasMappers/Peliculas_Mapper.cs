using ApiPeliculas.Modelos;
using ApiPeliculas.Modelos.Dtos;
using AutoMapper;

namespace ApiPeliculas.PeliculasMapper
{
    public class Peliculas_Mapper: Profile
    {
        public Peliculas_Mapper()
        {
            CreateMap<Categoria, Categoria_Dto>().ReverseMap();
            CreateMap<Categoria, Crear_Categoria_Dto>().ReverseMap();
        }
    }
}
