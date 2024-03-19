using Microsoft.EntityFrameworkCore;
using MinimalAPIFilms.Entidades;
using MinimalAPIFilms.Entities;

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

            modelBuilder.Entity<Actor>().Property(p => p.Name).HasMaxLength(50);
            modelBuilder.Entity<Actor>().Property(p => p.Foto).IsUnicode();
        }


        public DbSet<Genero> Generos { get; set; }
        public DbSet<Actor> Actores { get; set; }


    }
}
