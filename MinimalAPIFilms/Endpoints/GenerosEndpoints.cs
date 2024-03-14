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

        static async Task<Ok<List<Genero>>> ObtenerGeneros(IRepositoryGeneros Repository)
        {
            var generos = await Repository.ObtenerTodos();
            return TypedResults.Ok(generos);
        }

        static async Task<Results<Ok<Genero>, NotFound>> ObtenerGenerosId(IRepositoryGeneros Repository, int id)
        {
            var genero = await Repository.ObtenerPorId(id);
            if (genero is null)
            {
                return TypedResults.NotFound();
            }

            return TypedResults.Ok(genero);
        }

        static async Task<Created<GeneroDTO>> CrearGeneros(CrearGeneroDTO crearGeneroDTO, IRepositoryGeneros Repository, IOutputCacheStore outputCacheStore)
        {
            var genero = new Genero
            {
                Name = crearGeneroDTO.Nombre
            };
            var id = await Repository.Crear(genero);
            await outputCacheStore.EvictByTagAsync("generos-get", default);

            var generoDTO = new GeneroDTO
            {
                Id = id,
                Name = crearGeneroDTO.Nombre
            };
            return TypedResults.Created($"/generos/{id}", generoDTO);
        }

        static async Task<Results<NoContent, NotFound>> ActualizarGenero(int id, Genero genero, IRepositoryGeneros repository,
            IOutputCacheStore outputCacheStore)
        {
            var existe = await repository.Existe(id);
            if (!existe)
            {
                return TypedResults.NotFound();
            }

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
