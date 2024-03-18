using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.OutputCaching;
using Microsoft.EntityFrameworkCore;
using MinimalAPIFilms;
using MinimalAPIFilms.Endpoints;
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

builder.Services.AddAutoMapper(typeof(Program));
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

app.MapGroup("/generos").MapGeneros();


// fin area middlware
app.Run();

