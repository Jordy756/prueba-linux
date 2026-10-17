using MiApi.Domain.Ports;
using MiApi.Domain.Entities;

namespace MiApi.Application.Commands
{
  public class RegisterUserCommand(IUserRepository userRepository)
    {
    private readonly IUserRepository _userRepository = userRepository;

    public async Task Execute(User user)
    {
      await _userRepository.AddAsync(user);
    }
  }
}