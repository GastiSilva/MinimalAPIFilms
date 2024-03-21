namespace MinimalAPIFilms.DTOs
{
    public class CrearActorDTO
    {
        public string Name { get; set; } = null!;
        public DateTime FechaNacimiento { get; set; }
        // Iformfile es para la representacion de un archivo
        public IFormFile? Foto { get; set; }
    }
}
