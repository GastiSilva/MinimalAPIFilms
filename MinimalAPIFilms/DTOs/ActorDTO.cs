namespace MinimalAPIFilms.DTOs
{
    public class ActorDTO
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public DateTime FechaNacimiento { get; set; }
        public string? Foto { get; set; }
    }
}
