using AutoMapper;
using MinimalAPIFilms.DTOs;
using MinimalAPIFilms.Entidades;

namespace MinimalAPIFilms.Utilidades
{
    public class AutoMapperProfiles : Profile
    {
        public AutoMapperProfiles() 
        {
            CreateMap<CrearGeneroDTO, Genero>();
            CreateMap<Genero, GeneroDTO>();
        }

    }
}
