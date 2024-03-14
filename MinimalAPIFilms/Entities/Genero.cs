using System.ComponentModel.DataAnnotations;

namespace MinimalAPIFilms.Entidades
{
    public class Genero
    {
        public int Id { get; set; }
        //anotaciones de datos
        public string Name { get; set; } = null!;
    }
}
