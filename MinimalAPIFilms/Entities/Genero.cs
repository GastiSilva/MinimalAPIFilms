using System.ComponentModel.DataAnnotations;

namespace MinimalAPIFilms.Entidades
{
    public class Genero
    {
        public int Id { get; set; }
        //anotaciones de datos
        [StringLength(50)]
        public string Name { get; set; } = null!;
    }
}
