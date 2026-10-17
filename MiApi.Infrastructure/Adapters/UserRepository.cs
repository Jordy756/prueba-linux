using MiApi.Domain.Entities;
using MiApi.Domain.Ports;
using MiApi.Infrastructure.Data;

namespace MiApi.Infrastructure.Adapters
{
  public class UserRepository(ApplicationDbContext context) : IUserRepository
  {
    private readonly ApplicationDbContext _context = context;

    public async Task AddAsync(User user)
    {
      _context.Add(user);
      await _context.SaveChangesAsync();
    }
  }
}
