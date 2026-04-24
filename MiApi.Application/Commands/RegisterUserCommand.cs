using MiApi.Domain.Ports;
using MiApi.Domain.Entities;

namespace MiApi.Application.Commands
{
  public class RegisterUserCommand
  {
    private readonly IUserRepository _userRepository;

    public RegisterUserCommand(IUserRepository userRepository)
    {
      _userRepository = userRepository;
    }

    public async Task Execute(User user)
    {
      await _userRepository.AddAsync(user);
    }
  }
}