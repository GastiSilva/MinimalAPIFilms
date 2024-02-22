using MinimalAPIFilms.Entidades;

namespace MinimalAPIFilms.Repository
{
    public interface IRepositoryGeneros
    {
        Task<int> Crear(Genero genero);
    }
}
