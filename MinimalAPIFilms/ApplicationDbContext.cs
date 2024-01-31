using Microsoft.EntityFrameworkCore;

namespace MinimalAPIFilms
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions options) : base(options)
        {

        }
    }
}
