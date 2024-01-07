using Microsoft.EntityFrameworkCore;
using SoundCanvas.Models;

namespace SoundCanvas.Data
{
    public class ApplicationDb : DbContext
    {
        public ApplicationDb(DbContextOptions<ApplicationDb> options) : base(options) { 
        }

        public DbSet<Genre> Genres { get; set; }
        public DbSet<Artist> Artists { get; set; }

    }
}
