using MiApi.Domain.Entities;
using MiApi.Domain.Ports;
using MiApi.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace MiApi.Infrastructure.Adapters
{
    public class UserRepository : IUserRepository
    {
        private readonly ApplicationDbContext _context;

        public UserRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(User user)
        {
            _context.Add(user);
            await _context.SaveChangesAsync();
        }
    }
}
