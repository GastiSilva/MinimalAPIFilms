using Microsoft.EntityFrameworkCore;
using MinimalAPIFilms.Entidades;

namespace MinimalAPIFilms
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions options) : base(options)
        {

        }

        public DbSet<Genero> Generos { get; set; }
    }
}
