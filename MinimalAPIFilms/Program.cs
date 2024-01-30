using MinimalAPIFilms.Entidades;

var builder = WebApplication.CreateBuilder(args);
var origenesPermitidos = builder.Configuration.GetValue<string>("origenesPermitidos")!;
// el "!" sirve para asegurar que siempre habra un avlor y no sera nulo

//inicio area de servicios

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

//fin area de serrvicios
var app = builder.Build();

// inicio area middlware
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
});

// fin area middlware
app.Run();

