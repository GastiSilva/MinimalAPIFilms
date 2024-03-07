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

app.MapGet("/generos", async (IRepositoryGeneros Repository) =>
{
    return await Repository.ObtenerTodos();
}).CacheOutput(c => c.Expire(TimeSpan.FromSeconds(60)).Tag("generos-get"));

// {id:int} indicar paramentro de url
app.MapGet("/generos/{id:int}", async (IRepositoryGeneros Repository, int id) =>
{
    var genero = await Repository.ObtenerPorId(id);
    if (genero is null)
    {
        return Results.NotFound();
    }

    return Results.Ok(genero);
});
//creo endpoint POST para crear genero

app.MapPost("/generos", async (Genero genero, IRepositoryGeneros Repository,
    IOutputCacheStore outputCacheStore) =>
{
    var id = await Repository.Crear(genero);
    await outputCacheStore.EvictByTagAsync("generos-get", default);
    return Results.Created($"/generos/{id}", genero);

});

app.MapPut("/generos/{id:int}", async (int id, Genero genero, IRepositoryGeneros repository,
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

app.MapDelete("/generos/{id:int}", async (int id, IRepositoryGeneros repository,
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

