using Microsoft.EntityFrameworkCore;

namespace MiApi.Infrastructure.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        // Ejemplo de DbSet (Reemplazá con tus entidades de Domain)
        // public virtual DbSet<TuEntidad> TusEntidades { get; set; }
    }
}
