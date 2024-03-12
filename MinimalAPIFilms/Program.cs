using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.OutputCaching;
using Microsoft.EntityFrameworkCore;
using MinimalAPIFilms;
using MinimalAPIFilms.Entidades;
using MinimalAPIFilms.Migrations;
using MinimalAPIFilms.Repository;

var builder = WebApplication.CreateBuilder(args);
var origenesPermitidos = builder.Configuration.GetValue<string>("origenesPermitidos")!;
// el "!" sirve para asegurar que siempre habra un avlor y no sera nulo

//inicio area de servicios
builder.Services.AddDbContext<ApplicationDbContext>(opciones =>
    opciones.UseSqlServer("name=DefaultConnection"));

builder.Services.AddCors(opciones =>
{
    opciones.AddDefaultPolicy(configuracion =>
    {
        configuracion.WithOrigins(origenesPermitidos).AllowAnyHeader().AllowAnyMethod();
    });

    opciones.AddPolicy("libre", configuracion =>
    {
        configuracion.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod();
    });
});
// conf cache
builder.Services.AddOutputCache();
//conf swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddScoped<IRepositoryGeneros, RepositoryGeneros>();
//fin area de serrvicios
var app = builder.Build();

// inicio area middlware

//para usar Swagger en desarollo y sin el if para usarlo normal
//if (builder.Environment.IsDevelopment()) {}
app.UseSwagger();
app.UseSwaggerUI();

app.UseCors();
app.UseOutputCache();

app.MapGet("/", () => "Hello World!");

var endpointGeneros = app.MapGroup("/generos");

endpointGeneros.MapGet("/", ObtenerGeneros).CacheOutput(c => c.Expire(TimeSpan.FromSeconds(60)).Tag("generos-get"));

// {id:int} indicar paramentro de url
endpointGeneros.MapGet("/{id:int}", ObtenerGenerosId);
//creo endpoint POST para crear genero

endpointGeneros.MapPost("/", async (Genero genero, IRepositoryGeneros Repository,
    IOutputCacheStore outputCacheStore) =>
{
    var id = await Repository.Crear(genero);
    await outputCacheStore.EvictByTagAsync("generos-get", default);
    return Results.Created($"/generos/{id}", genero);

});

endpointGeneros.MapPut("/{id:int}", async (int id, Genero genero, IRepositoryGeneros repository,
    IOutputCacheStore outputCacheStore) =>
    {
        var existe = await repository.Existe(id);
        if (!existe)
        {
            return Results.NotFound();
        }

        await repository.Actualizar(genero);
        await outputCacheStore.EvictByTagAsync("generos-get", default);
        return Results.NoContent();
    });

endpointGeneros.MapDelete("/{id:int}", async (int id, IRepositoryGeneros repository,
    IOutputCacheStore outputCacheStore) => 
    {
        var existe = await repository.Existe(id);
        if (!existe)
        {
            return Results.NotFound();
        }
        await repository.Borrar(id);
        await outputCacheStore.EvictByTagAsync("generos-get", default);
        return Results.NoContent();
    });

// fin area middlware
app.Run();

//el typedResults es para que en swager se ve la estructura
static async Task<Ok<List<Genero>>> ObtenerGeneros (IRepositoryGeneros Repository)
{
    var generos =  await Repository.ObtenerTodos();
    return TypedResults.Ok(generos);
}

static async Task<Results<Ok<Genero>, NotFound>> ObtenerGenerosId  (IRepositoryGeneros Repository, int id)
{
    var genero = await Repository.ObtenerPorId(id);
    if (genero is null)
    {
        return TypedResults.NotFound();
    }

    return TypedResults.Ok(genero);
}