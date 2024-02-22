using MinimalAPIFilms.Entidades;

namespace MinimalAPIFilms.Repository
{
    public class RepositoryGeneros : IRepositoryGeneros
    {
        private readonly ApplicationDbContext context;

        //creo para tenr acceso a un campo privado desde todos los metodos
        public RepositoryGeneros(ApplicationDbContext context)
        {
            this.context = context;
        }
        
        public async Task<int> Crear(Genero genero)
        {
            context.Add(genero);
            await context.SaveChangesAsync();
            return genero.Id;
        }
    }
}
