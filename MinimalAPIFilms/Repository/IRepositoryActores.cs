using MinimalAPIFilms.Entities;

namespace MinimalAPIFilms.Repository
{
    public interface IRepositoryActores
    {
        Task Actulizar(Actor actor);
        Task Borrar(int id);
        Task<int> Crear(Actor actor);
        Task<bool> Existe(int id);
        Task<Actor?> ObtenerPorId(int id);
        Task<List<Actor>> ObtenerTodos();
    }
}