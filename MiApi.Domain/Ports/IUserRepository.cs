using MiApi.Domain.Entities;

namespace MiApi.Domain.Ports
{
    public interface IUserRepository
    {
        Task AddAsync(User user);
    }
}
