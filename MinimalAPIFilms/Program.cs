using Microsoft.EntityFrameworkCore;
using MinimalAPIFilms;
using MinimalAPIFilms.Entidades;

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

app.MapGet("/generos", () =>
{
    var generos = new List<Genero>
    {
        new Genero { Id = 1,
                     Name = "Drama"},

         new Genero { Id = 2,
                     Name = "Accion"},

          new Genero { Id = 3,
                     Name = "Comedia"},
    };
    return generos;
}).CacheOutput(c => c.Expire(TimeSpan.FromSeconds(15)));

// fin area middlware
app.Run();

