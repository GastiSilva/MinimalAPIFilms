using AutoMapper;
using MinimalAPIFilms.DTOs;
using MinimalAPIFilms.Entidades;
using MinimalAPIFilms.Entities;

namespace MinimalAPIFilms.Utilidades
{
    public class AutoMapperProfiles : Profile
    {
        public AutoMapperProfiles() 
        {
            CreateMap<CrearGeneroDTO, Genero>();
            CreateMap<Genero, GeneroDTO>();

            CreateMap<CrearActorDTO, Actor>().ForMember(a => a.Foto, opciones => opciones.Ignore());
            CreateMap<Actor, ActorDTO>();
        }

    }
}
