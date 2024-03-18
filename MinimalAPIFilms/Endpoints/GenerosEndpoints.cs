using AutoMapper;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.OutputCaching;
using MinimalAPIFilms.DTOs;
using MinimalAPIFilms.Entidades;
using MinimalAPIFilms.Repository;

namespace MinimalAPIFilms.Endpoints
{
    public static class GenerosEndpoints
    {
        public static RouteGroupBuilder MapGeneros(this RouteGroupBuilder group) 
        {
            group.MapGet("/", ObtenerGeneros).CacheOutput(c => c.Expire(TimeSpan.FromSeconds(60)).Tag("generos-get"));
            // {id:int} indicar paramentro de url
            group.MapGet("/{id:int}", ObtenerGenerosId);
            //creo endpoint POST para crear genero
            group.MapPost("/", CrearGeneros);
            group.MapPut("/{id:int}", ActualizarGenero);
            group.MapDelete("/{id:int}", EliminarGenero);
            return group;
        }

        static async Task<Ok<List<GeneroDTO>>> ObtenerGeneros(IRepositoryGeneros Repository, IMapper mapper)
        {
            var generos = await Repository.ObtenerTodos();
            var generosDTO = mapper.Map<List<GeneroDTO>>(generos);
            return TypedResults.Ok(generosDTO);
        }

        static async Task<Results<Ok<GeneroDTO>, NotFound>> ObtenerGenerosId(IRepositoryGeneros Repository,
            int id, IMapper mapper)
        {
            var genero = await Repository.ObtenerPorId(id);
            if (genero is null)
            {
                return TypedResults.NotFound();
            }
            var generoDTO = mapper.Map<GeneroDTO>(genero);   
            return TypedResults.Ok(generoDTO);
        }


        static async Task<Created<GeneroDTO>> CrearGeneros(CrearGeneroDTO crearGeneroDTO, 
            IRepositoryGeneros Repository,
            IOutputCacheStore outputCacheStore, IMapper mapper)
        {
            var genero = mapper.Map<Genero>(crearGeneroDTO);
            var id = await Repository.Crear(genero);
            await outputCacheStore.EvictByTagAsync("generos-get", default);
            var generoDTO = mapper.Map<GeneroDTO>(genero);
            return TypedResults.Created($"/generos/{id}", generoDTO);
        }

        static async Task<Results<NoContent, NotFound>> ActualizarGenero(int id, CrearGeneroDTO crearGeneroDTO,
            IRepositoryGeneros repository,
            IOutputCacheStore outputCacheStore, IMapper mapper)
        {
            var existe = await repository.Existe(id);
            if (!existe)
            {
                return TypedResults.NotFound();
            }
            var genero = mapper.Map<Genero>(crearGeneroDTO);
            genero.Id = id;
           
            await repository.Actualizar(genero);
            await outputCacheStore.EvictByTagAsync("generos-get", default);
            return TypedResults.NoContent();
        }

        static async Task<Results<NoContent, NotFound>> EliminarGenero(int id, IRepositoryGeneros repository,
            IOutputCacheStore outputCacheStore)
        {
            var existe = await repository.Existe(id);
            if (!existe)
            {
                return TypedResults.NotFound();
            }
            await repository.Borrar(id);
            await outputCacheStore.EvictByTagAsync("generos-get", default);
            return TypedResults.NoContent();
        }
    }

}
