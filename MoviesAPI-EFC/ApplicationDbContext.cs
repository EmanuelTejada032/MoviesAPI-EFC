using Microsoft.EntityFrameworkCore;
using MoviesAPI_EFC.Entities;

namespace MoviesAPI_EFC
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions options) : base(options)
        {

        }

        public DbSet<Genre> Genres { get; set; }
    }
}
