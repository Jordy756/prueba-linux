using MiApi.Application.DTOs;
using MiApi.Domain.Entities;

namespace MiApi.Application.Mappers
{
    public class UserMapper
    {
        public static User MapToDomain(RegisterUserDto userDto)
        {
            return new User(userDto.Id, userDto.Name, userDto.Email);
        }

        public static RegisterUserDto MapToDto(User user)
        {
            return new RegisterUserDto
            {
                Id = user.Id,
                Name = user.Name,
                Email = user.Email
            };
        }
    }
}