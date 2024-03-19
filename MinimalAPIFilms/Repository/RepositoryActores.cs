using Microsoft.EntityFrameworkCore;
using MinimalAPIFilms.Entities;

namespace MinimalAPIFilms.Repository
{
    public class RepositoryActores : IRepositoryActores
    {
        private ApplicationDbContext context;

        public RepositoryActores(ApplicationDbContext context)
        {
            this.context = context;
        }

        public async Task<List<Actor>> ObtenerTodos()
        {
            return await context.Actores.OrderBy(a => a.Name).ToListAsync();
        }
        //AsNoTracking indica que no le doy seguimiento al actor que retorne esto
        public async Task<Actor?> ObtenerPorId(int id)
        {
            return await context.Actores.AsNoTracking().FirstOrDefaultAsync(a => a.Id == id);
        }

        public async Task<int> Crear(Actor actor)
        {
            context.Add(actor);
            await context.SaveChangesAsync();
            return actor.Id;
        }

        public async Task<bool> Existe(int id)
        {
            return await context.Actores.AnyAsync(a => a.Id == id);
        }

        public async Task Actulizar(Actor actor)
        {
            context.Update(actor);
            await context.SaveChangesAsync();
        }

        public async Task Borrar(int id)
        {
            await context.Actores.Where(a => a.Id == id).ExecuteDeleteAsync();
        }
    }
}
