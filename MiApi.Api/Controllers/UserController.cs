using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MiApi.Application.Commands;
using MiApi.Application.DTOs;
using MiApi.Application.Mappers;
using MiApi.Domain.Entities;

namespace MiApi.Api.Controllers
{
    [Route("users")]
    [ApiController]
    public class UserController(RegisterUserCommand registerUserCommand) : ControllerBase
    {
        private readonly RegisterUserCommand _registerUserCommand = registerUserCommand;

        [HttpPost]
        public async Task<IActionResult> RegisterUser([FromBody] RegisterUserDto userDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var user = UserMapper.MapToDomain(userDto);
            await _registerUserCommand.Execute(user);

            return Ok(userDto);
        }

    }
}
