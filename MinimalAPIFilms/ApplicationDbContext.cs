using Microsoft.EntityFrameworkCore;
using MinimalAPIFilms.Entidades;

namespace MinimalAPIFilms
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions options) : base(options)
        {

        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder); 
            // lo mismo que anotaciones de datos
            modelBuilder.Entity<Genero>().Property(p => p.Name).HasMaxLength(50);
        }


        public DbSet<Genero> Generos { get; set; }
    }
}
