
using Microsoft.EntityFrameworkCore;

namespace MiApp.Infrastructure.Data
{
    public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : DbContext(options), DbContext
    {}
}