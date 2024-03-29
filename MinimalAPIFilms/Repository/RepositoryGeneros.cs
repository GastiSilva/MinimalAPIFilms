﻿using Microsoft.EntityFrameworkCore;
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
        public  async Task Actualizar(Genero genero)
        {
            context.Update(genero);
            await context.SaveChangesAsync();
        }
        public async Task<bool> Existe(int id)
        {
            return await context.Generos.AnyAsync(x => x.Id == id);
        }

        public async Task<Genero?> ObtenerPorId(int id)
        {
            return await context.Generos.FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<List<Genero>> ObtenerTodos()
        {
            return await context.Generos.OrderBy(x => x.Name).ToListAsync();

        }

        public async Task Borrar(int id)
        {
            await context.Generos.Where(x => x.Id == id).ExecuteDeleteAsync();
        }
    }
}
