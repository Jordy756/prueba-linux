using Microsoft.EntityFrameworkCore;
using MiApi.Domain.Entities;

namespace MiApi.Infrastructure.Data
{
    public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : DbContext(options)
    {
        public DbSet<User> Users { get; set; }
    }
}
